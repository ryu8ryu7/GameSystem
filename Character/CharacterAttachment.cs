using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static CharacterBase;

[System.Serializable]
public class Attachment
{
    /// <summary>
    /// アタッチのTransform
    /// </summary>
    public Transform AttachmentTransform = null;

    /// <summary>
    /// どの位置についているか
    /// </summary>
    public PositionObjName AttachmentPosition = PositionObjName.UnKnown;
}

/// <summary>
/// キャラクター：アタッチメント
/// </summary>
public partial class CharacterBase
{
    /// <summary>
    /// 装着品リスト
    /// </summary>
    [SerializeField]
    private List<Attachment> _attachmentList = new List<Attachment>();
    public List<Attachment> AttachmentList { get { return _attachmentList; } }

    /// <summary>
    /// 
    /// </summary>
    protected void InitializeAttachment()
    {
        FindOriginalAttachment();
        InitialzieEquipmentDummy();
    }

    /// <summary>
    /// アタッチメントをつけるためのダミーをセット
    /// </summary>
    protected void InitialzieEquipmentDummy()
    {
        GameObject obj = gameObject.FindDeep("WeaponRAttach", true);
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
        if (obj == null)
        {
            GameObject parent = gameObject.FindDeep(PositionObjName.Hand_R.ToString(), true);
            if (parent != null)
            {
                obj = ResourceManager.LoadOnView<GameObject>("3D/Character/Attachment/WeaponRAttach");
                obj = GameObject.Instantiate(obj, parent.transform);
                obj.name = obj.name.Replace("(Clone)", string.Empty);
            }
        }

        obj = gameObject.FindDeep("WeaponLAttach", true);
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
        if (obj == null)
        {
            GameObject parent = gameObject.FindDeep(PositionObjName.Hand_L.ToString(), true);
            if (parent != null)
            {
                obj = ResourceManager.LoadOnView<GameObject>("3D/Character/Attachment/WeaponLAttach");
                obj = GameObject.Instantiate(obj, parent.transform);
                obj.name = obj.name.Replace("(Clone)", string.Empty);
            }
        }
    }

    /// <summary>
    /// 元からついているAttachmentの調査
    /// </summary>
    protected void FindOriginalAttachment()
    { 
        Transform[] objArray = gameObject.GetComponentsInChildren<Transform>();
        Transform root = null;
        for (int i = 0; i < objArray.Length; i++)
        {
            if (objArray[i].name.StartsWith("Root"))
            {
                root = objArray[i];
                break;
            }
        }

        if (root == null)
        {
            return;
        }

        objArray = root.GetComponentsInChildren<Transform>();
        for (int i = 0; i < objArray.Length; i++)
        {
            if (objArray[i].name.StartsWith("SM_Chr_Attach") && objArray[i].transform != transform)
            {
                PositionObjName name = GetPosition(objArray[i].parent);

                Attachment attachment = new Attachment();
                attachment.AttachmentTransform = objArray[i];
                attachment.AttachmentPosition = name;
                _attachmentList.Add(attachment);
            }
        }
    }

#if UNITY_EDITOR
    public partial class CharacterBaseEditor : Editor
    {
        private bool _isFoldAttachment = false;
        protected void OnInspectorAttachment()
        {
            CharacterBase chara = target as CharacterBase;

            if (_isFoldAttachment = EditorGUILayout.Foldout(_isFoldAttachment, "Attachment"))
            {
                EditorGUI.indentLevel += 1;

                SerializedProperty prop = serializedObject.FindProperty("_attachmentList");
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(prop, true);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.indentLevel -= 1;
            }
        }

    }

#endif
}
