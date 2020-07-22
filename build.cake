//////////////////////////////////////////////////////////////////////
// NUGET PACKAGES
//////////////////////////////////////////////////////////////////////
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#tool nuget:?package=JetBrains.dotCover.CommandLineTools&version=2019.2.3
#tool nuget:?package=ReportGenerator&version=4.3.6

//////////////////////////////////////////////////////////////////////
// CAKE ADDINS
//////////////////////////////////////////////////////////////////////
#addin nuget:?package=Cake.Powershell&version=0.4.8
#addin nuget:?package=Cake.FileHelpers&version=3.3.0

//////////////////////////////////////////////////////////////////////
// CONFIGURATION
//////////////////////////////////////////////////////////////////////
var targetProject = "Kapture";
var targetDir = @"D:\workspaces\Kapture\lib";

//////////////////////////////////////////////////////////////////////
// ARGUMENT VARIABLES
//////////////////////////////////////////////////////////////////////
var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");
var verbosity = Argument<string> ("verbosity", "Information");

//////////////////////////////////////////////////////////////////////
// OTHER GLOBAL VARIABLES
//////////////////////////////////////////////////////////////////////
var projectNamespace = "ACT_FFXIV_" + targetProject + ".Aetherbridge";
var projectAssemblyName = targetProject + ".Aetherbridge";
var projectMocksAssemblyName = projectAssemblyName + ".Mocks";
var projectName = "Aetherbridge";
var projectMocksName = "Aetherbridge.Mocks";
var buildDir = Directory ("./src") + Directory (projectName) + Directory ("bin") + Directory (configuration);
var assemblyFile = Directory ("./src") + Directory (projectName) + Directory ("bin") + Directory (configuration) + File (projectAssemblyName + ".dll");
var assemblyMockFile = Directory ("./src") + Directory (projectMocksName) + Directory ("bin") + Directory (configuration) + File (projectMocksAssemblyName + ".dll");
var solutionFile = Directory ("./src") + File (projectName + ".sln");
var testResultDir = Directory ("./test-result");
var testResultFile = testResultDir + File ("NUnitResults.xml");
var coverageReportXML = testResultDir + File ("result.xml");
var buildSuccess = true;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task ("Update-Assembly")
    .Does (() => {
        ReplaceRegexInFiles("./src/**/**/*", "ACT_FFXIV.Aetherbridge", projectNamespace);
        ReplaceRegexInFiles("./src/**/**/*", "<AssemblyName>Aetherbridge</AssemblyName>", "<AssemblyName>" + projectAssemblyName + "</AssemblyName>");
        ReplaceRegexInFiles("./src/**/**/*", "<AssemblyName>Aetherbridge.Mocks</AssemblyName>", "<AssemblyName>" + projectAssemblyName + ".Mocks</AssemblyName>");
        Information ("Assembly references updated.");
    });

Task ("Clean")
    .IsDependentOn ("Update-Assembly")
    .Does (() => {
        CleanDirectory (buildDir);
        Information ("Build directory cleaned.");
    });

Task ("Restore-Nuget-Packages")
    .IsDependentOn ("Clean")
    .Does (() => {
        NuGetRestore (solutionFile);
    });

Task ("Build")
    .IsDependentOn ("Restore-Nuget-Packages")
    .Does (() => {
        MSBuild (solutionFile, settings =>
            settings.SetConfiguration (configuration));
    })
    .OnError (exception => {
        buildSuccess = false;
    });

Task ("Run-Unit-Tests")
    .WithCriteria (buildSuccess)
    .IsDependentOn ("Build")
    .Does (() => {
        MSBuild (solutionFile, settings =>
            settings.SetConfiguration ("Debug"));
        var testsPath = "./src/**/bin/Debug/*.Test.dll";
        var coverageReportDCVR = testResultDir + File ("result.dcvr");
        DotCoverCover (tool => {
                tool.NUnit3 (
                    testsPath, 
                    new NUnit3Settings {
                        Results = new [] { new NUnit3Result { FileName = testResultFile } },
                        ShadowCopy = false
                    });
            },
            new FilePath (coverageReportDCVR),
            new DotCoverCoverSettings ()
                .WithFilter("+:" + projectName)
                .WithFilter("-:" + projectName + ".Test"));
        DotCoverReport (new FilePath (coverageReportDCVR),
            new FilePath (coverageReportXML),
            new DotCoverReportSettings {
                ReportType = DotCoverReportType.DetailedXML
            });
    })
    .OnError (exception => {
        buildSuccess = false;
    });

Task ("Copy-Assembly")
    .IsDependentOn ("Run-Unit-Tests")
    .Does (() => {
        CopyFileToDirectory(assemblyFile, targetDir);
        CopyFileToDirectory(assemblyMockFile, targetDir);
        Information ("Copied assembly to target project.");
    });

Task ("Check-Build-Status")
    .IsDependentOn ("Copy-Assembly")
    .Does (() => {
        if (buildSuccess) {
            Information ("Tasks complete.");
        } else {
            throw new Exception ("Build failed.");
        }
    });

Task ("Default")
    .IsDependentOn ("Update-Assembly")
    .IsDependentOn ("Clean")
    .IsDependentOn ("Restore-Nuget-Packages")
    .IsDependentOn ("Build")
    .IsDependentOn ("Run-Unit-Tests")
    .IsDependentOn ("Copy-Assembly")
    .IsDependentOn ("Check-Build-Status");

RunTarget (target);