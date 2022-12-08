using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemStamp : IStamp
{
    // TODO: Consider if this can be consolidated in a class instead of inheritance
    // might not be possible due to different animation names
    void TakeDamage(float damage);
    // TODO : Make sure naming conventions are the same
    // TODO : Move this to a base item stamp class because functionality is the same
    void HealHealth(float heal);
    // TODO : Move this to a base item stamp class because functionality is the same
    // might even just exist on the SO lol
    bool IsDead();
}
