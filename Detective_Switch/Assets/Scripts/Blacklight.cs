using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacklight : MonoBehaviour
{

    public Transform effector;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(effector.position, effector.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject == gameObject)
            {
                rend.sharedMaterial.SetVector("_blacklightpos", hit.point);
                Debug.Log("Hit the plane");
            }
        }
        else
        {
            Debug.Log("Didn't hit plane");
            rend.sharedMaterial.SetVector("_blacklightpos", new Vector3(0, 0, 0));
        }
    }

}