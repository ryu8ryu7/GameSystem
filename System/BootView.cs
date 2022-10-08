using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootView : ViewBase
{
    void Start()
    {
        ViewManager.Instance.ChangeView(ViewManager.ViewId.LocalMapView);
    }
}
