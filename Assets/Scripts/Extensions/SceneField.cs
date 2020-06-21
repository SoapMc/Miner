using UnityEngine;


[System.Serializable]
public class SceneField
{
    [SerializeField]
    private Object _sceneAsset;
    [SerializeField]
    private string _sceneName = "";
    public string SceneName
    {
        get { return _sceneName; }
    }

    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }
}
