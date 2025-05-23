package com.tacker.tracker.repository;

import com.tacker.tracker.models.Microservice;
import org.springframework.data.jpa.repository.JpaRepository;

public interface MicroserviceRepository extends JpaRepository<Microservice, Integer> {
}
