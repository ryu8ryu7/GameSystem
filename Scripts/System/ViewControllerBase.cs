using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewControllerBase : Updater
{
    protected ViewBase _viewBase = null;
    public ViewBase ViewBase { set { _viewBase = value; } }

    protected View3DBase _view3DBase = null;
    public View3DBase View3DBase { set { _view3DBase = value; } }

    public virtual void InitializeView()
    {
        UpdaterManager.Instance.AddUpdater(this);

        _viewBase.InitializeView();
        _view3DBase.InitializeView();
    }

    public virtual void RegistDownload()
    {
        _viewBase.RegistDownload();
        _view3DBase.RegistDownload();
    }

    public virtual IEnumerator LoadViewAsync()
    {
        yield return StartCoroutine(_viewBase.LoadViewAsync());
        yield return StartCoroutine(_view3DBase.LoadViewAsync());
    }

    public virtual void DestroyView()
    {
        UpdaterManager.Instance.RemoveUpdater(this);

        _viewBase.DestroyView();
        _view3DBase.DestroyView();
    }
}
