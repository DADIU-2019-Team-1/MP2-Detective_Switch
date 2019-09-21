using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacklight : MonoBehaviour
{
    public Vector2 initSize;
    public Transform effector;
    Renderer rend;

    /* Static setting of AlphaClipThreshold in the appropriate shader is required,
     * depending on size of circle. For 0.5, a threshold of 1.5 is suitable
     * There might be a correlation, like 1 from size, but this is yet untested.
     */

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (initSize == new Vector2(0,0))
        {
            initSize = rend.sharedMaterial.GetVector("_size");
        }
        Debug.Log(initSize);
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(effector.position, effector.forward);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject == gameObject)
            {
                rend.sharedMaterial.SetVector("_blacklightpos", hit.point);
                rend.sharedMaterial.SetVector("_size", new Vector4(initSize.x, initSize.y, 0, 0));
                Debug.Log("Hit the plane");
            }
            else
            {
                Debug.Log("Didn't hit plane");
                rend.sharedMaterial.SetVector("_size", new Vector4(0, 0, 0, 0));
            }
        }
    }

}