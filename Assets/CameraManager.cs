using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera fpsCam;
    [SerializeField] LayerMask interactLayer;//可能移植到相机管理上，暂用于互动检测
    InputManager inputManager;
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }


    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Debug.DrawRay(origin, transform.forward);
        if (Physics.Raycast(origin,transform.forward, out hit, 3f, interactLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Debug.Log("Detected");
                IInteractable interactableObject = hit.collider.GetComponent<IInteractable>();

                if (interactableObject != null)
                {
                    //显示互动UI
                    if (inputManager.interact_Input)
                    {
                        interactableObject.Interact();
                    }
                }
            }
        }
        else
        {
            //关闭互动UI
        }
    }
}
