package com.tacker.tracker.dto;
import com.tacker.tracker.dto.SimpleDto;
import lombok.*;

import java.util.List;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class TrackerDto {
    private int id;
    private int trackerId;
    private String trackerName;
    private String clientName;
    private double investment;
    private boolean genAiAdoptation;
    private SimpleDto genAiTool;
    private SimpleDto reasonForNoGenAiAdoptation;
    private List<SimpleDto> oppAccelerators;
    private List<SimpleDto> oppMicroservices;
    private List<SimpleDto> oppCompetitions;
}
