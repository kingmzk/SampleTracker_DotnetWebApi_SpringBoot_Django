package com.tacker.tracker.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class InputTrackerDto {
    private Integer id;
    private String trackerId;
    private String trackerName;
    private String clientName;
    private Double investment;
    private Boolean genAiAdoptation;
    private Integer genAiTool; // just the ID
    private Integer reasonForNoGenAiAdoptation; // just the ID
    private List<Integer> oppAccelerators; // list of IDs
    private List<Integer> oppMicroservices;
    private List<Integer> oppCompetitions;
}
