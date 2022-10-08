using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    /// <summary>
    /// óDêÊèá
    /// </summary>
    protected int _updatePriority = 0;
    public int UpdatePriority { get { return _updatePriority; } }

    public virtual void PreAlterFixedUpdate() { }

    public virtual void AlterFixedUpdate() { }

    public virtual void PreAlterUpdate() { }

    public virtual void AlterUpdate() { }

    public virtual void PreAlterLateUpdate() { }

    public virtual void AlterLateUpdate() { }
}

