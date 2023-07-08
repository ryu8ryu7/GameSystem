using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonUpdater<UIManager>
{
    [SerializeField]
    private Canvas _gameCanvas = null;
    public Canvas GameCanvas { get { return _gameCanvas; } }


    [SerializeField]
    private Canvas _systemCanvas = null;
    public Canvas SystemCanvas { get { return _systemCanvas; } }

    [SerializeField]
    private Camera _camera = null;

    public static void CreateInstance()
    {
        if (HasInstance() == true)
        {
            return;
        }

        GameObject gameObject = Instantiate<GameObject>(ResourceManager.LoadOnView<GameObject>("UI/UIManager"));
        gameObject.transform.SetParent(GameSystem.Instance.transform);
        _instance = gameObject.GetComponent<UIManager>();
        _instance.Initialize();
    }

    public void Initialize()
    {
        UpdaterManager.Instance.AddUpdater(this);
    }
}
