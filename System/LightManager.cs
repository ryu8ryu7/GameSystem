using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : Singleton<LightManager>
{
    private Light _light = null;

    /// <summary>
    /// インスタンスの生成
    /// </summary>
    public static void CreateInstance()
    {
        _instance = new LightManager();
    }


    public void Initialize()
    {
        _light = new GameObject("Light").AddComponent<Light>();
        _light.transform.SetParent(GameSystem.Instance.transform);

        _light.type = LightType.Directional;
        _light.transform.rotation = Quaternion.Euler(50, 30, 0);
        _light.shadows = LightShadows.Soft;
    }


}
