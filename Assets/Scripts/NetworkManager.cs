using System;

using UnityEngine;
using Utils;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    private static readonly object lockObj = new object();

    public DataHandler DataHandler => _dataHandler;

    private DataHandler _dataHandler;

    public static NetworkManager Instance => _instance;

    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Time.fixedDeltaTime = 1.0f / Application.targetFrameRate;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else if (_instance == null)
        {
            lock (lockObj)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                Init();
            }
        }
    }

    private void Init()
    {
        _dataHandler = new DataHandler();
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Quit the application
                Application.Quit();
            }
        }
    }
}