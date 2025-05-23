using ModelTracKer.Dto;

namespace ModelTracKer.Services.Interfaces
{
    public interface ITrackerService
    {
        Task<IEnumerable<TrackerOutputDto>> GetAllAsync();
        Task<TrackerOutputDto?> GetByIdAsync(int id);
        Task<TrackerOutputDto> CreateAsync(TrackerInputDto dto);
        Task<bool> UpdateAsync(int id, TrackerInputDto dto);
        Task<bool> PatchAsync(int id, TrackerPatchDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
