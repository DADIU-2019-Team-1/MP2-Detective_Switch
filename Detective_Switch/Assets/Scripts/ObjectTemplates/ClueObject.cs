using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New clue", menuName = "Items/Clue")]
public class ClueObject : ScriptableObject
{

    public string clueName;
    public string clueText;
    public Sprite clueImage;
    public int clueID;
    

}

[CreateAssetMenu(fileName = "New Key item", menuName = "Items/Keyitem")]
public class KeyItem : ScriptableObject
{
    public string keyItemName;
    public string keyItemText;
    public Sprite keyItemImage;
    public int keyItemID;
}


