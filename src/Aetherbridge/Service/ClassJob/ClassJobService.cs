using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
    public class ClassJobService : IClassJobService
    {
        private IGameDataRepository<FFXIV.CrescentCove.ClassJob> _repository;

        public ClassJobService(IGameDataRepository<FFXIV.CrescentCove.ClassJob> repository)
        {
            _repository = repository;
        }

        public ClassJob GetClassJobById(int id)
        {
            return ClassJobMapper.MapToClassJob(_repository.GetById(id));
        }

        public void DeInit()
        {
            _repository = null;
        }
    }
}