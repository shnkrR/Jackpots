using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private static GameManager m_Instance;
    public static GameManager _Instance { get { return m_Instance; } }


    private void Awake()
    {
        if (GameManager._Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        m_Instance = this;
    }

    private void Start()
    {

    }
}
