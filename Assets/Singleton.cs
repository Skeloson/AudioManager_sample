using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Généralisation du pattern Singleton
/// by Leclef Jérémy 27-09-18
/// </summary>
/// <typeparam name="T">MonoBehaviour</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject l_singleton = new GameObject();
                    _instance = l_singleton.AddComponent<T>();
                    l_singleton.name = typeof(T).ToString();

                    DontDestroyOnLoad(l_singleton);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(!_instance)
        {
            //voir si il n'y a pas moyen de trouver mieux
            _instance = this.gameObject.GetComponent<T>();
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
