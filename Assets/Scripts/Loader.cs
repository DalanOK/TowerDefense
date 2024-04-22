using UnityEngine;

public class Loader <T>: MonoBehaviour where T : MonoBehaviour 
{
    private static T instance;

    public static T Instance
    { get 
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
            }
            else if (instance != FindAnyObjectByType<T>())
            {
                Destroy(FindAnyObjectByType<T>());
            }
            DontDestroyOnLoad(FindAnyObjectByType<T>());
            return instance; 
        } 
    }
}
