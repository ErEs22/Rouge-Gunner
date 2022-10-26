using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DisableMark();
    }

    public bool connected;
    public void Dye(Color color)
    {
        spriteRenderer.color = color;
    }

    public void EnableMark(Vector3 position,Color color)
    {
        gameObject.SetActive(true);
        connected=true;
        transform.position= position;
        Dye(color);
    }

    public void DisableMark()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        
    }
}
