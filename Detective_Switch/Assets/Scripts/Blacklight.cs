using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacklight : MonoBehaviour
{
    [Header("Make sure an object in the scene", order = 1)]
    [Header("has the tag \"Flashlight\"!!", order = 2)]
    public Vector2 initSize;
    Transform effector;
    Renderer rend;

    /// Default value of alphaclipping is 1.5

    void Start()
    {
        effector = GameObject.FindGameObjectWithTag("Flashlight").transform;
        rend = GetComponent<Renderer>();
        if (initSize == new Vector2(0,0))
        {
            initSize = rend.material.GetVector("_size");
        }
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(effector.position, effector.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                rend.material.SetVector("_blacklightpos", hit.point);
                rend.material.SetVector("_size", new Vector4(initSize.x, initSize.y, 0, 0));
            }
            else
            {
                rend.material.SetVector("_size", new Vector4(0, 0, 0, 0));
            }
        }
    }

}