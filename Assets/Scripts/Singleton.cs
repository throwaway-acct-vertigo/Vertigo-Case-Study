using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public bool DontDestroyThisOnLoad;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T existing = FindFirstObjectByType<T>();
                if (existing != null)
                {
                    _instance = existing;
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (DontDestroyThisOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}