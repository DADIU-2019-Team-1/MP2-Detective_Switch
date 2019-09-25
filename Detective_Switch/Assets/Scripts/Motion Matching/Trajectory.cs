using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trajectory
{
    public string clipName;
    public int clipFrames;
    public int trajectoryId;
    public TrajectoryPoint[] points;

    public Trajectory (string _clipName, int _clipFrames, int _trajectoryId, TrajectoryPoint[] _points)
    {
        clipName = _clipName;
        clipFrames = _clipFrames;
        trajectoryId = _trajectoryId;
        points = _points;
    }
}
