package com.tacker.tracker.repository;

import com.tacker.tracker.models.Tracker;
import org.springframework.data.jpa.repository.JpaRepository;

public interface TrackerRepository extends JpaRepository<Tracker, Integer> {
}