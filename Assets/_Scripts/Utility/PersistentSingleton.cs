using UnityEngine;

/// <summary>
///     <para>
///         Maintains Singleton property of child classes, with DontDestroyOnLoad.
///     </para>
///     
///     SHOULD NOT BE INSTANTIATED ON ITS OWN, PRIMARILY A BASE CLASS
/// </summary>
/// <typeparam name="T"></typeparam>
public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; protected set; }



    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this.GetComponent<T>();
        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}