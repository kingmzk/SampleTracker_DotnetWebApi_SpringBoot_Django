package com.tacker.tracker.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class PatchTrackerDto {
    private String trackerId;
    private String trackerName;
    private String clientName;
    private Double investment;
    private Boolean genAiAdoptation;
    private SimpleDto genAiTool;
    private SimpleDto reasonForNoGenAiAdoptation;
    private List<SimpleDto> oppAccelerators;
    private List<SimpleDto> oppMicroservices;
    private List<SimpleDto> oppCompetitions;
}
