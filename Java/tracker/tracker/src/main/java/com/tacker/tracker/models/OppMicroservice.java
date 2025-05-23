package com.tacker.tracker.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "opp_microservice")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class OppMicroservice {  // Changed to PascalCase

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;  // Changed from Id to id (lowercase)

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "tracker_id")
    @JsonIgnore
    private Tracker tracker;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "microservice_id")
    private Microservice microservice;

    public OppMicroservice(Microservice microservice, Tracker tracker) {
        this.microservice = microservice;
        this.tracker = tracker;
        this.tracker.getOppMicroservices().add(this); // Add to parent collection
    }

}