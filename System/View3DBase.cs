using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View3DBase : MonoBehaviour
{
    public virtual void InitializeView()
    {

    }

    public virtual void RegistDownload()
    {

    }

    public virtual IEnumerator LoadViewAsync()
    {
        yield return null;
    }

    public virtual void DestroyView()
    {

    }
}
