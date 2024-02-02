using UnityEngine;

// Maintains Singleton property of child classes
// Singletons using this class utilize lazy-loading and remain persistent between scenes
// 
// SHOULD NOT BE INSTANTIATED ON ITS OWN, PRIMARILY A BASE CLASS
public class LLPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool applicationIsQuitting = false;
    private static object _lock = null;

    private static T _instance = null;
    public static T instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            _lock = new object();
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        GameObject newObj = new GameObject(name: typeof(T).Name);
                        _instance = newObj.AddComponent<T>();

                        DontDestroyOnLoad(newObj);
                    }
                    
                    else
                    {
                        Debug.LogError($"[Duplicate SceneSingleton Error]: Duplicate SceneSingleton {typeof(T).Name} found in scene.");
                    }
                }

                return _instance;
            }
        }
    }

    // Unity destroys objects in a random order when quitting. If any object
    // calls the instance after it has been destroyed, this prevents creation
    // of "ghost objects" that stick around in editor after application quit.
    public void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}