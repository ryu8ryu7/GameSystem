using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterBase
{
    /// <summary>
    /// Position種別
    /// </summary>
    public enum PositionObjName
    {
        Root,

        Head,

        Hips,
        Spine_01,
        Spine_03,

        Hand_L,
        Hand_R,

        BackpackAttach,
        WeaponHolder_Back01,
        WeaponHolder_Back02,

        WeaponL01,
        WeaponL02,
        WeaponL02_RHand01,

        WeaponR01,
        WeaponR01_LHand01,
        WeaponR01_LHand02,
        WeaponR01_LHand03,
        WeaponR01_RHand01,

        WeaponR02,
        WeaponR03,

        // 捜索が行われるのはここまで--------------------------------------

        UnKnown,

        Max
    }

    protected List<Transform> _positionList = new List<Transform>();
    public List<Transform> PositionList { get { return _positionList; } }

    protected virtual void InitializePosition()
    {
        GameObject obj = null;

        for (int i = 0; i < (int)PositionObjName.Max; i++)
        {
            PositionObjName name = (PositionObjName)i;

            if( name >= PositionObjName.UnKnown )
            {
                break;
            }

            obj = gameObject.FindDeep(name.ToString(), true);
            if (obj != null)
            {
                _positionList.Add(obj.transform);
            }
            else
            {
                _positionList.Add(null);
            }
        }
    }

    protected virtual Transform GetPosition(PositionObjName positionObjName)
    {
        return _positionList[(int)positionObjName];
    }

    /// <summary>
    /// ポジション検索（どこのものか）
    /// </summary>
    /// <param name="checkTransform"></param>
    /// <returns></returns>
    protected PositionObjName GetPosition( Transform checkTransform ) 
    {
        int index = _positionList.IndexOf(checkTransform);
        if( index < 0 )
        {
            return PositionObjName.UnKnown;
        }

        return (PositionObjName)index;
    }
}
