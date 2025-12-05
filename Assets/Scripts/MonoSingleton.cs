using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isApplicationQuitting = false;

    [Header("是否跨场景销毁")]
    [SerializeField] private bool _persistentAcrossScenes = true; // 是否跨场景持久化
    public static T Instance
    {
        get
        {
            if (_isApplicationQuitting)
            {
                Debug.LogWarning($"[{typeof(T).Name}] 应用程序正在退出，返回null");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject($"{typeof(T).Name}_Singleton");
                        _instance = singletonObject.AddComponent<T>();

                        _instance.InitializeSingleton();

                        Debug.Log($"[{typeof(T).Name}] 创建新的单例实例");
                    }
                    else
                    {
                        _instance.InitializeSingleton();
                    }
                }
                return _instance;
            }
        }
    }
    public static bool IsInitialized => _instance != null;

    public bool PersistentAcrossScenes
    {
        get => _persistentAcrossScenes;
        set => _persistentAcrossScenes = value;
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_persistentAcrossScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
            OnSingletonInitialized();
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[{typeof(T).Name}] 检测到重复的单例实例，销毁: {gameObject.name}");
            DestroyImmediate(gameObject);
        }
    }

    protected virtual void OnSingletonInitialized()
    {
        Debug.Log($"[{typeof(T).Name}] 单例初始化完成");
    }
    public static void DestroyInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
        _instance = null;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            _isApplicationQuitting = true;
        }
    }
}
