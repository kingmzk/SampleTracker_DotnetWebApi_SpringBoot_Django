package com.tacker.tracker.models;

import jakarta.persistence.*;
import lombok.*;

import java.util.List;



import jakarta.persistence.*;
import lombok.Data;

import java.util.List;

@Entity
@Table(name = "reason_for_no_gen_ai_adoptation")  // Changed table name to snake_case
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class ReasonForNoGenAiAdoptation {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;  // Changed from Id to id

    @Column(length = 500, nullable = false)
    private String value;

    @Column(nullable = true)
    private boolean disabled;

    @OneToMany(mappedBy = "reasonForNoGenAiAdoptation", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Tracker> trackers;  // Changed from trackerList to trackers

}
