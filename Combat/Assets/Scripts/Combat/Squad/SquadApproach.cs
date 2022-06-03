using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Spine.Unity;

public class SquadApproach : MonoBehaviour
{
    [SerializeField]
    private Vector2 finalPosition;
    [SerializeField]
    private float   duration;

    private List<SkeletonAnimation> skeletons;

    private void Awake()
    {
        skeletons = (from o in gameObject.GetChildren() where o.GetComponent<SkeletonAnimation>()
            select o.GetComponent<SkeletonAnimation>()).ToList();
    }

    public IEnumerator StartApproaching()
    {
        yield return Approach();
    }

    private IEnumerator Approach()
    {
        SetAnimation("walk");
        yield return transform.MoveTo(finalPosition, duration);
        SetAnimation("idle");
    }

    private void SetAnimation(string name)
    {
        foreach (SkeletonAnimation s in skeletons)
        {
            s.AnimationName = name;
        }
    }
}
