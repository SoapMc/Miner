using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Miner.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(ServiceUsableItemOffer))]
public class ServiceUsableItemOfferEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif