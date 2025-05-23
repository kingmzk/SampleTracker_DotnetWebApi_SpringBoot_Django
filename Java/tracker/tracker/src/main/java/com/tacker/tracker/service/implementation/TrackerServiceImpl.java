package com.tacker.tracker.service.implementation;

import com.tacker.tracker.dto.InputTrackerDto;
import com.tacker.tracker.dto.PatchTrackerDto;
import com.tacker.tracker.dto.TrackerDto;
import com.tacker.tracker.dto.SimpleDto;
import com.tacker.tracker.models.*;
import com.tacker.tracker.repository.*;
import com.tacker.tracker.service.TrackerService;
import jakarta.persistence.EntityNotFoundException;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.function.BiFunction;
import java.util.function.Function;
import java.util.stream.Collectors;

@Service
@Transactional
public class TrackerServiceImpl implements TrackerService {

    private final TrackerRepository trackerRepository;
    private final GenAiToolRepository genAiToolRepository;
    private final ReasonForNoGenAiAdoptationRepository reasonRepository;
    private final AcceleratorRepository acceleratorRepository;
    private final MicroserviceRepository microserviceRepository;
    private final CompetitionRepository competitionRepository;

    public TrackerServiceImpl(
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

    @Override
    public List<TrackerDto> getAllTrackers() {
        return trackerRepository.findAll().stream()
                .map(this::convertToDto)
                .collect(Collectors.toList());
    }

    @Override
    public TrackerDto getTrackerById(int id) {
        Tracker tracker = trackerRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Tracker not found with ID: " + id));
        return convertToDto(tracker);
    }

//    @Override
//    public TrackerDto createTracker(TrackerDto dto) {
//        Tracker tracker = convertToEntity(dto, new Tracker());
//        return convertToDto(trackerRepository.save(tracker));
//    }

    @Override
    public TrackerDto createTracker(InputTrackerDto dto) {
        Tracker tracker = convertToEntity(dto, new Tracker());
        return convertToDto(trackerRepository.save(tracker));
    }


    @Override
    public TrackerDto updateTracker(int id, InputTrackerDto dto) {
        Tracker existing = trackerRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Tracker not found with ID: " + id));

        clearRelationships(existing);
        Tracker updated = convertToEntity(dto, existing);
        return convertToDto(trackerRepository.save(updated));
    }

    private void clearRelationships(Tracker tracker) {
        tracker.getOppAccelerators().clear();
        tracker.getOppMicroservices().clear();
        tracker.getOppCompetitions().clear();
    }

    private TrackerDto convertToDto(Tracker tracker) {
        TrackerDto dto = new TrackerDto();
        dto.setId(tracker.getId());
        dto.setTrackerId(tracker.getTrackerId());
        dto.setTrackerName(tracker.getTrackerName());
        dto.setClientName(tracker.getClientName());
        dto.setInvestment(tracker.getInvestment());
        dto.setGenAiAdoptation(tracker.isGenAiAdoptation());

        if (tracker.getGenAiTool() != null)
            dto.setGenAiTool(new SimpleDto(tracker.getGenAiTool().getId(), tracker.getGenAiTool().getValue()));

        if (tracker.getReasonForNoGenAiAdoptation() != null)
            dto.setReasonForNoGenAiAdoptation(new SimpleDto(tracker.getReasonForNoGenAiAdoptation().getId(),
                    tracker.getReasonForNoGenAiAdoptation().getValue()));

        dto.setOppAccelerators(convertOppRelationships(tracker.getOppAccelerators(),
                opp -> opp.getAccelerator().getId(), opp -> opp.getAccelerator().getValue()));

        dto.setOppMicroservices(convertOppRelationships(tracker.getOppMicroservices(),
                opp -> opp.getMicroservice().getId(), opp -> opp.getMicroservice().getValue()));

        dto.setOppCompetitions(convertOppRelationships(tracker.getOppCompetitions(),
                opp -> opp.getCompetition().getId(), opp -> opp.getCompetition().getValue()));

        return dto;
    }


    @Override
    public TrackerDto patchTracker(int id, PatchTrackerDto dto) {
        Tracker tracker = trackerRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Tracker not found with ID: " + id));

        if (dto.getTrackerId() != null) tracker.setTrackerId(Integer.parseInt(dto.getTrackerId()));
        if (dto.getTrackerName() != null) tracker.setTrackerName(dto.getTrackerName());
        if (dto.getClientName() != null) tracker.setClientName(dto.getClientName());
        if (dto.getInvestment() != null) tracker.setInvestment(dto.getInvestment());
        if (dto.getGenAiAdoptation() != null) tracker.setGenAiAdoptation(dto.getGenAiAdoptation());

        if (dto.getGenAiTool() != null) {
            genAiToolRepository.findById(dto.getGenAiTool().getId())
                    .ifPresentOrElse(tracker::setGenAiTool,
                            () -> { throw new EntityNotFoundException("GenAI Tool not found"); });
        }

        if (dto.getReasonForNoGenAiAdoptation() != null) {
            reasonRepository.findById(dto.getReasonForNoGenAiAdoptation().getId())
                    .ifPresentOrElse(tracker::setReasonForNoGenAiAdoptation,
                            () -> { throw new EntityNotFoundException("Reason not found"); });
        }

        if (dto.getOppAccelerators() != null) {
            tracker.getOppAccelerators().clear();
            addRelationships(dto.getOppAccelerators(), acceleratorRepository,
                    (entity, t) -> new OppAccelerator(entity, t), tracker.getOppAccelerators(), tracker);
        }

        if (dto.getOppMicroservices() != null) {
            tracker.getOppMicroservices().clear();
            addRelationships(dto.getOppMicroservices(), microserviceRepository,
                    (entity, t) -> new OppMicroservice(entity, t), tracker.getOppMicroservices(), tracker);
        }

        if (dto.getOppCompetitions() != null) {
            tracker.getOppCompetitions().clear();
            addRelationships(dto.getOppCompetitions(), competitionRepository,
                    (entity, t) -> new OppCompetition(entity, t), tracker.getOppCompetitions(), tracker);
        }

        return convertToDto(trackerRepository.save(tracker));
    }


    private <T> List<SimpleDto> convertOppRelationships(
            Collection<T> relationships,
            Function<T, Integer> idExtractor,
            Function<T, String> valueExtractor
    ) {
        return relationships.stream()
                .map(rel -> {
                    try {
                        return new SimpleDto(idExtractor.apply(rel), valueExtractor.apply(rel));
                    } catch (Exception e) {
                        return null;
                    }
                })
                .filter(Objects::nonNull)
                .collect(Collectors.toList());
    }

    private Tracker convertToEntity(InputTrackerDto dto, Tracker tracker) {
        tracker.setTrackerId(dto.getTrackerId());
        tracker.setTrackerName(dto.getTrackerName());
        tracker.setClientName(dto.getClientName());
        tracker.setInvestment(dto.getInvestment());
        tracker.setGenAiAdoptation(Boolean.TRUE.equals(dto.getGenAiAdoptation()));

        if (dto.getGenAiTool() != null) {
            genAiToolRepository.findById(dto.getGenAiTool())
                    .ifPresentOrElse(tracker::setGenAiTool,
                            () -> { throw new EntityNotFoundException("GenAI Tool not found"); });
        }

        if (dto.getReasonForNoGenAiAdoptation() != null) {
            reasonRepository.findById(dto.getReasonForNoGenAiAdoptation())
                    .ifPresentOrElse(tracker::setReasonForNoGenAiAdoptation,
                            () -> { throw new EntityNotFoundException("Reason not found"); });
        }

        tracker.getOppAccelerators().clear();
        tracker.getOppMicroservices().clear();
        tracker.getOppCompetitions().clear();

        addIdRelationships(dto.getOppAccelerators(), acceleratorRepository,
                (entity, t) -> new OppAccelerator(entity, t), tracker.getOppAccelerators(), tracker);

        addIdRelationships(dto.getOppMicroservices(), microserviceRepository,
                (entity, t) -> new OppMicroservice(entity, t), tracker.getOppMicroservices(), tracker);

        addIdRelationships(dto.getOppCompetitions(), competitionRepository,
                (entity, t) -> new OppCompetition(entity, t), tracker.getOppCompetitions(), tracker);

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


    private <T, O> void addIdRelationships(
            List<Integer> ids,
            JpaRepository<T, Integer> repo,
            BiFunction<T, Tracker, O> creator,
            Collection<O> collection,
            Tracker tracker
    ) {
        if (ids == null) return;
        ids.stream()
                .map(repo::findById)
                .filter(Optional::isPresent)
                .map(Optional::get)
                .map(entity -> creator.apply(entity, tracker))
                .forEach(collection::add);
    }

}



