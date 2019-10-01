using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MMPose
{
    public string clipName;
    public int frame, id;
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
    public MMPose(string _clipName, int _frame, Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos,
	    Vector3 _rootVel, Vector3 _lFootVel, Vector3 _rFootVel)
    {
	    clipName = _clipName;
	    frame = _frame;
	    rootPos = _rootPos;
	    lFootPos = _lFootPos;
	    rFootPos = _rFootPos;
	    rootVel = _rootVel;
	    lFootVel = _lFootVel;
	    rFootVel = _rFootVel;
    }
    public MMPose(Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos, Vector3 _rootVel, Vector3 _lFootVel, Vector3 _rFootVel)
    {
        rootPos = _rootPos;
        lFootPos = _lFootPos;
        rFootPos = _rFootPos;
        rootVel = _rootVel;
        lFootVel = _lFootVel;
        rFootVel = _rFootVel;
    }

    public string GetClipName()
    {
        return clipName;
    }

    public int GetFrame()
    {
        return frame;
    }
    public int GetPoseId(int _id)
    {
        return id;
    }

    public float ComparePose(MMPose poseToCompare)
    {
        float distance = 0;
        distance += Vector3.Distance(rootPos, poseToCompare.rootPos);
        distance += Vector3.Distance(lFootPos, poseToCompare.lFootPos);
        distance += Vector3.Distance(rFootPos, poseToCompare.rFootPos);
        distance += Vector3.Distance(rootVel, poseToCompare.rootVel);
        distance += Vector3.Distance(lFootVel, poseToCompare.lFootVel);
        distance += Vector3.Distance(rFootVel, poseToCompare.rFootVel);
        return distance;
    }
}
