using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    static private T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"[Singleton] {typeof(T)} not found in scene.");    
            }
            
            return _instance;
        }
    }

    // 에러 로그 없이 인스턴스 존재 여부만 확인 (앱 종료 시 파괴 순서 대비)
    public static bool HasInstance => _instance != null;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"[Singleton] {typeof(T).Name} 중복 인스턴스 파괴: {gameObject.name}");
            Destroy(gameObject);
        }
    }
    
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
