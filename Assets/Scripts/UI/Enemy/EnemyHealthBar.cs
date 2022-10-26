using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : StateBar
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
