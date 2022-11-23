using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemStamp : IStamp
{
    void TakeDamage(float damage);
    void HealHealth(float heal);
    bool IsDead();
}
