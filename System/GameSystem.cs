using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// GameSystem
/// </summary>
public class GameSystem : SingletonMonoBehavior<GameSystem>
{
    /// <summary>
    /// フレームレート
    /// </summary>
    public const int FRAME_RATE = 60;

    /// <summary>
    /// 初期化
    /// </summary>
    private bool _isInitialized = false;

    /// <summary>
    /// インスタンス生成
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void CreateInstance()
    {
        if (HasInstance() == true)
        {
            return;
        }

        _instance = new GameObject("GameSystem").AddComponent<GameSystem>();
        _instance.Initialize();

        DontDestroyOnLoad(_instance.gameObject);
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if( _isInitialized )
        {
            return;
        }

        Application.targetFrameRate = FRAME_RATE;
        Screen.SetResolution(1920, 1080, true);

        ViewManager.CreateInstance();
        UpdaterManager.CreateInstance();
        ResourceManager.CreateInstance();
        MasterDataManager.CreateInstance();
        ClothManager.CreateInstance();
        CameraManager.CreateInstance();
        LightManager.CreateInstance();

        MasterDataManager.Instance.LoadMaster();
        ClothManager.Instance.Initialize();
        CameraManager.Instance.Initialize();
        LightManager.Instance.Initialize();

        UIManager.CreateInstance();

        _isInitialized = true;
    }

    private void FixedUpdate()
    {
        UpdaterManager.Instance.FixedUpdate();
    }

    private void Update()
    {
        UpdaterManager.Instance.Update();
    }

    private void LateUpdate()
    {
        UpdaterManager.Instance.LateUpdate();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameSystem))]
    public partial class GameSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameSystem gameSystem = target as GameSystem;

            Utility.OnInspectorGUI();

            EditorUtility.SetDirty(gameSystem);
        }

    }

#endif
}
