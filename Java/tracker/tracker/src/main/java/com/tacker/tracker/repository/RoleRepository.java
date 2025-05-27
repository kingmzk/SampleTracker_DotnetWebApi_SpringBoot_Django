package com.tacker.tracker.repository;

import com.tacker.tracker.models.Role;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface RoleRepository extends JpaRepository<Role, Long> {
    List<Role> findAllByIdIn(List<Long> ids);
    Optional<Role> findByName(String name);
}