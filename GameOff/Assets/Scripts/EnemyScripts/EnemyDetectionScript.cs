using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    /*
    TODO: 
    For the Whiteout Carriage, there should probably be an OverlapCircle instead which checks for IItemStamps instead in order to
    make the attacks from the Carriage AoE.
    */
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent(out IItemStamp item))
        {
            Debug.Log("Enemy attacking item stamp!");
            GetComponentInParent<IEnemy>().GetAttackTarget(other.gameObject);
        }
    }
}
