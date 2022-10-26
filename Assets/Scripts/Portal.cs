using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] int destinationID;
    [SerializeField] Room destination;
    bool portalEnabled;
    private void OnEnable()
    {
        //PortalManager.Instance.portalDic.Add(this.id, this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (portalEnabled)
        {
            PlayerManager.Instance.PrepareTeleporting();
            PlayerManager.Instance.TraverseOtherRoom(destination.reachPoint.transform.position);
        }
  

  
    }

    private void OnTriggerStay(Collider other)
    {
        if (!portalEnabled&& other.CompareTag("Player") && PlayerManager.Instance.isTeleporting)
        {
            PlayerManager.Instance.isTeleporting = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        portalEnabled = false;

    }

    internal void SetDestination(Room room)
    {
        portalEnabled = true;
        destination =room;
    }
}
