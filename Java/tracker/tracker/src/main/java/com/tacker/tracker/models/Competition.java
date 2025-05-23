package com.tacker.tracker.models;

import jakarta.persistence.*;
import lombok.*;

import java.util.Set;

@Entity
@Table(name = "competition")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Competition {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int Id;

    private String value;

    @OneToMany(mappedBy = "competition")
    private Set<OppCompetition> oppCompetitionSet;

}
