package com.tacker.tracker.controller;

import com.tacker.tracker.dto.TrackerDto;
import com.tacker.tracker.service.TrackerService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.tacker.tracker.dto.PatchTrackerDto;
import java.util.List;

@RestController
@RequestMapping("/api/trackers")
public class TrackerController {

    private final TrackerService trackerService;

    public TrackerController(TrackerService trackerService) {
        this.trackerService = trackerService;
    }

    @GetMapping
    public ResponseEntity<List<TrackerDto>> getAllTrackers() {
        return ResponseEntity.ok(trackerService.getAllTrackers());
    }

    @GetMapping("/{id}")
    public ResponseEntity<TrackerDto> getTrackerById(@PathVariable int id) {
        return ResponseEntity.ok(trackerService.getTrackerById(id));
    }

    @PostMapping
    public ResponseEntity<TrackerDto> createTracker(@RequestBody TrackerDto dto) {
        return ResponseEntity.ok(trackerService.createTracker(dto));
    }

    @PutMapping("/{id}")
    public ResponseEntity<TrackerDto> updateTracker(@PathVariable int id, @RequestBody TrackerDto dto) {
        return ResponseEntity.ok(trackerService.updateTracker(id, dto));
    }

    @PatchMapping("/{id}")
    public ResponseEntity<TrackerDto> patchTracker(@PathVariable int id, @RequestBody PatchTrackerDto dto) {
        return ResponseEntity.ok(trackerService.patchTracker(id, dto));
    }

}








/*
package com.tacker.tracker.controller;


import com.tacker.tracker.models.*;
import com.tacker.tracker.repository.*;
import jakarta.persistence.EntityNotFoundException;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.tacker.tracker.dto.TrackerDto;
import com.tacker.tracker.dto.SimpleDto;

import java.util.*;
import java.util.function.BiFunction;
import java.util.function.Function;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/trackers")
public class TrackerController {

    private final TrackerRepository trackerRepository;
    private final GenAiToolRepository genAiToolRepository;
    private final ReasonForNoGenAiAdoptationRepository reasonRepository;
    private final AcceleratorRepository acceleratorRepository;
    private final MicroserviceRepository microserviceRepository;
    private final CompetitionRepository competitionRepository;

    public TrackerController(
            TrackerRepository trackerRepository,
            GenAiToolRepository genAiToolRepository,
            ReasonForNoGenAiAdoptationRepository reasonRepository,
            AcceleratorRepository acceleratorRepository,
            MicroserviceRepository microserviceRepository,
            CompetitionRepository competitionRepository
    ) {
        this.trackerRepository = trackerRepository;
        this.genAiToolRepository = genAiToolRepository;
        this.reasonRepository = reasonRepository;
        this.acceleratorRepository = acceleratorRepository;
        this.microserviceRepository = microserviceRepository;
        this.competitionRepository = competitionRepository;
    }

    // ------------------ GET ALL ------------------
    @GetMapping
    public ResponseEntity<List<TrackerDto>> getAllTrackers() {
        List<Tracker> trackers = trackerRepository.findAll();
        return ResponseEntity.ok(trackers.stream()
                .map(this::convertToDto)
                .collect(Collectors.toList()));
    }

    // ------------------ GET BY ID ------------------
    @GetMapping("/{id}")
    public ResponseEntity<TrackerDto> getTrackerById(@PathVariable int id) {
        return trackerRepository.findById(id)
                .map(this::convertToDto)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }


    // ------------------ POST (Create) ------------------
    @PostMapping
    @Transactional
    public ResponseEntity<TrackerDto> createTracker(@RequestBody TrackerDto dto) { // Remove com.tracker.dto
        Tracker tracker = convertToEntity(dto, new Tracker());
        return ResponseEntity.ok(convertToDto(trackerRepository.save(tracker)));
    }

    // ------------------ PUT (Update) ------------------
    @PutMapping("/{id}")
    @Transactional
    public ResponseEntity<TrackerDto> updateTracker(
            @PathVariable int id,
            @RequestBody TrackerDto dto // Fix package reference
    ) {
        return trackerRepository.findById(id)
                .map(existing -> {
                    clearRelationships(existing);
                    return ResponseEntity.ok(convertToDto(
                            trackerRepository.save(convertToEntity(dto, existing))
                    ));
                })
                .orElse(ResponseEntity.notFound().build());
    }
    private void clearRelationships(Tracker tracker) {
        tracker.getOppAccelerators().clear();
        tracker.getOppMicroservices().clear();
        tracker.getOppCompetitions().clear();
    }

    private TrackerDto convertToDto(Tracker tracker) { // Use correct TrackerDto
        TrackerDto dto = new TrackerDto();
        dto.setId(tracker.getId());
        dto.setTrackerId(tracker.getTrackerId());
        dto.setTrackerName(tracker.getTrackerName());
        dto.setClientName(tracker.getClientName());
        dto.setInvestment(tracker.getInvestment());
        dto.setGenAiAdoptation(tracker.isGenAiAdoptation());


        // Handle GenAI relationships
        if (tracker.getGenAiTool() != null) {
            dto.setGenAiTool(new SimpleDto(tracker.getGenAiTool().getId(),
                    tracker.getGenAiTool().getValue()));
        }
        if (tracker.getReasonForNoGenAiAdoptation() != null) {
            dto.setReasonForNoGenAiAdoptation(
                    new SimpleDto(tracker.getReasonForNoGenAiAdoptation().getId(),
                            tracker.getReasonForNoGenAiAdoptation().getValue())
            );
        }

        // Fix relationship accessors
        dto.setOppAccelerators(convertOppRelationships(
                tracker.getOppAccelerators(),
                opp -> opp.getAccelerator().getId() ,// Changed to getAccelerator()
                opp -> opp.getAccelerator().getValue()
        ));
        dto.setOppMicroservices(convertOppRelationships(
                tracker.getOppMicroservices(),
                opp -> opp.getMicroservice().getId(),
                opp -> opp.getMicroservice().getValue()
        ));
        dto.setOppCompetitions(convertOppRelationships(
                tracker.getOppCompetitions(),
                opp -> opp.getCompetition().getId(),
                opp -> opp.getCompetition().getValue()
        ));

        return dto;
    }

    private <T> List<SimpleDto> convertOppRelationships(
            Collection<T> relationships,
            Function<T, Integer> idExtractor,
            Function<T, String> valueExtractor
    ) {
        return relationships.stream()
                .map(rel -> {
                    try {
                        SimpleDto dto = new SimpleDto();
                        dto.setId(idExtractor.apply(rel));
                        dto.setValue(valueExtractor.apply(rel));
                        return dto;
                    } catch (Exception e) {
                        return null;
                    }
                })
                .filter(Objects::nonNull)
                .collect(Collectors.toList());
    }


    private SimpleDto createSimpleDto(int id) {
        SimpleDto dto = new SimpleDto();
        dto.setId(id);
        return dto;
    }

    private Tracker convertToEntity(TrackerDto dto, Tracker tracker) {
        try {
            tracker.setTrackerId(dto.getTrackerId());
        } catch (NumberFormatException e) {
            throw new IllegalArgumentException("Invalid trackerId format");
        }

        tracker.setTrackerName(dto.getTrackerName());
        tracker.setClientName(dto.getClientName());
        tracker.setInvestment(dto.getInvestment());
        tracker.setGenAiAdoptation(dto.isGenAiAdoptation());

        // Handle relationships with null checks
        if (dto.getGenAiTool() != null) {
            genAiToolRepository.findById(dto.getGenAiTool().getId())
                    .ifPresentOrElse(
                            tracker::setGenAiTool,
                            () -> { throw new EntityNotFoundException("GenAI Tool not found"); }
                    );
        }

        if (dto.getReasonForNoGenAiAdoptation() != null) {
            reasonRepository.findById(dto.getReasonForNoGenAiAdoptation().getId())
                    .ifPresentOrElse(
                            tracker::setReasonForNoGenAiAdoptation,
                            () -> { throw new EntityNotFoundException("Reason not found"); }
                    );
        }

        // Clear existing relationships
        tracker.getOppAccelerators().clear();
        tracker.getOppMicroservices().clear();
        tracker.getOppCompetitions().clear();

        // Add new relationships
        addRelationships(
                dto.getOppAccelerators(),
                acceleratorRepository,
                (acc, t) -> new OppAccelerator(acc, t),
                tracker.getOppAccelerators(),
                tracker
        );

        addRelationships(
                dto.getOppMicroservices(),
                microserviceRepository,
                (ms, t) -> new OppMicroservice(ms, t),
                tracker.getOppMicroservices(),
                tracker
        );

        addRelationships(
                dto.getOppCompetitions(),
                competitionRepository,
                (comp, t) -> new OppCompetition(comp, t),
                tracker.getOppCompetitions(),
                tracker
        );

        return tracker;
    }

    private <T, O> void addRelationships(
            List<SimpleDto> dtos,
            JpaRepository<T, Integer> repo,
            BiFunction<T, Tracker, O> creator,
            Collection<O> collection,
            Tracker tracker
    ) {
        dtos.stream()
                .map(SimpleDto::getId)
                .map(repo::findById)
                .filter(Optional::isPresent)
                .map(Optional::get)
                .map(entity -> creator.apply(entity, tracker))
                .forEach(collection::add);
    }

    private <T> void setEntityRelationship(
            SimpleDto dto,
            JpaRepository<T, Integer> repository,
            java.util.function.Consumer<T> setter
    ) {
        if (dto != null) {
            repository.findById(dto.getId()).ifPresent(setter);
        } else {
            setter.accept(null);
        }
    }

    private <T, O> void updateOppRelationships(
            List<SimpleDto> dtos,
            JpaRepository<T, Integer> repository,
            BiFunction<T, Tracker, O> oppCreator,
            Collection<O> existingCollection,
            Tracker tracker
    ) {
        Set<Integer> ids = dtos.stream()
                .map(SimpleDto::getId)
                .collect(Collectors.toSet());

        // Create temporary list to avoid concurrent modification
        List<O> newRelationships = repository.findAllById(ids)
                .stream()
                .map(entity -> oppCreator.apply(entity, tracker))
                .toList();

        existingCollection.clear();
        existingCollection.addAll(newRelationships); // Add all at once
    }
}

*/