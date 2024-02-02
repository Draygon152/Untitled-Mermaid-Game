using UnityEngine;

// Maintains Singleton property of child classes
//
// SHOULD NOT BE INSTANTIATED ON ITS OWN, PRIMARILY A BASE CLASS
public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; protected set; }



    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError($"[Duplicate SceneSingleton Error]: Duplicate SceneSingleton {typeof(T).Name} found in scene.");
            return;
        }

        instance = this.GetComponent<T>();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}