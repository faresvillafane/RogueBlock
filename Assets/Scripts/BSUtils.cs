using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSUtils : MonoBehaviour {

    /// <summary>
    /// Returns a random integer between min and max value.
    /// </summary>
    public static float RandFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    /// <summary>
    /// Returns a random integer between min and max value.
    /// </summary>
    public static int RandInt(int min, int max)
    {
        return (int)UnityEngine.Random.Range(min, max + 1);
    }
    /// <summary>
    /// Returns a random integer from 0 to array index -1
    /// </summary>
    public static int RandArrayIndex<T>(T[] array)
    {
        return RandInt(0, array.Length - 1);
    }
    /// <summary>
    /// Returns a random integer from 0 to List index -1
    /// </summary>
    public static int RandListIndex<T>(List<T> array)
    {
        return RandInt(0, array.Count - 1);
    }
    /// <summary>
    /// Returns a random element from an array
    /// </summary>
    public static T RandElement<T>(T[] array)
    {
        return array[RandInt(0, array.Length - 1)];
    }
    /// <summary>
    /// Shuffles all list elements.
    /// </summary>
    public static void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    /// <summary>
    /// Returns a random bool.
    /// </summary>
    public static bool RandBool()
    {
        return RandInt(0, 1) == 1;
    }

    public static void SetAlpha(UnityEngine.UI.Image img, float alpha)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

    }

    public static void SetAlpha(UnityEngine.SpriteRenderer spr, float alpha)
    {
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);

    }

    public static void SetAlpha(UnityEngine.UI.Text txt, float alpha)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);

    }

    public static void SetAlpha(TextMesh txt, float alpha)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);

    }


    public static int EnumSize<T>(T enumVar)
    {
        return System.Enum.GetValues(typeof(T)).Length;
    }

    public static bool VectorAtTarget(Vector3 vectorStart, Vector3 vectorFinish)
    {
        return VectorAtTarget(vectorStart, vectorFinish, BSConstants.MIN_LERP_DISTANCE);
    }
    public static bool VectorAtTarget(Vector3 vectorStart, Vector3 vectorFinish, float distanceThreshold)
    {
        return Vector3.Distance(vectorStart, vectorFinish) <= distanceThreshold;
    }

    public static bool LerpAtTarget(float fCurrentPosition, float fTarget)
    {
        return LerpAtTarget(fCurrentPosition, fTarget, BSConstants.MIN_LERP_DISTANCE);
    }

    public static bool LerpAtTarget(Vector3 v3CurrentPosition, Vector3 v3Target)
    {
        return LerpAtTarget(v3CurrentPosition, v3Target, BSConstants.MIN_LERP_DISTANCE);
    }
    public static bool LerpAtTarget(Vector3 v3CurrentPosition, Vector3 v3Target, float fTolerance)
    {
        return Vector3.Distance(v3CurrentPosition, v3Target) <= fTolerance;
    }

    public static bool LerpAtTarget(float fCurrentPosition, float fTarget, float fTolerance)
    {
        return Mathf.Abs(fCurrentPosition - fTarget) <= fTolerance;
    }

    public static Vector2 WorldPosToMatrix(Vector3 v3Position)
    {
        return new Vector2(v3Position.x, v3Position.z);
    }

}
