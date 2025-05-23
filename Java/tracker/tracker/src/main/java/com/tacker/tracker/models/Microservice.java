package com.tacker.tracker.models;

import jakarta.persistence.*;
import lombok.*;

import java.util.Set;

@Entity
@Table(name = "microservice")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Microservice {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int Id;

    private String value;

    @OneToMany(mappedBy = "microservice")
    private Set<OppMicroservice> oppMicroserviceSet;

}
