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

    [SerializeField]
    private bool _isAwakeInitialize = false;

    private void Start()
    {
        if( _isAwakeInitialize )
        {
            Initialize();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public virtual void Initialize()
    {
        InitializeCharacterController();
        InitializePosition();
        InitializeAttachment();
        InitializeAnimation();

        LoadAnimation(LoadAnimationSetData(10001));
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
    /// �j��
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
            }
        }
    }

#endif
}
