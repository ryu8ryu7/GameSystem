using System.Collections;
using System.Collections.Generic;
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
        LocalMapView,

        Max
    }

    public class ViewSet
    {
        public SceneId SceneId;
        public ViewId ViewId;
        public string SceneName;
    }

    private static ViewSet[] _viewSetArray = new ViewSet[(int)ViewId.Max]
    {
        new ViewSet{ ViewId = ViewId.BootView, SceneId = SceneId.BootScene, SceneName = "Boot"},
        new ViewSet{ ViewId = ViewId.TitleView, SceneId = SceneId.TitleScene, SceneName = "Title"},
        new ViewSet{ ViewId = ViewId.LocalMapView, SceneId = SceneId.LocalMapScene, SceneName = "LocalMap"},
    };

    private ViewSet _currentViewSet = _viewSetArray[(int)ViewId.BootView];
    public ViewSet CurrentViewSet { get { return _currentViewSet; } }

    private SceneBase _currentScene = null;

    private ViewBase _currentView = null;

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
        if (_currentView != null)
        {
            _currentView.DestroyView();
        }

        if ( nextViewSet.SceneId != _currentViewSet.SceneId )
        {
            // シーン切り替え

            // Sceneの破棄
            if( _currentScene != null )
            {
                _currentScene.DestroyScene();
            }

            GameSystem.Instance.StartCoroutine(ChangeSceneAsync(nextViewSet));
            return;
        }

        GameSystem.Instance.StartCoroutine(ChangeViewAsync(nextViewSet));
    }

    private IEnumerator ChangeSceneAsync(ViewSet nextViewSet)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextViewSet.SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameSystem.Instance.StartCoroutine(ChangeViewAsync(nextViewSet));
    }

    private IEnumerator ChangeViewAsync(ViewSet nextViewSet)
    {
        yield return null;


    }
}
