package com.tacker.tracker.models;

import jakarta.persistence.*;
import lombok.*;

import java.util.List;

@Entity
@Table(name = "gen_ai_tool")  // Changed from GenAiTool to gen_ai_tool
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class GenAiTool {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;  // Changed from Id to id

    @Column(length = 500, nullable = false)
    private String value;

    @Column(nullable = true)
    private boolean disabled;


    @OneToMany(mappedBy = "genAiTool", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Tracker> trackers;  // Changed from trackerList to trackers


}