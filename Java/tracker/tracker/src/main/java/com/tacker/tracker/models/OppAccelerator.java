package com.tacker.tracker.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "opp_accelerator")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class OppAccelerator {  // Changed from opp_accelerator to OppAccelerator

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;  // Changed from Id to id

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "tracker_id")
    private Tracker tracker;


    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "accelerator_id")
    @JsonIgnore
    private Accelerator accelerator;

    public OppAccelerator(Accelerator accelerator, Tracker tracker) {
        this.accelerator = accelerator;
        this.tracker = tracker;
        this.tracker.getOppAccelerators().add(this); // Add to parent collection
    }

}