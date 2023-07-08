using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : Singleton<ViewManager>
{
    public enum SceneId
    {
        BootScene,

        TitleScene,
        LocalMapScene,
    }


    public enum ViewId
    {
        BootView,

        TitleView,
        //LocalMapView,

        Max
    }

    public class ViewSet
    {
        public SceneId SceneId;
        public ViewId ViewId;
        public Type View3DType;
        public Type ViewControllerType;
    }

    private static ViewSet[] _viewSetArray = new ViewSet[(int)ViewId.Max]
    {
        new ViewSet{ ViewId = ViewId.BootView, SceneId = SceneId.BootScene},
        new ViewSet{ ViewId = ViewId.TitleView, SceneId = SceneId.TitleScene},
        //new ViewSet{ ViewId = ViewId.LocalMapView, SceneId = SceneId.LocalMapScene, View3DType = typeof(LocalMapView3D), ViewControllerType = typeof( LocalMapViewController) },
    };

    private ViewSet _currentViewSet = _viewSetArray[(int)ViewId.BootView];
    public ViewSet CurrentViewSet { get { return _currentViewSet; } }

    private ViewControllerBase _currentViewController = null;

    /// <summary>
    /// インスタンスの生成
    /// </summary>
    public static void CreateInstance()
    {
        _instance = new ViewManager();
    }

    public void ChangeView( ViewId viewId )
    {
        if (_currentViewSet.ViewId == viewId)
            return;

        ViewSet nextViewSet = _viewSetArray[(int)viewId];

        // 画面切り替え画面


        // Viewの破棄
        if (_currentViewController != null)
        {
            _currentViewController.DestroyView();
            GameObject.DestroyImmediate(_currentViewController.gameObject);
        }

        if ( nextViewSet.SceneId != _currentViewSet.SceneId )
        {
            // シーン切り替え
            GameSystem.Instance.StartCoroutine(ChangeSceneAsync(nextViewSet));
            return;
        }

        GameSystem.Instance.StartCoroutine(ChangeViewAsync(nextViewSet));
    }

    private IEnumerator ChangeSceneAsync(ViewSet nextViewSet)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextViewSet.SceneId.ToString());

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameSystem.Instance.StartCoroutine(ChangeViewAsync(nextViewSet));
    }

    private IEnumerator ChangeViewAsync(ViewSet nextViewSet)
    {
        const string VIEW_PATH = "UI/View/{0}/{1}";
        GameObject obj = ResourceManager.LoadOnView<GameObject>(Utility.StringFormat( VIEW_PATH, nextViewSet.SceneId, nextViewSet.ViewId ));
        ViewBase view = GameObject.Instantiate<GameObject>(obj).GetComponent<ViewBase>();
        view.transform.SetParent(UIManager.Instance.GameCanvas.transform, false);

        obj = new GameObject(Utility.StringFormat("{0}3D", nextViewSet.ViewId));
        View3DBase view3D = obj.AddComponent(nextViewSet.View3DType) as View3DBase;

        obj = new GameObject(Utility.StringFormat("{0}Controller", nextViewSet.ViewId));
        _currentViewController = obj.AddComponent(nextViewSet.ViewControllerType) as ViewControllerBase;
        _currentViewController.ViewBase = view;
        _currentViewController.View3DBase = view3D;

        _currentViewController.InitializeView();

        yield return null;

        _currentViewSet = _viewSetArray[(int)nextViewSet.ViewId];

        // 画面を開ける

    }
}
