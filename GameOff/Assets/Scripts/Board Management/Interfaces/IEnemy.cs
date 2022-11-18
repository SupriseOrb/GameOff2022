using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(float damage);
    void GetAttackTarget(GameObject target);
    void ActivateStampAttack();
    void Stun(float stunDuration);
    void ModifyStat();
}
