using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM : MonoBehaviour
{
    /* Main script for handling Motion Matching.
     * Load the feature list from the preprocesser,
     * and the desired trajectory from the TrajectoryCalculator,
     * and applies motion matching to the character.
     */
    private MMPreProcessing preprocces;
    private TrajectoryTest trajectory;
    private TrajectoryCalculator trajectoryCalc;
    public AnimationClip currentClip;
    public int currentAnimTime;
    public List<Vector3> jointPosList;


    void Start()
    {
        /// This is where the data would be loaded from a file, but for debugging
        /// purposes we simply run the preprocesser script with an input.
        /// Preprocesser does not derive from monobehaviour, and as such does not need
        /// an object reference.
        preprocces = new MMPreProcessing(currentClip, currentAnimTime, jointPosList);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    void ComputeCost()
    {

    }

    void NewClipSelector()
    {

    }

}
