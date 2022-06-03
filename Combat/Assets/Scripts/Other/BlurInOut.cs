using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BlurInOut : MonoBehaviour
{
    [SerializeField]
    private float blurStrength;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public IEnumerator BlurIn(float duration)
    {
        yield return Lerp(0, blurStrength, duration);
    }

    public IEnumerator BlurOut(float duration)
    {
        yield return Lerp(blurStrength, 0, duration);
    }

    private IEnumerator Lerp(float startValue, float endValue, float lerpDuration)
    {
        float timeElapsed = 0;
        
        while (timeElapsed < lerpDuration)
        {
            rend.material.SetFloat("_Size", Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rend.material.SetFloat("_Size", endValue);
    }
}
