using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public class CameraSet
    {
        private Camera _camera = null;
        public Camera Camera { get { return _camera; } }

        private CameraLookDown _lookDown = null;
        public CameraLookDown LookDown { get { return _lookDown; } }

        public void Initialize( string name )
        {
            _camera = new GameObject(name).AddComponent<Camera>();
            _camera.transform.SetParent(GameSystem.Instance.transform);
            _camera.fieldOfView = 45.0f;

            _lookDown = _camera.gameObject.AddComponent<CameraLookDown>();
        }
    }


    private CameraSet _mainCamera = null;
    public CameraSet MainCamera { get { return _mainCamera; } }


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
