using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothManager : Singleton<ClothManager>
{
    private MagicaCloth.MagicaDirectionalWind _wind = null;

    private MagicaCloth.MagicaPhysicsManager _manager = null;

    /// <summary>
    /// インスタンスの生成
    /// </summary>
    public static void CreateInstance()
    {
        _instance = new ClothManager();
    }

    public void Initialize()
    {
        _wind = new GameObject("MagicaDirectionalWind").AddComponent<MagicaCloth.MagicaDirectionalWind>();
        _wind.transform.SetParent(GameSystem.Instance.transform);

        _manager = new GameObject("MagicaPhysicsManager").AddComponent<MagicaCloth.MagicaPhysicsManager>();
        _manager.transform.SetParent(GameSystem.Instance.transform);

    }
}
