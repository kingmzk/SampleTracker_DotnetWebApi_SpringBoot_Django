package com.tacker.tracker.repository;

import com.tacker.tracker.models.OppMicroservice;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OppMicroserviceRepository extends JpaRepository<OppMicroservice, Integer> {
    void deleteAllByTrackerId(int trackerId);
}
