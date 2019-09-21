using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMPreProcessing : MonoBehaviour
{
    /* The purpose of this script is to process and store the
     * pose and trajectory in a feature list, which must cached
     * such that it can simply be read when the game starts.
     * However, initially we will simply test this by preprocessing
     * on start (or awake).
     *
     * Pose contains relevant joints (feet, neck), relevant velocities,
     * and other information regarding the joints in the rig.
     * Trajectory contains relevant information about the current trajectory
     * when playing a specific animation. It contains the time steps that is
     * being processed, and the amount of time to look in advance.
     */


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
