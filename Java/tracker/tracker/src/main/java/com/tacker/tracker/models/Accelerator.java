package com.tacker.tracker.models;

import jakarta.persistence.*;
import lombok.*;

import java.util.Set;

@Entity
@Table(name = "accelerator")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Accelerator {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;  // Changed from Id to id

    private String value;

    @OneToMany(mappedBy = "accelerator", cascade = CascadeType.ALL, orphanRemoval = true)
    private Set<OppAccelerator> oppAccelerators;  // Changed from oppAcceleratorSet and class name

}