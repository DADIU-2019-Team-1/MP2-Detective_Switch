using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    public GameObject invBtn;
    // Start is called before the first frame update
    void Start()
    {
        // invBtn = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryButton(Button button) {
            Debug.Log(button.name);
            Transform invChild = button.transform.GetChild(0);
            if(!invChild.gameObject.activeInHierarchy) {
                invChild.gameObject.SetActive(true);
            }
            else {
                invChild.gameObject.SetActive(false);
            }


    }
}
