

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelTracKer.Dto;
using ModelTracKer.Models;
using ModelTracKer.Services.Interfaces;

namespace ModelTracKer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly ITrackerService _trackerService;

        public TrackerController(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        // GET: api/tracker
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackerOutputDto>>> GetAll()
        {
            var result = await _trackerService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/tracker/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackerOutputDto>> GetById(int id)
        {
            var result = await _trackerService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // POST: api/tracker
        [HttpPost]
        public async Task<ActionResult<TrackerOutputDto>> Create([FromBody] TrackerInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _trackerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/tracker/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TrackerInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _trackerService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // PATCH: api/tracker/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] TrackerPatchDto dto)
        {
            var patched = await _trackerService.PatchAsync(id, dto);
            if (!patched)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/tracker/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _trackerService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }

}


/*
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelTracKer.Dto;
using ModelTracKer.Models;

namespace ModelTracKer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly TrackerDbContext _context;

        public TrackerController(TrackerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackerOutputDto>>> GetTrackers()
        {
            var trackers = await _context.Trackers
                .Include(t => t.GenAiTool)
                .Include(t => t.ReasonForNoGenAiAdoptation)
                .Include(t => t.oppAccelerators)
                    .ThenInclude(o => o.accelerators)
                .Include(t => t.oppMicroservices)
                    .ThenInclude(o => o.microService)
                .Include(t => t.oppCompetition)
                    .ThenInclude(o => o.competition)
                .ToListAsync();

            var result = trackers.Select(MapToOutputDto).ToList();
            return Ok(result);
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackerOutputDto>> GetTracker(int id)
        {
            var tracker = await GetTrackerWithIncludesAsync(id);
            if (tracker == null) return NotFound();

            var result = MapToOutputDto(tracker);
            return Ok(result);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateTracker([FromBody] TrackerInputDto dto)
        {
            var tracker = CreateTrackerEntity(dto);

            _context.Trackers.Add(tracker);
            await _context.SaveChangesAsync(); // to get the generated Id

            AddRelations(tracker.Id, dto);
            await _context.SaveChangesAsync();

            return Ok(tracker.Id);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTracker(int id, [FromBody] TrackerInputDto dto)
        {
            var tracker = await GetTrackerWithIncludesAsync(id);
            if (tracker == null) return NotFound();

            UpdateTrackerFields(tracker, dto);
            ReplaceRelations(tracker, dto);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTracker(int id, [FromBody] TrackerPatchDto dto)
        {
            var tracker = await GetTrackerWithIncludesAsync(id);
            if (tracker == null) return NotFound();

            PatchTrackerFields(tracker, dto);
            PatchRelations(tracker, dto);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTracker(int id)
        {
            var tracker = await GetTrackerWithIncludesAsync(id);
            if (tracker == null) return NotFound();

            _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
            _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
            _context.OppCompetitions.RemoveRange(tracker.oppCompetition);
            _context.Trackers.Remove(tracker);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ================== Private Helper Methods ===================

        private async Task<Tracker?> GetTrackerWithIncludesAsync(int id)
        {
            return await _context.Trackers
                .Include(t => t.GenAiTool)
                .Include(t => t.ReasonForNoGenAiAdoptation)
                .Include(t => t.oppAccelerators).ThenInclude(o => o.accelerators)
                .Include(t => t.oppMicroservices).ThenInclude(o => o.microService)
                .Include(t => t.oppCompetition).ThenInclude(o => o.competition)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        private TrackerOutputDto MapToOutputDto(Tracker tracker)
        {
            return new TrackerOutputDto
            {
                Id = tracker.Id,
                Tracker_id = tracker.Tracker_id,
                Tracker_Name = tracker.Tracker_Name,
                Client_Name = tracker.Client_Name,
                Investment = tracker.Investment,
                GenAiAdoptation = tracker.GenAiAdoptation,
                GenAiToolName = tracker.GenAiTool?.Name,
                ReasonForNoGenAiAdoptationName = tracker.ReasonForNoGenAiAdoptation?.Name,
                OppAccelerators = tracker.oppAccelerators?
                    .Where(o => o.accelerators != null).Select(o => o.accelerators.Name).ToList(),
                OppMicroservices = tracker.oppMicroservices?
                    .Where(o => o.microService != null).Select(o => o.microService.Name).ToList(),
                OppCompetitions = tracker.oppCompetition?
                    .Where(o => o.competition != null).Select(o => o.competition.Name).ToList()
            };
        }

        private Tracker CreateTrackerEntity(TrackerInputDto dto)
        {
            return new Tracker
            {
                Tracker_id = dto.Tracker_id,
                Tracker_Name = dto.Tracker_Name,
                Client_Name = dto.Client_Name,
                Investment = dto.Investment,
                GenAiAdoptation = dto.GenAiAdoptation,
                GenAiTool_Id = dto.GenAiTool_Id,
                ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id
            };
        }

        private void AddRelations(int trackerId, TrackerInputDto dto)
        {
            if (dto.OppAcceleratorIds != null)
            {
                _context.OppAccelerators.AddRange(dto.OppAcceleratorIds.Select(id =>
                    new opp_Accelerator { TrackerId = trackerId, AcceleratorId = id }));
            }

            if (dto.OppMicroserviceIds != null)
            {
                _context.OppMicroservices.AddRange(dto.OppMicroserviceIds.Select(id =>
                    new opp_microservice { TrackerId = trackerId, MicroserviceId = id }));
            }

            if (dto.OppCompetitionIds != null)
            {
                _context.OppCompetitions.AddRange(dto.OppCompetitionIds.Select(id =>
                    new opp_competition { TrackerId = trackerId, CompetitionId = id }));
            }
        }

        private void UpdateTrackerFields(Tracker tracker, TrackerInputDto dto)
        {
            tracker.Tracker_id = dto.Tracker_id;
            tracker.Tracker_Name = dto.Tracker_Name;
            tracker.Client_Name = dto.Client_Name;
            tracker.Investment = dto.Investment;
            tracker.GenAiAdoptation = dto.GenAiAdoptation;
            tracker.GenAiTool_Id = dto.GenAiTool_Id;
            tracker.ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id;
        }

        private void ReplaceRelations(Tracker tracker, TrackerInputDto dto)
        {
            _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
            _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
            _context.OppCompetitions.RemoveRange(tracker.oppCompetition);

            AddRelations(tracker.Id, dto);
        }

        private void PatchTrackerFields(Tracker tracker, TrackerPatchDto dto)
        {
            if (dto.Tracker_id.HasValue) tracker.Tracker_id = dto.Tracker_id.Value;
            if (dto.Tracker_Name != null) tracker.Tracker_Name = dto.Tracker_Name;
            if (dto.Client_Name != null) tracker.Client_Name = dto.Client_Name;
            if (dto.Investment.HasValue) tracker.Investment = dto.Investment.Value;
            if (dto.GenAiAdoptation.HasValue) tracker.GenAiAdoptation = dto.GenAiAdoptation.Value;
            if (dto.GenAiTool_Id.HasValue) tracker.GenAiTool_Id = dto.GenAiTool_Id.Value;
            if (dto.ReasonForNoGenAiAdoptation_Id.HasValue) tracker.ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id.Value;
        }

        private void PatchRelations(Tracker tracker, TrackerPatchDto dto)
        {
            if (dto.OppAcceleratorIds != null)
            {
                _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
                tracker.oppAccelerators = dto.OppAcceleratorIds.Select(id =>
                    new opp_Accelerator { TrackerId = tracker.Id, AcceleratorId = id }).ToList();
            }

            if (dto.OppMicroserviceIds != null)
            {
                _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
                tracker.oppMicroservices = dto.OppMicroserviceIds.Select(id =>
                    new opp_microservice { TrackerId = tracker.Id, MicroserviceId = id }).ToList();
            }

            if (dto.OppCompetitionIds != null)
            {
                _context.OppCompetitions.RemoveRange(tracker.oppCompetition);
                tracker.oppCompetition = dto.OppCompetitionIds.Select(id =>
                    new opp_competition { TrackerId = tracker.Id, CompetitionId = id }).ToList();
            }
        }
    }
}
*/



/*
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelTracKer.Dto;
using ModelTracKer.Models;

namespace ModelTracKer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly TrackerDbContext _context;

        public TrackerController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackerOutputDto>> GetTracker(int id)
        {
            var tracker = await _context.Trackers
                .Include(t => t.GenAiTool)
                .Include(t => t.ReasonForNoGenAiAdoptation)
                .Include(t => t.oppAccelerators)
                    .ThenInclude(o => o.accelerators)
                .Include(t => t.oppMicroservices)
                    .ThenInclude(o => o.microService)
                .Include(t => t.oppCompetition)
                    .ThenInclude(o => o.competition)
                .FirstOrDefaultAsync(t => t.Id == id);


            if (tracker == null) return NotFound();

            var result = new TrackerOutputDto
            {
                Id = tracker.Id,
                Tracker_id = tracker.Tracker_id,
                Tracker_Name = tracker.Tracker_Name,
                Client_Name = tracker.Client_Name,
                Investment = tracker.Investment,
                GenAiAdoptation = tracker.GenAiAdoptation,
                GenAiToolName = tracker.GenAiTool?.Name,
                ReasonForNoGenAiAdoptationName = tracker.ReasonForNoGenAiAdoptation?.Name,
                OppAccelerators = tracker.oppAccelerators?
                .Where(o => o.accelerators != null)
                .Select(o => o.accelerators.Name).ToList(),
                    OppMicroservices = tracker.oppMicroservices?
                .Where(o => o.microService != null)
                .Select(o => o.microService.Name).ToList(),
                    OppCompetitions = tracker.oppCompetition?
                .Where(o => o.competition != null)
                .Select(o => o.competition.Name).ToList()
                };

            return Ok(result);
        }


        // POST
        [HttpPost]
        public async Task<IActionResult> CreateTracker([FromBody] TrackerInputDto dto)
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
            await _context.SaveChangesAsync(); // Save first to get the generated tracker.Id

            // Now insert into connection tables manually
            if (dto.OppAcceleratorIds != null)
            {
                foreach (var accId in dto.OppAcceleratorIds)
                {
                    var opp = new opp_Accelerator
                    {
                        TrackerId = tracker.Id,
                        AcceleratorId = accId
                    };
                    _context.OppAccelerators.Add(opp);
                }
            }

            if (dto.OppMicroserviceIds != null)
            {
                foreach (var msId in dto.OppMicroserviceIds)
                {
                    var oppMs = new opp_microservice
                    {
                        TrackerId = tracker.Id,
                        MicroserviceId = msId
                    };
                    _context.OppMicroservices.Add(oppMs);
                }
            }

            if (dto.OppCompetitionIds != null)
            {
                foreach (var compId in dto.OppCompetitionIds)
                {
                    var oppComp = new opp_competition
                    {
                        TrackerId = tracker.Id,
                        CompetitionId = compId
                    };
                    _context.OppCompetitions.Add(oppComp);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(tracker.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTracker(int id, [FromBody] TrackerInputDto dto)
        {
            var tracker = await _context.Trackers
                .Include(t => t.oppAccelerators)
                .Include(t => t.oppMicroservices)
                .Include(t => t.oppCompetition)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tracker == null)
                return NotFound();

            // Update scalar properties
            tracker.Tracker_id = dto.Tracker_id;
            tracker.Tracker_Name = dto.Tracker_Name;
            tracker.Client_Name = dto.Client_Name;
            tracker.Investment = dto.Investment;
            tracker.GenAiAdoptation = dto.GenAiAdoptation;
            tracker.GenAiTool_Id = dto.GenAiTool_Id;
            tracker.ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id;

            // Remove old connections
            _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
            _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
            _context.OppCompetitions.RemoveRange(tracker.oppCompetition);

            // Add new connections
            tracker.oppAccelerators = dto.OppAcceleratorIds?.Select(id => new opp_Accelerator
            {
                TrackerId = tracker.Id,
                AcceleratorId = id
            }).ToList();

            tracker.oppMicroservices = dto.OppMicroserviceIds?.Select(id => new opp_microservice
            {
                TrackerId = tracker.Id,
                MicroserviceId = id
            }).ToList();

            tracker.oppCompetition = dto.OppCompetitionIds?.Select(id => new opp_competition
            {
                TrackerId = tracker.Id,
                CompetitionId = id
            }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTracker(int id, [FromBody] TrackerPatchDto dto)
        {
            var tracker = await _context.Trackers
                .Include(t => t.oppAccelerators)
                .Include(t => t.oppMicroservices)
                .Include(t => t.oppCompetition)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tracker == null)
                return NotFound();

            // Only update if not null
            if (dto.Tracker_id != null)
                tracker.Tracker_id = (int) dto.Tracker_id; 

            if (dto.Tracker_Name != null)
                tracker.Tracker_Name = dto.Tracker_Name;

            if (dto.Client_Name != null)
                tracker.Client_Name = dto.Client_Name;

            if (dto.Investment.HasValue)
                tracker.Investment = dto.Investment.Value;          

            if (dto.GenAiAdoptation.HasValue)
                tracker.GenAiAdoptation = dto.GenAiAdoptation.Value;

            if (dto.GenAiTool_Id.HasValue)
                tracker.GenAiTool_Id = dto.GenAiTool_Id.Value;

            if (dto.ReasonForNoGenAiAdoptation_Id.HasValue)
                tracker.ReasonForNoGenAiAdoptation_Id = dto.ReasonForNoGenAiAdoptation_Id.Value;

            // If new IDs are passed, update them
            if (dto.OppAcceleratorIds != null)
            {
                _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
                tracker.oppAccelerators = dto.OppAcceleratorIds.Select(id => new opp_Accelerator
                {
                    TrackerId = tracker.Id,
                    AcceleratorId = id
                }).ToList();
            }

            if (dto.OppMicroserviceIds != null)
            {
                _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
                tracker.oppMicroservices = dto.OppMicroserviceIds.Select(id => new opp_microservice
                {
                    TrackerId = tracker.Id,
                    MicroserviceId = id
                }).ToList();
            }

            if (dto.OppCompetitionIds != null)
            {
                _context.OppCompetitions.RemoveRange(tracker.oppCompetition);
                tracker.oppCompetition = dto.OppCompetitionIds.Select(id => new opp_competition
                {
                    TrackerId = tracker.Id,
                    CompetitionId = id
                }).ToList();
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTracker(int id)
        {
            var tracker = await _context.Trackers
                .Include(t => t.oppAccelerators)
                .Include(t => t.oppMicroservices)
                .Include(t => t.oppCompetition)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tracker == null)
                return NotFound();

            // Remove related join table records first
            _context.OppAccelerators.RemoveRange(tracker.oppAccelerators);
            _context.OppMicroservices.RemoveRange(tracker.oppMicroservices);
            _context.OppCompetitions.RemoveRange(tracker.oppCompetition);

            // Remove tracker
            _context.Trackers.Remove(tracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
*/