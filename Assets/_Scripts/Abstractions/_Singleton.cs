using UnityEngine;

public abstract class _Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    /// <summary>
    /// If true, the singleton will not be destroyed when loading a new scene.
    /// will be set to false if the singleton is a child of another object.
    /// </summary>
    [SerializeField] bool dontDestroyOnLoad = true;
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;

        Init();

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
    private void OnValidate()
        => Init();
    private void Init()
    {
        gameObject.name = $"[{typeof(T).Name}]";

        if (gameObject.transform.parent != null)
            dontDestroyOnLoad = false;
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
