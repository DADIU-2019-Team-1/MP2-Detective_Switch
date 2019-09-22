using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalculator : MonoBehaviour
{
    /* The purpose of this script is to calculate the
     * desired trajectory of the player, which is then fed into
     * the MM script to match the current trajectory to the desired.
     * The desired trajectory is calculated based on the character movement,
     * so a trajectory test script will be used for testing purposes.
     */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /// Find current player direction from movement (depends on input method)
        Vector3 mov = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Quaternion playerDirection = Quaternion.LookRotation(mov, Vector3.up);
    }
}
