using UnityEditor;
using Miner.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(MissionButton))]
public class MissionButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif