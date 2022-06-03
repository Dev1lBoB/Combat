using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AnimationController : MonoBehaviour
{
    private SkeletonAnimation skeleton;

    private void Awake()
    {
        skeleton = GetComponent<SkeletonAnimation>();
    }

    public IEnumerator PlayAnimationOnce(string newAnimationType, string defaultAnimationType)
    {
        bool animationEnd = false;

        skeleton.AnimationName = newAnimationType;
        skeleton.loop = false;
        skeleton.AnimationState.End += delegate
        {
            // After animation played once return to delault state 
            skeleton.AnimationName = defaultAnimationType;
            skeleton.loop = true;
            animationEnd = true;
        };

        while (animationEnd == false) // Wait untill the animation ends
            yield return null;
    }
}
