using System.Collections.Generic;
using System.Linq;

namespace ACT_FFXIV_Aetherbridge
{
	public static class ClassJobMapper
	{
		public static List<ClassJob> MapToClassJobs(List<FFXIV.CrescentCove.ClassJob> classJobs, ILanguage language)
		{
			return classJobs.Select(classJob => MapToClassJob(classJob, language)).ToList();
		}

		public static ClassJob MapToClassJob(FFXIV.CrescentCove.ClassJob classJob, ILanguage language)
		{
			if (classJob == null) return null;
			return new ClassJob
			{
				Id = classJob.Id,
				Name = classJob.Localized[language.Index].Name,
				Abbreviation = classJob.Localized[language.Index].Abbreviation
			};
		}
	}
}