using UnityEditor;
using Miner.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(PartGridElement))]
public class PartGridElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif