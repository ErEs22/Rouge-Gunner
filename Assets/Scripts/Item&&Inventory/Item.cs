using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դ�����
/// </summary>
public class Item : ScriptableObject
{
    public string itemName;
    public ResourceType type;
    public virtual void Interact()
    {

    }
}
