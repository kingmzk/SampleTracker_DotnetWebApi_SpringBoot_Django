package com.tacker.tracker.service.implementation;

import com.tacker.tracker.dto.RegisterRequest;
import com.tacker.tracker.models.Role;
import com.tacker.tracker.models.User;
import com.tacker.tracker.repository.RoleRepository;
import com.tacker.tracker.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.Collection;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class UserService {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final RoleRepository roleRepository;

    public User registerUser(RegisterRequest request) {
        if (userRepository.existsByUsername(request.getUsername())) {
            throw new RuntimeException("Username already exists");
        }

        // Fetch roles from database
        Set<Role> roles = new HashSet<>();
        if (request.getRoleIds() != null && !request.getRoleIds().isEmpty()) {
            roles = roleRepository.findAllByIdIn(request.getRoleIds())
                    .stream()
                    .collect(Collectors.toSet());

            // Optional: Validate all requested roles exist
            if (roles.size() != request.getRoleIds().size()) {
                throw new RuntimeException("One or more invalid role IDs");
            }
        } else {
            // Assign default role if none specified
            Role defaultRole = roleRepository.findByName("ROLE_USER")
                    .orElseThrow(() -> new RuntimeException("Default role not found"));
            roles.add(defaultRole);
        }

        User newUser = new User();
        newUser.setUsername(request.getUsername());
        newUser.setPassword(passwordEncoder.encode(request.getPassword()));
        newUser.setRoles(roles);

        return userRepository.save(newUser);
    }

}