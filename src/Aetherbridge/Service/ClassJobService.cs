using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class ClassJobService
	{
		private readonly LanguageService _languageService;
		private readonly IGameDataRepository<FFXIV.CrescentCove.ClassJob> _repository;
		private List<List<ClassJob>> _classJobs = new List<List<ClassJob>>();

		public ClassJobService(LanguageService languageService,
			IGameDataRepository<FFXIV.CrescentCove.ClassJob> repository)
		{
			_languageService = languageService;
			_repository = repository;

			var languages = _languageService.GetLanguages();
			foreach (var _ in languages) _classJobs.Add(new List<ClassJob>());
		}

		public void AddLanguage(Language language)
		{
			var crescentClassJobs = _repository.GetAll();
			foreach (var crescentClassJob in crescentClassJobs)
				_classJobs[language.Index].Add(MapToClassJob(crescentClassJob, language));
		}

		public ClassJob GetClassJobById(int id)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			return _classJobs[languageIndex].FirstOrDefault(classJob => classJob.Id == id);
		}

		public void DeInit()
		{
			_classJobs = null;
		}

		public static List<ClassJob> MapToClassJobs(List<FFXIV.CrescentCove.ClassJob> classJobs, Language language)
		{
			return classJobs.Select<FFXIV.CrescentCove.ClassJob, ClassJob>(classJob => MapToClassJob(classJob, language)).ToList();
		}

		public static ClassJob MapToClassJob(FFXIV.CrescentCove.ClassJob classJob, Language language)
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