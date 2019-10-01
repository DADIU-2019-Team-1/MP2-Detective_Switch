using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public GameObject item;
    public int id;
    public string text;
    public bool empty;
    // If we want to have panels when we change sprite, activate this and start.
    // public Transform slotIconGO;
    public Sprite icon;
    public Sprite emptyIcon;

    public Sprite greyedOutImage;

/*     public void Start() {
        slotIconGO = transform.GetChild(0);
    } */
    public void UpdateSlot() {
        this.GetComponent<Image>().sprite = icon;
    }

    void Start() {
        emptyIcon = GetComponent<Image>().sprite;
    }
}

