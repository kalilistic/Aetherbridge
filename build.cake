//////////////////////////////////////////////////////////////////////
// NUGET PACKAGES
//////////////////////////////////////////////////////////////////////
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#tool nuget:?package=JetBrains.dotCover.CommandLineTools&version=2019.2.3
#tool nuget:?package=ReportGenerator&version=4.3.6
#tool nuget:?package=Codecov&version=1.8.0
#tool nuget:?package=GitVersion.CommandLine&version=5.1.2

//////////////////////////////////////////////////////////////////////
// CAKE ADDINS
//////////////////////////////////////////////////////////////////////
#addin nuget:?package=Octokit&version=0.32.0
#addin nuget:?package=Cake.Codecov&version=0.7.0
#addin nuget:?package=Cake.Powershell&version=0.4.8

//////////////////////////////////////////////////////////////////////
// IMPORTS
//////////////////////////////////////////////////////////////////////
using Octokit;
using System.Xml;

//////////////////////////////////////////////////////////////////////
// PROJECT VARIABLES
//////////////////////////////////////////////////////////////////////
var projectName = "Aetherbridge";
var authorName = "kalilistic";
var packageName = "Aetherbridge";
var packageDescription = "FFXIV ACT Plugin Library for easier development.";
var packageTags = "FFXIV ACT plugin";

//////////////////////////////////////////////////////////////////////
// ARGUMENT VARIABLES
//////////////////////////////////////////////////////////////////////
var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");
var verbosity = Argument<string> ("verbosity", "Information");

//////////////////////////////////////////////////////////////////////
// ENVIRONMENT VARIABLES
//////////////////////////////////////////////////////////////////////
var githubToken = EnvironmentVariable ("GITHUB_TOKEN");
var nugetToken = EnvironmentVariable ("NUGET_TOKEN");
var appVeyorBuildVersion = EnvironmentVariable ("APPVEYOR_BUILD_VERSION");
var pullRequestNumber = EnvironmentVariable ("APPVEYOR_PULL_REQUEST_NUMBER");
var branchName = EnvironmentVariable ("APPVEYOR_REPO_BRANCH");

//////////////////////////////////////////////////////////////////////
// BUILD SYSTEM VARIABLES
//////////////////////////////////////////////////////////////////////
var isLocalBuild = BuildSystem.IsLocalBuild;
var isAppVeyorBuild = BuildSystem.IsRunningOnAppVeyor;

//////////////////////////////////////////////////////////////////////
// OTHER GLOBAL VARIABLES
//////////////////////////////////////////////////////////////////////
var buildDir = Directory ("./src") + Directory (projectName) + Directory ("bin") + Directory (configuration);
var buildFilePath = Directory (buildDir) + File (projectName + ".dll");
var solutionFile = Directory ("./src") + File (projectName + ".sln");
var testResultDir = Directory ("./test-result");
var testResultFile = testResultDir + File ("NUnitResults.xml");
var coverageReportXML = testResultDir + File ("result.xml");
var nugetDir = buildDir + Directory ("nuget");
var nugetPackage = string.Empty;
var isPullRequest = !string.IsNullOrWhiteSpace (pullRequestNumber);
var isMasterBranch = branchName == "master";
var version = string.Empty;
var buildSuccess = true;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task ("Clean")
    .Does (() => {
        CleanDirectory (buildDir);
        Information ("Build directory cleaned.");
    });

Task ("Restore-Nuget-Packages")
    .IsDependentOn ("Clean")
    .Does (() => {
        NuGetRestore (solutionFile);
    });

Task ("Set-Version")
    .IsDependentOn ("Restore-Nuget-Packages")
    .Does (() => {
        if (isLocalBuild) {
            version = "1.0.0-LOCAL";
        } else if (isPullRequest) {
            version = GitVersion ().SemVer + ".pr." + pullRequestNumber + "+" + appVeyorBuildVersion;
        } else if (!isMasterBranch) {
            version = GitVersion ().SemVer + "+" + appVeyorBuildVersion;
        } else {
            version = GitVersion ().SemVer;
        }
        Information ("Version set to " + version);
    });

Task ("Create-Assembly-Info")
    .WithCriteria (!isLocalBuild)
    .IsDependentOn ("Set-Version")
    .Does (() => {
        GitVersion (new GitVersionSettings {
            UpdateAssemblyInfo = true
        });
        Information ("Assembly info updated.");
    });

Task ("Build")
    .IsDependentOn ("Create-Assembly-Info")
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

Task ("Upload-Test-Results-To-AppVeyor")
    .WithCriteria (buildSuccess)
    .WithCriteria (isAppVeyorBuild)
    .IsDependentOn ("Run-Unit-Tests")
    .Does (() => {
        if (BuildSystem.IsRunningOnAppVeyor) {
            AppVeyor.UploadTestResults (testResultFile, AppVeyorTestResultsType.NUnit3);
            Information ("Test results uploaded to AppVeyor.");
        }
    });

Task ("Upload-Coverage-To-Codecov")
    .WithCriteria (buildSuccess)
    .WithCriteria (!isLocalBuild)
    .IsDependentOn ("Run-Unit-Tests")
    .Does (() => {
        Codecov (coverageReportXML);
    });

Task ("Publish-Github-Release")
    .WithCriteria (buildSuccess)
    .WithCriteria (!isLocalBuild)
    .WithCriteria (!isPullRequest)
    .WithCriteria (isMasterBranch)
    .IsDependentOn ("Run-Unit-Tests")
    .Does (async () => {
        var gitTag = "v" + version;
        var client = new GitHubClient (new ProductHeaderValue (projectName));
        var tokenAuth = new Credentials (githubToken);
        client.Credentials = tokenAuth;
        var newRelease = new NewRelease (gitTag);
        newRelease.Name = "Version " + version;
        newRelease.Body = "This is a pre-release and may not be stable.";
        newRelease.Draft = false;
        newRelease.Prerelease = true;
        await client.Repository.Release.Create (authorName, projectName, newRelease);
        using (var fileStream = System.IO.File.OpenRead (buildFilePath)) {
            var assetUpload = new ReleaseAssetUpload () {
            FileName = projectName + ".dll",
            ContentType = "application/x-msdownload",
            RawData = fileStream
            };
            var release = await client.Repository.Release.Get (authorName, projectName, gitTag);
            await client.Repository.Release.UploadAsset (release, assetUpload);
        }
        Information ("Created GitHub release.");
    });

Task ("Pack-Nuget")
    .WithCriteria (buildSuccess)
    .IsDependentOn ("Run-Unit-Tests")
    .Does (() => {
        CreateDirectory(nugetDir);
        var nugetSpecFile = nugetDir + File (projectName + ".nuspec");
        CopyFile (buildFilePath, nugetDir + File (projectName + ".dll"));
        CopyFile ("./img/packageIcon.png", nugetDir + File ("packageIcon.png"));
        XmlWriterSettings settings = new XmlWriterSettings ();
        settings.Indent = true;
        settings.NewLineOnAttributes = true;
        XmlWriter xmlWriter = XmlWriter.Create (nugetSpecFile, settings);
        xmlWriter.WriteStartDocument ();
        xmlWriter.WriteStartElement ("package");
        xmlWriter.WriteStartElement ("metadata");
        xmlWriter.WriteStartElement ("id");
        xmlWriter.WriteString (packageName);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("version");
        xmlWriter.WriteString (version);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("title");
        xmlWriter.WriteString (packageName);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("authors");
        xmlWriter.WriteString (authorName);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("owners");
        xmlWriter.WriteString (authorName);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("license");
        xmlWriter.WriteAttributeString ("type", "expression");
        xmlWriter.WriteString ("MIT");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("projectUrl");
        xmlWriter.WriteString ("https://github.com/" + authorName + "/" + projectName);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("repository");
        xmlWriter.WriteAttributeString ("type", "git");
        xmlWriter.WriteAttributeString ("url", "https://github.com/" + authorName + "/" + projectName + ".git");
        xmlWriter.WriteAttributeString ("branch", "master");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("icon");
        xmlWriter.WriteString ("packageIcon.png");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("requireLicenseAcceptance");
        xmlWriter.WriteString ("false");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("description");
        xmlWriter.WriteString (packageDescription);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("copyright");
        xmlWriter.WriteString ("Copyright © 2019 " + authorName);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("tags");
        xmlWriter.WriteString (packageTags);
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("dependencies");
        xmlWriter.WriteStartElement ("group");
        xmlWriter.WriteAttributeString ("targetFramework", ".NETFramework4.72");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("files");
        xmlWriter.WriteStartElement ("file");
        xmlWriter.WriteAttributeString ("src", projectName + ".dll");
        xmlWriter.WriteAttributeString ("target", "lib/net472");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteStartElement ("file");
        xmlWriter.WriteAttributeString ("src", "packageIcon.png");
        xmlWriter.WriteEndElement ();
        xmlWriter.WriteEndDocument ();
        xmlWriter.Close ();
        var nugetPackSettings = new NuGetPackSettings {
            BasePath = nugetDir,
            OutputDirectory = nugetDir,
			NoPackageAnalysis = true
        };
        NuGetPack (nugetSpecFile, nugetPackSettings);
        nugetPackage = nugetDir + File (packageName + "." + version + ".nupkg");
    });

Task ("Push-Nuget-To-Github-Packages")
    .WithCriteria (buildSuccess)
    .WithCriteria (!isLocalBuild)
    .WithCriteria (!isPullRequest)
    .WithCriteria (isMasterBranch)
    .IsDependentOn ("Pack-Nuget")
    .Does (() => {
        var packageFeed = "https://nuget.pkg.github.com/" + authorName + "/index.json";
        NuGetAddSource (
            packageName,
            packageFeed,
            new NuGetSourcesSettings {
                UserName = authorName,
                    Password = githubToken
            });
        NuGetPush (nugetPackage, new NuGetPushSettings {
            Source = packageFeed,
                ApiKey = githubToken
        });
    });

Task ("Push-Nuget-To-Nuget-Gallery")
    .WithCriteria (buildSuccess)
    .WithCriteria (!isLocalBuild)
    .WithCriteria (!isPullRequest)
    .WithCriteria (isMasterBranch)
    .IsDependentOn ("Pack-Nuget")
    .Does (() => {
        NuGetPush (nugetPackage, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package/",
                ApiKey = nugetToken
        });
    });

Task ("Update-Artifact-To-AppVeyor")
    .WithCriteria (buildSuccess)
    .WithCriteria (isAppVeyorBuild)
    .IsDependentOn ("Run-Unit-Tests")
    .Does (() => {
        StartPowershellScript ("Push-AppveyorArtifact", args => {
            args.AppendQuoted (buildFilePath);
        });
    });

Task ("Set-AppVeyor-Build-Number")
    .WithCriteria (isAppVeyorBuild)
    .IsDependentOn ("Upload-Coverage-To-Codecov")
    .Does (() => {
        StartPowershellScript ("Update-AppveyorBuild", args => {
            args.Append ("Version", version);
        });
    });

Task ("Check-Build-Status")
    .IsDependentOn ("Set-AppVeyor-Build-Number")
    .Does (() => {
        if (buildSuccess) {
            Information ("Tasks complete.");
        } else {
            throw new Exception ("Build failed.");
        }
    });

Task ("Default")
    .IsDependentOn ("Clean")
    .IsDependentOn ("Restore-Nuget-Packages")
    .IsDependentOn ("Set-Version")
    .IsDependentOn ("Create-Assembly-Info")
    .IsDependentOn ("Build")
    .IsDependentOn ("Run-Unit-Tests")
    .IsDependentOn ("Upload-Test-Results-To-AppVeyor")
    .IsDependentOn ("Upload-Coverage-To-Codecov")
    .IsDependentOn ("Publish-Github-Release")
    .IsDependentOn ("Pack-Nuget")
    .IsDependentOn ("Push-Nuget-To-Github-Packages")
    .IsDependentOn ("Push-Nuget-To-Nuget-Gallery")
    .IsDependentOn ("Update-Artifact-To-AppVeyor")
    .IsDependentOn ("Set-AppVeyor-Build-Number")
    .IsDependentOn ("Check-Build-Status");

RunTarget (target);