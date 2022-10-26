using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour, IInteractable
{
    public Portal portal;

    public void Interact()
    {
        EventManager.current.TransporterInteract();
    }
}
