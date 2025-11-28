using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T m_Instance;
    //Do NOT call in OnDestroy, prefer OnDisable
    public static T Instance
    {
        get
        {
            return m_Instance ?? new GameObject(nameof(T)).AddComponent<T>();
        }
        private set
        {
            m_Instance = value;
        }
    }

    public virtual void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}