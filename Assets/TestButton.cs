using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TestButton : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        //检测是否射线是否触碰到物体

        if (Physics.Raycast(ray, out hit, layerMask))
            Debug.Log("Hitten");
    }
}
