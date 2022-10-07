using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public class CameraSet
    {
        public Camera _camera = null;
        public CameraLookDown _lookDown = null;

        public void Initialize( string name )
        {
            _camera = new GameObject(name).AddComponent<Camera>();
            _camera.transform.SetParent(GameSystem.Instance.transform);

            _lookDown = _camera.gameObject.AddComponent<CameraLookDown>();
        }
    }


    private CameraSet _mainCamera = null;



    /// <summary>
    /// インスタンスの生成
    /// </summary>
    public static void CreateInstance()
    {
        _instance = new CameraManager();
    }

    public void Initialize()
    {
        _mainCamera = new CameraSet();
        _mainCamera.Initialize("MainCamera");
    }
}
