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
    private List<Vector3> rootPositions, leftFootPositions, rightFootPositions, 
        leftFootVelocity, rightFootVelocity, rootVelocity, trajPositions, trajForwards;
    private List<Quaternion> rootQuaternions;

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

                /// Populate the quaternion, position and timestamp arrays/lists:
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
            if (labels[i].Contains("FootLeftT.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                // Debug.Log(data[i] + " " + data[i + 1] + " " + data[i + 2]);  // For debugging (very performance heavy)
                leftFootPositions.Add(tempPosition);
            }
            else if (labels[i].Contains("FootRightT.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                rightFootPositions.Add(tempPosition);
            }
            else if (labels[i].Contains("RootT.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                rootPositions.Add(tempPosition);
            }
            else if (labels[i].Contains("RootV.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                rootVelocity.Add(tempPosition);
            }
            else if (labels[i].Contains("FootLeftV.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                leftFootVelocity.Add(tempPosition);
            }
            else if (labels[i].Contains("FootRightV.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                rightFootVelocity.Add(tempPosition);
            }
            else if (labels[i].Contains("TrajPos.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                trajPositions.Add(tempPosition);
            }
            else if (labels[i].Contains("TrajForward.x"))
            {
                Vector3 tempPosition = new Vector3(data[i], data[i + 1], data[i + 2]);
                trajForwards.Add(tempPosition);
            }
        }
    }

    private void QuaternionCreator(float[] data)
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].Contains("RootQ.x"))
            {
                Quaternion tempQuaternion = new Quaternion(data[i], data[i + 1], data[i + 2], data[i + 3]);
                rootQuaternions.Add(tempQuaternion);
            }
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
                labels = new string[30] {"ClipName", "Frame", "RootT.x","RootT.y","RootT.z","FootLeftT.x","FootLeftT.y","FootLeftT.z",
                "FootRightT.x","FootRightT.y","FootRightT.z","FootLeftV.x","FootLeftV.y", "FootLeftV.z","FootRightV.x","FootRightV.y",
                "FootRightV.z","RootV.x","RootV.x","RootV.x","RootQ.x","RootQ.y","RootQ.z","RootQ.w",
                    "TrajPos.x","TrajPos.y","TrajPos.z","TrajForward.x","TrajForward.y","TrajForward.z"};

                file.WriteLine(string.Join(",", labels));

                string spec;
                CultureInfo cul;

                spec = "G";
                cul = CultureInfo.CreateSpecificCulture("en-US");

                for (int i = 0; i < poseData.Count; i++)
                {
                    string[] tempLine = new string[30] {poseData[i].clipName, poseData[i].frame.ToString(),
                        poseData[i].rootPos.x.ToString(spec, cul),  poseData[i].rootPos.y.ToString(spec, cul),  poseData[i].rootPos.z.ToString(spec, cul),
                        poseData[i].lFootPos.x.ToString(spec, cul), poseData[i].lFootPos.y.ToString(spec, cul), poseData[i].lFootPos.z.ToString(spec, cul),
                        poseData[i].rFootPos.x.ToString(spec, cul), poseData[i].rFootPos.y.ToString(spec, cul), poseData[i].rFootPos.z.ToString(spec, cul),
                        poseData[i].rootVel.x.ToString(spec, cul),  poseData[i].rootVel.y.ToString(spec, cul),  poseData[i].rootVel.z.ToString(spec, cul),
                        poseData[i].lFootVel.x.ToString(spec, cul), poseData[i].lFootVel.y.ToString(spec, cul), poseData[i].lFootVel.z.ToString(spec, cul),
                        poseData[i].rFootVel.x.ToString(spec, cul), poseData[i].rFootVel.y.ToString(spec, cul), poseData[i].rFootVel.z.ToString(spec, cul),
                        poseData[i].rootQ.x.ToString(spec, cul),    poseData[i].rootQ.y.ToString(spec, cul),   poseData[i].rootQ.z.ToString(spec, cul), poseData[i].rootQ.w.ToString(spec, cul),
                        trajectoryData[i].position.x.ToString(spec, cul), trajectoryData[i].position.y.ToString(spec, cul),trajectoryData[i].position.z.ToString(spec, cul),
                        trajectoryData[i].forward.x.ToString(spec, cul), trajectoryData[i].forward.y.ToString(spec, cul),trajectoryData[i].forward.z.ToString(spec, cul),};

                    file.WriteLine(string.Join(",", tempLine));
                }
            }
        }
    }

    public void WriteCSV(List<string> _clipName, List<int> _frame, List<Vector3> _rootPos, List<Quaternion> _rootRot, List<Vector3> _footLeft, List<Vector3> _footRight, List<Vector3> _footLeftVel, List<Vector3> _footRightVel, List<Vector3> _rootVel)
    {
        if (CSVWritePath == null || CSVFileName == null)
        {
            Debug.LogError("CSV Reader/Writer Error: CSV write path or file name empty!");
        }
        else if (_frame.Count <= 0 || _frame == null)
        {
            Debug.LogError("CSV Reader/Writer Error: attempt to write CSV file using empty lists!");
        }
        else
        {
            // CSVFileName = CSVFileName + ".csv";
            // CSVWritePath = CSVWritePath + "/" + CSVFileName;
            Debug.Log("CSV Reader/Writer: writing lists to CSV file " + CSVFileName);

            using (var file = File.CreateText(CSVWritePath))
            {
                labels = new string[24] {"ClipName", "Frame", "RootT.x","RootT.y","RootT.z","RootQ.x","RootQ.y","RootQ.z","RootQ.w",
                "FootLeftT.x","FootLeftT.y","FootLeftT.z", "FootRightT.x","FootRightT.y","FootRightT.z","FootLeftV.x","FootLeftV.y",
                "FootLeftV.z","FootRightV.x","FootRightV.y","FootRightV.z","rootV.x","rootV.x","rootV.x"};

                file.WriteLine(string.Join(",", labels));

                string spec;
                CultureInfo cul;

                spec = "G";
                cul = CultureInfo.CreateSpecificCulture("en-US");

                for (int i = 0; i < _frame.Count; i++)
                {
                    string[] tempLine = new string[24] {_clipName[i], _frame[i].ToString(), _rootPos[i].x.ToString(spec, cul), _rootPos[i].y.ToString(spec, cul), _rootPos[i].z.ToString(spec, cul), _rootRot[i].x.ToString(spec, cul),
                    _rootRot[i].y.ToString(spec, cul), _rootRot[i].z.ToString(spec, cul), _rootRot[i].w.ToString(spec, cul), _footLeft[i].x.ToString(spec, cul), _footLeft[i].y.ToString(spec, cul), _footLeft[i].z.ToString(spec, cul),
                    _footRight[i].x.ToString(spec, cul), _footRight[i].y.ToString(spec, cul), _footRight[i].z.ToString(spec, cul), _footLeftVel[i].x.ToString(spec, cul), _footLeftVel[i].y.ToString(spec, cul), _footLeftVel[i].z.ToString(spec, cul),
                    _footRightVel[i].x.ToString(spec, cul), _footRightVel[i].y.ToString(spec, cul), _footRightVel[i].z.ToString(spec, cul), _rootVel[i].x.ToString(spec, cul), _rootVel[i].y.ToString(spec, cul), _rootVel[i].z.ToString(spec, cul)};

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
        leftFootPositions = new List<Vector3>();
        rightFootPositions = new List<Vector3>();
        leftFootVelocity = new List<Vector3>();
        rightFootVelocity = new List<Vector3>();
        rootVelocity = new List<Vector3>();
        rootQuaternions = new List<Quaternion>();
        trajPositions = new List<Vector3>();
        trajForwards = new List<Vector3>();

    }


    private void CalculateColoumns(int _amountColumns, int _amountVectors, int _amountQuaternions)
    {
        int tempColAmount;
        tempColAmount = (_amountColumns + (_amountVectors * 3) + (_amountQuaternions * 4)) - (_amountQuaternions + _amountVectors);
        Debug.Log(tempColAmount);
    }

    public void IndexHelper(int index)
    {
        if (index > -1 && index < labels.Length)
        {
            Debug.Log("CSV index " + index + " is the " + labels[index]);
        }
        else
        {
            Debug.LogError("CSV Reader/Writer Error: index helper out of bounds!");
        }
    }

    public void IndexHelper(string all)
    {
        if (all == "all")
        {
            for (int i = 0; 0 < labels.Length; i++)
            {
                Debug.Log("CSV index " + i + " is the " + labels[i]);
            }

        }
        else
        {
            Debug.Log("CSV Reader/Writer Notice: Index helper can only take specific index numbers or 'all' if you want to see all joints");
        }
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
        return leftFootPositions;
    }

    public List<Vector3> GetRightFootPos()
    {
        return rightFootPositions;
    }

    public List<Vector3> GetRootPos()
    {
        return rootPositions;
    }

    public List<Quaternion> GetRootQ()
    {
        return rootQuaternions;
    }

    public List<Vector3> GetLeftFootVel()
    {
        return leftFootVelocity;
    }

    public List<Vector3> GetRightFootVel()
    {
        return rightFootVelocity;
    }

    public List<Vector3> GetRootVel()
    {
        return rootVelocity;
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
