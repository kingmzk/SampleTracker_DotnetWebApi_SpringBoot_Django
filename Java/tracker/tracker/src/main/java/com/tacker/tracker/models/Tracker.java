package com.tacker.tracker.models;

import jakarta.persistence.*;
import lombok.*;


import java.util.HashSet;
import java.util.Set;

@Entity
@Table(name = "tracker")
//@Data // Generates getters, setters, toString, equals, hashCode
@Getter
@Setter
@NoArgsConstructor // Generates no-args constructor required by JPA
@AllArgsConstructor // Generates all-args constructor
@ToString
public class Tracker {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "tracker_id", nullable = false)
    private int trackerId;

    @Column(name = "tracker_name", length = 500, nullable = false)
    private String trackerName;

    @Column(name = "client_name", length = 500, nullable = false)
    private String clientName;

    @Column(nullable = false)
    private double investment;

    @Column(name = "gen_ai_adoptation", nullable = false)
    private boolean genAiAdoptation;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "gen_ai_tool_id")
    private GenAiTool genAiTool;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "reason_for_no_gen_ai_adoptation_id")
    private ReasonForNoGenAiAdoptation reasonForNoGenAiAdoptation;

    @OneToMany(mappedBy = "tracker", cascade = CascadeType.ALL, orphanRemoval = true)
    @ToString.Exclude
    private Set<OppAccelerator> oppAccelerators = new HashSet<>();

    @OneToMany(mappedBy = "tracker", cascade = CascadeType.ALL, orphanRemoval = true)
    @ToString.Exclude
    private Set<OppMicroservice> oppMicroservices = new HashSet<>();

    @OneToMany(mappedBy = "tracker", cascade = CascadeType.ALL, orphanRemoval = true)
    @ToString.Exclude
    private Set<OppCompetition> oppCompetitions = new HashSet<>();

}