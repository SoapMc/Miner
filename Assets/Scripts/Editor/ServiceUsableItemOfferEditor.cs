using UnityEditor;
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