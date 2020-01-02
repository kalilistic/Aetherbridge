using System.Collections.Generic;
using System.Linq;

namespace ACT_FFXIV_Aetherbridge
{
    public static class ClassJobMapper
    {
        public static List<ClassJob> MapToClassJobs(List<FFXIV.CrescentCove.ClassJob> classJobs)
        {
            return classJobs?.Select(MapToClassJob).ToList();
        }

        public static ClassJob MapToClassJob(FFXIV.CrescentCove.ClassJob classJobs)
        {
            if (classJobs == null) return null;
            return new ClassJob
            {
                Id = classJobs.Id,
                Name = classJobs.Name,
                Abbreviation = classJobs.Abbreviation
            };
        }
    }
}