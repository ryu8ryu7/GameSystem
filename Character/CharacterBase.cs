using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public partial class CharacterBase : Updater
{
    [SerializeField]
    private CharacterController _characterController = null;

    private CameraLookDown _cameraLookDown = null;
    public CameraLookDown CameraLookDown { set { _cameraLookDown = value; } }

    /// <summary>
    /// AwakeÇ≈èâä˙âª
    /// </summary>
    [SerializeField]
    private bool _isAwakeInitialize = false;

    [SerializeField]
    private int _animationSetIndex = -1;

    private void Start()
    {
        if( _isAwakeInitialize )
        {
            Initialize();
        }
    }

    /// <summary>
    /// èâä˙âª
    /// </summary>
    public virtual void Initialize()
    {
        InitializeCharacterController();
        InitializePosition();
        InitializeAttachment();
        InitializeAnimation();

        int setIndex = _animationSetIndex < 0 ? 10001 : _animationSetIndex;
        LoadAnimation(LoadAnimationSetData(setIndex));
        PlayAnimation(_animationSetScriptableObject.Get(AnimationSetScriptableObject.AnimationSetNameLabel.Idle01));

        _updatePriority = (int)UpdaterManager.Priority.Character;

        UpdaterManager.Instance.AddUpdater(this);
    }

    private void InitializeCharacterController()
    {
        if (!TryGetComponent<CharacterController>(out _characterController))
        {
            _characterController = CharacterLoader.AddCharacterController(gameObject);
        }
    }

    /// <summary>
    /// PreAlterUpdate
    /// </summary>
    public override void PreAlterUpdate()
    {
        base.PreAlterUpdate();
    }

    /// <summary>
    /// AlterUpdate
    /// </summary>
    public override void AlterUpdate()
    {
        base.AlterUpdate();

        UpdateInput();
        UpdateAnimation();
        //UpdateFollowPosition();
        //UpdateFollow();
        UpdateMove();
    }

    /// <summary>
    /// îjä¸
    /// </summary>
    private void OnDestroy()
    {
        UpdaterManager.Instance.RemoveUpdater(this);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CharacterBase))]
    public partial class CharacterBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterBase chara = target as CharacterBase;
            OnInspectorBase();
            OnInspectorControl();
            OnInspectorAnimation();
            OnInspectorIkController();
            OnInspectorAttachment();
            EditorUtility.SetDirty(chara);
        }

        private bool _isFoldBase = false;
        protected void OnInspectorBase()
        {
            CharacterBase chara = target as CharacterBase;

            if (_isFoldBase = EditorGUILayout.Foldout(_isFoldBase, "Base"))
            {
                chara._isAwakeInitialize = EditorGUILayout.Toggle("_isAwakeInitialize", chara._isAwakeInitialize);
                chara._animationSetIndex = EditorGUILayout.IntField("_animationSetIndex", chara._animationSetIndex);
            }
        }
    }

#endif
}
