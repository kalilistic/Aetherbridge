using FFXIV.CrescentCove;
using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test.Service.ClassJob
{
    [TestFixture]
    public class ClassJobServiceTest
    {
        [SetUp]
        public void TestInitialize()
        {
            const string classJobsStr = "2pugilistPGL30Pugilist2";
            IGameDataRepository<FFXIV.CrescentCove.ClassJob> classJobRepository =
                new GameDataRepository<FFXIV.CrescentCove.ClassJob>(classJobsStr);
            _classJobService = new ClassJobService(classJobRepository);
        }

        private ClassJobService _classJobService;

        [Test]
        public void DeInit_SetsNull()
        {
            _classJobService.DeInit();
        }

        [Test]
        public void GetClassJobByID_BadID_ReturnsNull()
        {
            var classJob = _classJobService.GetClassJobById(-1);
            Assert.IsNull(classJob);
        }

        [Test]
        public void GetClassJobByID_ReturnsClassJob()
        {
            var classJob = _classJobService.GetClassJobById(2);
            Assert.AreEqual("PGL", classJob.Abbreviation);
        }
    }
}