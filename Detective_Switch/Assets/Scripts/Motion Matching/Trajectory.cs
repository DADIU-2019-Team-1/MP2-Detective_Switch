[System.Serializable]
public class Trajectory
{
    public string clipName;
    public int clipFrames;
    public int trajectoryId;
    public TrajectoryPoint[] points;

    public Trajectory (TrajectoryPoint[] _points)
    {
        clipName = "";
        clipFrames = -1;
        trajectoryId = -1;
        points = _points;
    }

    public Trajectory (string _clipName, int _clipFrames, int _trajectoryId, TrajectoryPoint[] _points)
    {
        clipName = _clipName;
        clipFrames = _clipFrames;
        trajectoryId = _trajectoryId;
        points = _points;
    }

    public TrajectoryPoint[] GetTrajectoryPoints()
    {
        return points;
    }

    public int GetTrajectoryId()
    {
        return trajectoryId;
    }
}
