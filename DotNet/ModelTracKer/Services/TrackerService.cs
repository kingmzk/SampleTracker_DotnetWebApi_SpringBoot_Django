using ModelTracKer.Dto;
using ModelTracKer.Repositories.Interfaces;
using ModelTracKer.Services.Interfaces;

namespace ModelTracKer.Services
{
    public class TrackerService : ITrackerService
    {
        private readonly ITrackerRepository _trackerRepository;

        public TrackerService(ITrackerRepository trackerRepository)
        {
            _trackerRepository = trackerRepository;
        }

        public Task<IEnumerable<TrackerOutputDto>> GetAllAsync() => _trackerRepository.GetAllAsync();

        public Task<TrackerOutputDto?> GetByIdAsync(int id) => _trackerRepository.GetByIdAsync(id);

        public Task<TrackerOutputDto> CreateAsync(TrackerInputDto dto) => _trackerRepository.CreateAsync(dto);

        public Task<bool> UpdateAsync(int id, TrackerInputDto dto) => _trackerRepository.UpdateAsync(id, dto);

        public Task<bool> PatchAsync(int id, TrackerPatchDto dto) => _trackerRepository.PatchAsync(id, dto);

        public Task<bool> DeleteAsync(int id) => _trackerRepository.DeleteAsync(id);
    }
}
