using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ViewManager.Instance.ChangeView(ViewManager.ViewId.LocalMapView);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
