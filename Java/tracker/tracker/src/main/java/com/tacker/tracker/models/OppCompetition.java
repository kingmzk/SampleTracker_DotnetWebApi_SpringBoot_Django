package com.tacker.tracker.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "opp_competition")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class OppCompetition {  // Changed to PascalCase

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;  // Changed from Id to id (lowercase)

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "tracker_id")
    @JsonIgnore
    private Tracker tracker;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "competition_id")
    private Competition competition;

    public OppCompetition(Competition competition, Tracker tracker) {
        this.competition = competition;
        this.tracker = tracker;
        this.tracker.getOppCompetitions().add(this); // Add to parent collection
    }

}