using ModelTracKer.Dto;

namespace ModelTracKer.Repositories.Interfaces
{
    public interface ITrackerRepository
    {
        Task<IEnumerable<TrackerOutputDto>> GetAllAsync();
        Task<TrackerOutputDto?> GetByIdAsync(int id);
        Task<TrackerOutputDto> CreateAsync(TrackerInputDto dto);
        Task<bool> UpdateAsync(int id, TrackerInputDto dto);
        Task<bool> PatchAsync(int id, TrackerPatchDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
