using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("peepee poopoo");
        if(other.TryGetComponent(out IItemStamp item))
        {
            GetComponentInParent<IEnemy>().GetAttackTarget(other.gameObject);
        }
    }
}
