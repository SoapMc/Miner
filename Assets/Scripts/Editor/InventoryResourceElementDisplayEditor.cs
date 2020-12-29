using UnityEditor;
using Miner.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryResourceElementDisplay))]
public class InventoryResourceElementDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif