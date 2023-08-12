using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();

                if (instance == null)
                {
                    T inst = new GameObject().AddComponent<T>();
                    inst.name = typeof(T).Name;

                    instance = inst;
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
