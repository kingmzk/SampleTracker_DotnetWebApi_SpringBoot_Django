package com.tacker.tracker.repository;

import com.tacker.tracker.models.OppCompetition;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OppCompetitionRepository extends JpaRepository<OppCompetition, Integer> {
    void deleteAllByTrackerId(int trackerId);
}