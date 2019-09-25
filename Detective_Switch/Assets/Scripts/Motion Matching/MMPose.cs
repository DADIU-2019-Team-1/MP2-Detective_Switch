using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MMPose
{
    public string clipName;
    public int frame;
    public Vector3 rootPos, lFootPos, rFootPos, rootVel, lFootVel, rFootVel;
    public Quaternion rootQ; // Might be redundant

    public MMPose(string _clipName, int _frame, Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos,
        Vector3 _rootVel, Vector3 _lFootVel, Vector3 _rFootVel, Quaternion _rootQ)
    {
        clipName = _clipName;
        frame = _frame;
        rootPos = _rootPos;
        lFootPos = _lFootPos;
        rFootPos = _rFootPos;
        rootVel = _rootVel;
        lFootVel = _lFootVel;
        rFootVel = _rFootVel;
        rootQ = _rootQ;
    }
}
