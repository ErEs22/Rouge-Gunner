using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;



    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    public event Action OnEntityRoomGenerate;
    public event Action OnCoolingStop;//未来可能出现武器不能正常CD的情况
    public event Action OnDropCollected;//获得掉落物触发
    public event Action OnCountDownUIStarted;
    public event Action OnCountDownUIEnded;
    public event Action OnDungeonGenerated;//地牢生成完毕
    public event Action OnTransporterInteract;
    public event Action OnPortalSelected;
    public event Action OnTeleported;

    public void EntityRoomGenerate()
    {
        if (OnEntityRoomGenerate != null)
        {
            OnEntityRoomGenerate();
        }
    }

    public void DropCollected()
    {
        OnDropCollected?.Invoke();
    }

    public void DungeonGenerated()
    {
        OnDungeonGenerated?.Invoke();
    }

    public void CountDownUIStarted()
    {
        OnCountDownUIStarted?.Invoke();
    }

    public void CountDownUIEnded()
    {
        OnCountDownUIEnded?.Invoke();
    }

    public void Teleport()
    {
        OnTeleported?.Invoke();
    }

    public void TransporterInteract()
    {
        OnTransporterInteract?.Invoke();
    }

    public void PortalSelected()
    {
        OnPortalSelected?.Invoke();
    }

}
