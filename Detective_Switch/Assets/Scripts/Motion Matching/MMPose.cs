using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MMPose
{
    public string clipName;
    public int frame, id;
    public Vector3 rootPos, lFootPos, rFootPos, rootVel, lFootVel, rFootVel;
    public Quaternion rootQ, lFootQ, rFootQ; 

    public MMPose(string _clipName, int _frame, Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos,
        Vector3 _rootVel, Vector3 _lFootVel, Vector3 _rFootVel, Quaternion _rootQ, Quaternion _lFootQ, Quaternion _rFootQ)
    {
        clipName = _clipName;
        frame = _frame;
        rootPos = _rootPos;
        rootVel = _rootVel;
        lFootPos = _lFootPos;
        rFootPos = _rFootPos;
        lFootVel = _lFootVel;
        rFootVel = _rFootVel;
        rootQ = _rootQ;
        lFootQ = _lFootQ;
        rFootQ = _rFootQ;
    }
    public MMPose(string _clipName, int _frame, Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos,
        Vector3 _lFootVel, Vector3 _rFootVel, Quaternion _lFootQ, Quaternion _rFootQ)
    {
	    clipName = _clipName;
	    frame = _frame;
	    rootPos = _rootPos;
	    lFootPos = _lFootPos;
	    rFootPos = _rFootPos;
	    lFootVel = _lFootVel;
	    rFootVel = _rFootVel;
        lFootQ = _lFootQ;
        rFootQ = _rFootQ;
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
    public MMPose(Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos, Quaternion _rootQ, Quaternion _lFootQ, Quaternion _rFootQ, Vector3 _lFootVel, Vector3 _rFootVel)
    {
        rootPos = _rootPos;
        rootQ = _rootQ;
        lFootPos = _lFootPos;
        rFootPos = _rFootPos;
        lFootQ = _lFootQ;
        rFootQ = _rFootQ;
        lFootVel = _lFootVel;
        rFootVel = _rFootVel;
    }
    public MMPose(Vector3 _rootPos, Vector3 _lFootPos, Vector3 _rFootPos, Vector3 _rootVel, Vector3 _lFootVel, Vector3 _rFootVel, Quaternion _rootQ)
    {
        rootPos = _rootPos;
        lFootPos = _lFootPos;
        rFootPos = _rFootPos;
        rootVel = _rootVel;
        lFootVel = _lFootVel;
        rFootVel = _rFootVel;
        rootQ = _rootQ;
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
        distance += Vector3.Distance(lFootPos, poseToCompare.lFootPos);
        distance += Vector3.Distance(rFootPos, poseToCompare.rFootPos);
        distance += Quaternion.Angle(lFootQ, poseToCompare.lFootQ) / 100;
        distance += Quaternion.Angle(rFootQ, poseToCompare.rFootQ) / 100;
        //distance += Vector3.Distance(lFootVel, poseToCompare.lFootVel);
        //distance += Vector3.Distance(rFootVel, poseToCompare.rFootVel);
        return distance;
    }

    public MMPose ConvertToOtherPoseSpace(MMPose otherPose)
    {
        Matrix4x4 m = Matrix4x4.identity;
        Quaternion q = Quaternion.identity;
        q = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, q.eulerAngles.z);
        m = Matrix4x4.TRS(otherPose.rootPos, q, Vector3.one);
        MMPose poseInOtherPoseSpace = new MMPose(
            rootPos + otherPose.rootPos, lFootPos + otherPose.rootPos, rFootPos + otherPose.rootPos, 
            m.rotation * rootQ, m.rotation *rFootQ, m.rotation * lFootQ, lFootVel, rFootVel);
        return poseInOtherPoseSpace;
    }

    public string PrintJointsAndVelocities()
    {
        return "RootPos: (x=" + rootPos.x + ", z=" + rootPos.z + "), RootVel: (x=" + rootVel.x + ", y=" + rootVel.y + ", z=" + rootVel.z + ").\n" +
               "lFootPos: (x=" + lFootPos.x + ", z=" + lFootPos.z + "), lFootVel: (x=" + lFootVel.x + ", y=" + lFootVel.y + ", z=" + lFootVel.z + ").\n" +
               "rFootPos: (x=" + rFootPos.x + ", z=" + rFootPos.z + "), rFootVel: (x=" + rFootVel.x + ", y=" + rFootVel.y + ", z=" + rFootVel.z + ").\n";
    }
}
