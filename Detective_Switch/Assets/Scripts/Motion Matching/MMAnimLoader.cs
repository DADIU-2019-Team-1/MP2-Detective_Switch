using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MMAnimLoader : MonoBehaviour
{
    [Header("Please place this script on the animated player/character object")]
    GameObject player;
    Animator thisAnimator;
    AnimatorOverrideController animController;
    public List<AnimationClip> animationClips;

    void Start()
    {
        player = gameObject;

        if (player.GetComponent<Animator>() != null)
        {
            thisAnimator = player.GetComponent<Animator>();
            animController = new AnimatorOverrideController(thisAnimator.runtimeAnimatorController);
        }

        // Debug.Log(animationClips[1].name);
        // thisAnimator.Play(animationClips[1].name);

    }

    void Update()
    {
        
    }

    public List<AnimationClip> GetAnimationClips()
    {
        return animationClips;
    }

    public GameObject GetPlayerObject()
    {
        return player;
    }

    /*
    private void LateUpdate()
    {
        for (int i = 0; 0 < animationClips.Count; i++)
        {

            thisAnimator.Play(animationClips[i].name);
        }

    } */

}
