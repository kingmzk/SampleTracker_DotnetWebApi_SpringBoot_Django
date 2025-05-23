using Microsoft.EntityFrameworkCore;
using ModelTracKer.Data;
using ModelTracKer.Dto;
using ModelTracKer.Models;
using ModelTracKer.Repositories.Interfaces;

namespace ModelTracKer.Repositories
{
    public class TrackerRepository : ITrackerRepository
    {
        private readonly TrackerDbContext _context;

        public TrackerRepository(TrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrackerOutputDto>> GetAllAsync()
        {
            return await _context.Trackers
                .Select(t => new TrackerOutputDto
                {
                    Id = t.Id,
                    Tracker_id = t.Tracker_id,
                    Tracker_Name = t.Tracker_Name,
                    Client_Name = t.Client_Name,
                    Investment = t.Investment,
                    GenAiAdoptation = t.GenAiAdoptation,
                    GenAiToolName = t.GenAiTool.Name,
                    ReasonForNoGenAiAdoptationName = t.ReasonForNoGenAiAdoptation.Name,
                    OppAccelerators = t.oppAccelerators.Select(o => o.accelerators.Name).ToList(),
                    OppMicroservices = t.oppMicroservices.Select(o => o.microservice.Name).ToList(),
                    OppCompetitions = t.oppCompetitions.Select(o => o.competition.Name).ToList()
                }).ToListAsync();
        }

        public async Task<TrackerOutputDto?> GetByIdAsync(int id)
        {
            var t = await _context.Trackers
                .Include(t => t.GenAiTool)
                .Include(t => t.ReasonForNoGenAiAdoptation)
                .Include(t => t.oppAccelerators).ThenInclude(o => o.accelerators)
                .Include(t => t.oppMicroservices).ThenInclude(o => o.microservice)
                .Include(t => t.oppCompetitions).ThenInclude(o => o.competition)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (t == null) return null;

            return new TrackerOutputDto
            {
                Id = t.Id,
                Tracker_id = t.Tracker_id,
                Tracker_Name = t.Tracker_Name,
                Client_Name = t.Client_Name,
                Investment = t.Investment,
                GenAiAdoptation = t.GenAiAdoptation,
                GenAiToolName = t.GenAiTool?.Name,
                ReasonForNoGenAiAdoptationName = t.ReasonForNoGenAiAdoptation?.Name,
                OppAccelerators = t.oppAccelerators
                    ?.Where(o => o.accelerators != null)
                    .Select(o => o.accelerators.Name)
                    .ToList() ?? new List<string>(),

                OppMicroservices = t.oppMicroservices
                    ?.Where(o => o.microservice != null)
                    .Select(o => o.microservice.Name)
                    .ToList() ?? new List<string>(),

                OppCompetitions = t.oppCompetitions
                    ?.Where(o => o.competition != null)
                    .Select(o => o.competition.Name)
                    .ToList() ?? new List<string>()
            };
        }


        public async Task<TrackerOutputDto> CreateAsync(TrackerInputDto dto)
        {
            var tracker = new Tracker
            {
                Tracker_id = dto.Tracker_id,
                Tracker_Name = dto.Tracker_Name,
                Client_Name = dto.Client_Name,
                Investment = dto.Investment,
                GenAiAdoptation = dto.GenAiAdoptation,
                GenAiTool_Id = dto.GenAiTool_Id,
                ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id
            };

            _context.Trackers.Add(tracker);
            await _context.SaveChangesAsync(); // Save to get tracker.Id

            // Add opp_accelerators
            if (dto.OppAcceleratorIds != null)
            {
                foreach (var acceleratorId in dto.OppAcceleratorIds)
                {
                    _context.OppAccelerators.Add(new opp_Accelerator
                    {
                        TrackerId = tracker.Id,
                        AcceleratorId = acceleratorId
                    });
                }
            }

            // Add opp_microservices
            if (dto.OppMicroserviceIds != null)
            {
                foreach (var microserviceId in dto.OppMicroserviceIds)
                {
                    _context.OppMicroservices.Add(new opp_microservice
                    {
                        TrackerId = tracker.Id,
                        MicroserviceId = microserviceId
                    });
                }
            }

            // Add opp_competitions
            if (dto.OppCompetitionIds != null)
            {
                foreach (var competitionId in dto.OppCompetitionIds)
                {
                    _context.OppCompetitions.Add(new opp_competition
                    {
                        TrackerId = tracker.Id,
                        CompetitionId = competitionId
                    });
                }
            }

            await _context.SaveChangesAsync();

            // Fetch the tracker again with navigation properties included
            var createdTracker = await _context.Trackers
                .Include(t => t.GenAiTool)
                .Include(t => t.ReasonForNoGenAiAdoptation)
                .Include(t => t.oppAccelerators).ThenInclude(oa => oa.accelerators)
                .Include(t => t.oppMicroservices).ThenInclude(om => om.microservice)
                .Include(t => t.oppCompetitions).ThenInclude(oc => oc.competition)
                .FirstOrDefaultAsync(t => t.Id == tracker.Id);

            return new TrackerOutputDto
            {
                Id = createdTracker.Id,
                Tracker_id = createdTracker.Tracker_id,
                Tracker_Name = createdTracker.Tracker_Name,
                Client_Name = createdTracker.Client_Name,
                Investment = createdTracker.Investment,
                GenAiAdoptation = createdTracker.GenAiAdoptation,
                GenAiToolName = createdTracker.GenAiTool?.Name,
                ReasonForNoGenAiAdoptationName = createdTracker.ReasonForNoGenAiAdoptation?.Name,
                OppAccelerators = createdTracker.oppAccelerators?.Select(a => a.accelerators.Name).ToList() ?? new List<string>(),
                OppMicroservices = createdTracker.oppMicroservices?.Select(m => m.microservice.Name).ToList() ?? new List<string>(),
                OppCompetitions = createdTracker.oppCompetitions?.Select(c => c.competition.Name).ToList() ?? new List<string>()
            };
        }



        public async Task<bool> UpdateAsync(int id, TrackerInputDto dto)
        {
            var tracker = await _context.Trackers
                .Include(t => t.oppAccelerators)
                .Include(t => t.oppMicroservices)
                .Include(t => t.oppCompetitions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tracker == null) return false;

            // Update basic fields
            tracker.Tracker_id = dto.Tracker_id;
            tracker.Tracker_Name = dto.Tracker_Name;
            tracker.Client_Name = dto.Client_Name;
            tracker.Investment = dto.Investment;
            tracker.GenAiAdoptation = dto.GenAiAdoptation;
            tracker.GenAiTool_Id = dto.GenAiTool_Id;
            tracker.ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id;

            // Clear and re-add related oppAccelerators
            tracker.oppAccelerators.Clear();
            if (dto.OppAcceleratorIds != null)
            {
                foreach (var acceleratorId in dto.OppAcceleratorIds)
                {
                    tracker.oppAccelerators.Add(new opp_Accelerator
                    {
                        TrackerId = tracker.Id,
                        AcceleratorId = acceleratorId
                    });
                }
            }

            // Clear and re-add related oppMicroservices
            tracker.oppMicroservices.Clear();
            if (dto.OppMicroserviceIds != null)
            {
                foreach (var microserviceId in dto.OppMicroserviceIds)
                {
                    tracker.oppMicroservices.Add(new opp_microservice
                    {
                        TrackerId = tracker.Id,
                        MicroserviceId = microserviceId
                    });
                }
            }

            // Clear and re-add related oppCompetitions
            tracker.oppCompetitions.Clear();
            if (dto.OppCompetitionIds != null)
            {
                foreach (var competitionId in dto.OppCompetitionIds)
                {
                    tracker.oppCompetitions.Add(new opp_competition
                    {
                        TrackerId = tracker.Id,
                        CompetitionId = competitionId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> PatchAsync(int id, TrackerPatchDto dto)
        {
            var tracker = await _context.Trackers
                .Include(t => t.oppAccelerators)
                .Include(t => t.oppMicroservices)
                .Include(t => t.oppCompetitions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tracker == null) return false;

            // Patch scalar properties
            if (dto.Tracker_id.HasValue) tracker.Tracker_id = dto.Tracker_id.Value;
            if (!string.IsNullOrWhiteSpace(dto.Tracker_Name)) tracker.Tracker_Name = dto.Tracker_Name;
            if (!string.IsNullOrWhiteSpace(dto.Client_Name)) tracker.Client_Name = dto.Client_Name;
            if (dto.Investment.HasValue) tracker.Investment = dto.Investment.Value;
            if (dto.GenAiAdoptation.HasValue) tracker.GenAiAdoptation = dto.GenAiAdoptation.Value;
            if (dto.GenAiTool_Id.HasValue) tracker.GenAiTool_Id = dto.GenAiTool_Id.Value;
            if (dto.ReasonForNoGenAiAdoptation_Id.HasValue) tracker.ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id.Value;

            // Patch oppAccelerators
            if (dto.OppAcceleratorIds != null)
            {
                tracker.oppAccelerators.Clear();
                foreach (var idValue in dto.OppAcceleratorIds)
                {
                    tracker.oppAccelerators.Add(new opp_Accelerator
                    {
                        TrackerId = tracker.Id,
                        AcceleratorId = idValue
                    });
                }
            }

            // Patch oppMicroservices
            if (dto.OppMicroserviceIds != null)
            {
                tracker.oppMicroservices.Clear();
                foreach (var idValue in dto.OppMicroserviceIds)
                {
                    tracker.oppMicroservices.Add(new opp_microservice
                    {
                        TrackerId = tracker.Id,
                        MicroserviceId = idValue
                    });
                }
            }

            // Patch oppCompetitions
            if (dto.OppCompetitionIds != null)
            {
                tracker.oppCompetitions.Clear();
                foreach (var idValue in dto.OppCompetitionIds)
                {
                    tracker.oppCompetitions.Add(new opp_competition
                    {
                        TrackerId = tracker.Id,
                        CompetitionId = idValue
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var tracker = await _context.Trackers
                .Include(t => t.oppAccelerators)
                .Include(t => t.oppMicroservices)
                .Include(t => t.oppCompetitions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tracker == null) return false;

            // Remove related entities if cascade delete is not configured
            _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
            _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
            _context.OppCompetitions.RemoveRange(tracker.oppCompetitions);

            _context.Trackers.Remove(tracker);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
