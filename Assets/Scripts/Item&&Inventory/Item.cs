using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源类基础
/// </summary>
public class Item : ScriptableObject
{
    public string itemName;
    public ResourceType type;
    public virtual void Interact()
    {

    }
}
