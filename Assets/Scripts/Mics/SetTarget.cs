using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class SetTarget : MonoBehaviour
{
    [SerializeField] bool dynamicScan;

    [SerializeField] float dynamicScanInterval = 0.1f;

    Ray ray;

    RaycastHit raycastHit;

    IAstarAI ai;

    WaitForSeconds waitForScan;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();

        waitForScan = new WaitForSeconds(dynamicScanInterval);
    }

    private void Start()
    {
        if (dynamicScan)
        {
            StartCoroutine(nameof(DynamicScanCoroutine));
        }
    }

    private void Update()
    {
        // print(ai.velocity);
        // AstarPath.active.Scan();
        if (Mouse.current.leftButton.isPressed)
        {
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out raycastHit);
            ai.destination = raycastHit.point;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(DynamicScanCoroutine));
    }

    IEnumerator DynamicScanCoroutine()
    {
        while (true)
        {
            yield return waitForScan;
            AstarPath.active.Scan();
        }
    }
}
