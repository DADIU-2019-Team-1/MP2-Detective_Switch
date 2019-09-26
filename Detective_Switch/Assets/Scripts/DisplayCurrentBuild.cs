using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrentBuild : MonoBehaviour
{
    Text text;
    TextAsset asset;
    public string versionType = "Alpha";

    void Start()
    {
        text = GetComponent<Text>();
        asset = (TextAsset)Resources.Load("buildNumbers");
        string temp = asset.text.Substring(44);
        text.text += " " + temp;
        text.text += " - " + versionType;
    }
}
