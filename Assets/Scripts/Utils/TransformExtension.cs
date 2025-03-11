using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension 
{
    public static void Clear(this Transform transform)
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            Object.DestroyImmediate(child.gameObject);
        }
    }
}
