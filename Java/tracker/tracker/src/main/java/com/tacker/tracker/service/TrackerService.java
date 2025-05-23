package com.tacker.tracker.service;

import com.tacker.tracker.dto.InputTrackerDto;
import com.tacker.tracker.dto.PatchTrackerDto;
import com.tacker.tracker.dto.TrackerDto;
import com.tacker.tracker.models.Tracker;

import java.util.List;

public interface TrackerService {
    List<TrackerDto> getAllTrackers();
    TrackerDto getTrackerById(int id);

    TrackerDto createTracker(InputTrackerDto dto);
    TrackerDto updateTracker(int id, InputTrackerDto dto);

    TrackerDto patchTracker(int id, PatchTrackerDto dto);

}