using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrajectoryPoint
{
    // Base class for 
    public Vector3 position, forward;

    // Empty constructor if there are no values
    public TrajectoryPoint()
    {
        position = Vector3.zero;
        forward = Vector3.zero;
    }

    // If there are values, pass them into the constructor
    public TrajectoryPoint(Vector3 _position, Vector3 _forward)
    {
        position = _position;
        forward = _forward;
    }

    public string Print()
    {
        return position.x + "," + position.y + "," + position.z + "," + forward.x + "," + forward.y + "," + forward.z;
    }
}
