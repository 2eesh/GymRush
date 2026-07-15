using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        var prefab = Resources.Load<GameObject>("Managers");

        if (prefab == null)
        {
            Debug.LogError("[Bootstrapper] Resources/Managers 프리팹을 찾을 수 없음.");
            return;
        }
        
        var go = Object.Instantiate(prefab);
        go.name = "Managers";
        Object.DontDestroyOnLoad(go);
    }
}
