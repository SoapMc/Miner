using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag) where T : Component
    {
        List<T> results = new List<T>();

        if (gameObject.CompareTag(tag))
            results.Add(gameObject.GetComponent<T>());

        foreach (Transform t in gameObject.transform)
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

        return results.ToArray();
    }
}
