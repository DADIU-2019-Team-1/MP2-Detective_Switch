using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public class CSVReaderWriter // : MonoBehaviour
{
    // [Header("Tip: right-click and 'Copy Path' in project tab")]
    private string CSVReadPath = "Assets/Resources/CSV/AnimData.csv";
    private string CSVWritePath = "Assets/Resources/CSV/AnimData.csv";
    // [Tooltip("Specify the file name without writing .csv - This will be appended automatically")]
    private string CSVFileName = "AnimData.csv";

    // Variable holders from reading CSV files:
    private string[] labels;
    private List<string> clipNames;
    private List<int> frames;
    private List<Vector3> rootPositions, lFootPositions, rFootPositions, 
        lFootVelocities, rFootVelocities, rootVelocities, trajPositions, trajForwards;
    private List<Quaternion> rootQs, lFootQs, rFootQs;

#if UNITY_EDITOR
    void Awake()
    {
        // ReadCSV();
        // CalculateColoumns(8, 3, 1);
    }
#endif

    public void ReadCSV()
    {
        if (CSVReadPath == null)
        {
            Debug.LogError("CSV Reader/Writer Error: CSV read path empty!");
        }
        else
        {
            ReadCSV(CSVReadPath);
        }

    }

    public void ReadCSV(string path)
    {

        StreamReader strReader = new StreamReader(path);

        bool endOfFile = false;
        bool firstRun = true;

        while (!endOfFile)
        {
            string dataString = strReader.ReadLine();

            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            if (firstRun)
            {
                string[] tempLabelValues = dataString.Split(',');
                labels = tempLabelValues;

                /// Initialize thelists for quaternions, positions etc:
                InitializeLists();

                firstRun = false;
            }
            else
            {
                string[] tempDataValues = dataString.Split(',');
                string tempClipNameValue = "";
                float[] dataValues = new float[tempDataValues.Length];
                clipNames.Add(tempDataValues[0]);
                for (int i = 1; i < dataValues.Length; i++)
                    dataValues[i] = float.Parse(tempDataValues[i], CultureInfo.InvariantCulture.NumberFormat);

                // Populate the quaternion, position and timestamp arrays/lists:
                QuaternionCreator(dataValues);
                PositionCreator(dataValues);
                FloatIntCreator(dataValues);
            }

        }
        Debug.Log("CSV Reader/Writer: reading completed");
    }

    private void PositionCreator(float[] data)
    {
        for (int i = 0; i < labels.Length; i++)   // Vector starts from position 1
        {
            if (labels[i].Contains("RootT.x"))
                rootPositions.Add(      new Vector3(data[i], 0.0f, data[i + 1]));
            else if (labels[i].Contains("LFootT.x"))
                lFootPositions.Add(  new Vector3(data[i], 0.0f, data[i + 1]));
            else if (labels[i].Contains("LFootV.x"))
                lFootVelocities.Add(   new Vector3(data[i], 0.0f, data[i + 1]));
            else if (labels[i].Contains("RFootT.x"))
                rFootPositions.Add( new Vector3(data[i], 0.0f, data[i + 1]));
            else if (labels[i].Contains("RFootV.x"))
                rFootVelocities.Add(  new Vector3(data[i], 0.0f, data[i + 1]));
            else if (labels[i].Contains("TrajPos.x"))
                trajPositions.Add(      new Vector3(data[i], 0.0f, data[i + 1]));
            else if (labels[i].Contains("TrajForward.x"))
                trajForwards.Add(       new Vector3(data[i], 0.0f, data[i + 1]));
        }
    }

    private void QuaternionCreator(float[] data)
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].Contains("RootQ.x"))
                rootQs.Add(new Quaternion(data[i], data[i + 1], data[i + 2], data[i + 3]));
            else if (labels[i].Contains("LFootQ.x"))
                lFootQs.Add(new Quaternion(data[i], data[i + 1], data[i + 2], data[i + 3]));
            else if (labels[i].Contains("RFootQ.x"))
                rFootQs.Add(new Quaternion(data[i], data[i + 1], data[i + 2], data[i + 3]));
        }
    }

    private void FloatIntCreator(float[] data)
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].Contains("Frame"))
            {
                int tempInt = (int)data[i];
                frames.Add(tempInt);
            }
        }
    }

    public void WriteCSV(List<MMPose> poseData, List<TrajectoryPoint> trajectoryData)
    {
        // Error checking
        if (CSVWritePath == null || CSVFileName == null)
        {
            Debug.LogError("CSV Reader/Writer Error: CSV write path or file name empty!");
        }
        else if (poseData.Count == 0 || poseData == null)
        {
            Debug.LogError("CSV Reader/Writer Error: attempt to write CSV file using empty lists!");
        }
        else
        {
            Debug.Log("CSV Reader/Writer: writing lists to CSV file " + CSVFileName);

            using (var file = File.CreateText(CSVWritePath))
            {
	            labels = new string[24]
	            {
		            "ClipName", "Frame",
		            "RootT.x", "RootT.z",
                    "LFootT.x", "LFootT.z",
                    "LFootQ.x","LFootQ.y","LFootQ.z","LFootQ.w",
                    "LFootV.x", "LFootV.z",
                    "RFootT.x", "RFootT.z",
                    "RFootQ.x","RFootQ.y","RFootQ.z","RFootQ.w",
                    "RFootV.x", "RFootV.z",
		            "TrajPos.x", "TrajPos.z",
		            "TrajForward.x", "TrajForward.z"
	            };
                file.WriteLine(string.Join(",", labels));

                string spec;
                CultureInfo cul;

                spec = "G";
                cul = CultureInfo.CreateSpecificCulture("en-US");

                for (int i = 0; i < poseData.Count; i++)
                {
                    string[] tempLine = new string[24] {poseData[i].clipName, poseData[i].frame.ToString(),
                        poseData[i].rootPos.x.ToString(spec, cul),  poseData[i].rootPos.z.ToString(spec, cul),
                        poseData[i].lFootPos.x.ToString(spec, cul), poseData[i].lFootPos.z.ToString(spec, cul),
                        poseData[i].lFootQ.x.ToString(spec, cul),   poseData[i].lFootQ.y.ToString(spec, cul),
                        poseData[i].lFootQ.z.ToString(spec, cul),   poseData[i].lFootQ.w.ToString(spec, cul),
                        poseData[i].lFootVel.x.ToString(spec, cul), poseData[i].lFootVel.z.ToString(spec, cul),
                        poseData[i].rFootPos.x.ToString(spec, cul), poseData[i].rFootPos.z.ToString(spec, cul),
                        poseData[i].rFootQ.x.ToString(spec, cul),   poseData[i].rFootQ.y.ToString(spec, cul),
                        poseData[i].rFootQ.z.ToString(spec, cul),   poseData[i].rFootQ.w.ToString(spec, cul),
                        poseData[i].rFootVel.x.ToString(spec, cul),  poseData[i].rFootVel.z.ToString(spec, cul),
                        trajectoryData[i].position.x.ToString(spec, cul),   trajectoryData[i].position.z.ToString(spec, cul),
                        trajectoryData[i].forward.x.ToString(spec, cul),    trajectoryData[i].forward.z.ToString(spec, cul),};

                    file.WriteLine(string.Join(",", tempLine));
                }
            }
        }
    }

    private void InitializeLists()
    {
        clipNames = new List<string>();
        frames = new List<int>();
        rootPositions = new List<Vector3>();
        rootQs = new List<Quaternion>();
        lFootPositions = new List<Vector3>();
        lFootQs = new List<Quaternion>();
        lFootVelocities = new List<Vector3>();
        rFootPositions = new List<Vector3>();
        rFootQs = new List<Quaternion>();
        rFootVelocities = new List<Vector3>();
        trajPositions = new List<Vector3>();
        trajForwards = new List<Vector3>();
    }

    public string GetCSVReadPath()
    {
        return CSVReadPath;
    }

    public string GetCSVFileName()
    {
        return CSVFileName;
    }

    public string GetCSVWritePath()
    {
        return CSVWritePath;
    }

    public string[] GetLabels()
    {
        return labels;
    }

    public List<string> GetClipNames()
    {
        return clipNames;
    }

    public List<int> GetFrames()
    {
        return frames;
    }

    public List<Vector3> GetLeftFootPos()
    {
        return lFootPositions;
    }

    public List<Vector3> GetRightFootPos()
    {
        return rFootPositions;
    }

    public List<Vector3> GetRootPos()
    {
        return rootPositions;
    }

    public List<Quaternion> GetRootQ()
    {
        return rootQs;
    }
    public List<Quaternion> GetLeftFootQs()
    {
        return lFootQs;
    }
    public List<Quaternion> GetRightFootQs()
    {
        return rFootQs;
    }

    public List<Vector3> GetLeftFootVel()
    {
        return lFootVelocities;
    }

    public List<Vector3> GetRightFootVel()
    {
        return rFootVelocities;
    }

    public List<Vector3> GetRootVel()
    {
        return rootVelocities;
    }

    public List<Vector3> GetTrajectoryPos()
    {
        return trajPositions;
    }

    public List<Vector3> GetTrajectoryForwards()
    {
        return trajForwards;
    }

    public void SetWritePath(string path, string fileName)
    {

        if (path.Substring(path.Length - 1, 1) != "/")
        {
            if (fileName.Substring(fileName.Length - 4, 4) != ".csv")
            {
                CSVWritePath = path + "/" + fileName + ".csv";
            }
            else
            {
                CSVWritePath = path + "/" + fileName;
            }
        }
        else
        {
            if (fileName.Substring(fileName.Length - 4, 4) != ".csv")
            {
                CSVWritePath = path + fileName + ".csv";
            }
            else
            {
                CSVWritePath = path + fileName;
            }
        }

    }

    public void SetReadPath(string path, string fileName)
    {
        if (path.Substring(path.Length - 1, 1) != "/")
        {
            if (fileName.Substring(fileName.Length - 4, 4) != ".csv")
            {
                CSVReadPath = path + "/" + fileName + ".csv";
            } else
            {
                CSVReadPath = path + "/" + fileName;
            }
        }
        else
        {
            if (fileName.Substring(fileName.Length - 4, 4) != ".csv")
            {
                CSVReadPath = path + fileName + ".csv";
            }
            else
            {
                CSVReadPath = path + fileName;
            }
        }
    }

    public string GetWritePath()
    {
        return CSVWritePath;
    }

    public string GetReadPath()
    {
        return CSVReadPath;
    }
}
