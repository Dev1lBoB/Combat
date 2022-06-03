using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionHelper
{
    #region GetChildren
    public static List<GameObject> GetChildren(this GameObject go)
    {
        // Return a list of all the children objects
        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in go.transform)
        {
            children.Add(t.gameObject);
        }
        return children;
    }
    #endregion GetChildren

    #region MoveTo
    public static IEnumerator MoveTo(this Transform ts, Vector2 destination, float duration)
    {
        Vector3 finalDestination = new Vector3(destination.x, destination.y, ts.position.z); // Save Z axis to not mess up the rendering order
        yield return MoveLerp(ts, ts.position, finalDestination, duration);
    }

    private static IEnumerator MoveLerp(Transform ts, Vector3 startValue, Vector3 endValue, float lerpDuration)
    {
        float timeElapsed = 0;
        
        while (timeElapsed < lerpDuration)
        {
            ts.position = Vector3.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ts.position = endValue;
    }
    #endregion MoveTo

    #region RescaleTo
    public static IEnumerator RescaleTo(this Transform ts, Vector2 destination, float duration)
    {
        Vector3 finalDestination = new Vector3(destination.x, destination.y, ts.position.z); // Save Z axis to not mess up the rendering order
        yield return RescaleLerp(ts, ts.localScale, finalDestination, duration);
    }

    private static IEnumerator RescaleLerp(Transform ts, Vector3 startValue, Vector3 endValue, float lerpDuration)
    {
        float timeElapsed = 0;
        
        while (timeElapsed < lerpDuration)
        {
            ts.localScale = Vector3.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ts.localScale = endValue;
    }
    #endregion RescaleTo
}
