using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{

    private void Awake()
    {
        EventManager.current.OnCountDownUIStarted += EnableUI;
        EventManager.current.OnCountDownUIEnded += DisableUI;
    }

    void EnableUI()
    {
        gameObject.SetActive(true);
    }

    void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
