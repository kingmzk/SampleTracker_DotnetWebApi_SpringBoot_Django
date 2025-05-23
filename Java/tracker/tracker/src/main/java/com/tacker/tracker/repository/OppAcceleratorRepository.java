package com.tacker.tracker.repository;

import com.tacker.tracker.models.OppAccelerator;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OppAcceleratorRepository extends JpaRepository<OppAccelerator, Integer> {
    void deleteAllByTrackerId(int trackerId);
}