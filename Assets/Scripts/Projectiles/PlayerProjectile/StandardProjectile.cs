using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : BaseProjectile
{

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    protected override void Move()
    {
        base.Move();
    }
}
