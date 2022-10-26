using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrop : MonoBehaviour, ICollectible
{
    public ResourceType type;
    public int amount;
    public void Collect(InventoryManager inventoryManager)
    {
        if (type == ResourceType.Gold)
        {
            if (inventoryManager.GetGold(amount))
            {
                Destroy(gameObject);
            }
        }
        else if (type == ResourceType.Key)
        {
            if (inventoryManager.GetKey(amount))
            {
                Destroy(gameObject);
            }
            
        }
        else if(type == ResourceType.Boom)
        {
            if (inventoryManager.GetBoom(amount))
            {
                Destroy(gameObject);
            }            
        }
        else if (type == ResourceType.Health)
        {
            if (inventoryManager.GetHealth(amount))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (inventoryManager.GetAmmo(amount))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var inventoryManager = other.GetComponent<InventoryManager>();
            Collect(inventoryManager);
        }
    }
}
