using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "GameOff2022/Wave", order = 0)]
public class WaveScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct EnemySpawn
    {
        [Tooltip("Array of prefab of the enemies that will be spawned in lane one. Leave this array empty if you dont want to spawn anything in this lane.")]
        public GameObject[] _laneOneEnemies; //Array of prefab of the enemies that will be spawned in lane 1
        [Tooltip("Array of prefab of the enemies that will be spawned in lane two. Leave this array empty if you dont want to spawn anything in this lane.")]
        public GameObject[] _laneTwoEnemies; //Array of prefab of the enemies that will be spawned in lane 2
        [Tooltip("Array of prefab of the enemies that will be spawned in lane three. Leave this array empty if you dont want to spawn anything in this lane.")]
        public GameObject[] _laneThreeEnemies; //Array of prefab of the enemies that will be spawned in lane 3

        [Tooltip("How many seconds into the wave they will be spawned")]
        public int _timeToSpawn; //What time the enemy will be spawned
    }

    [Tooltip("All of the enemy spawns that will occur in this wave")]
    [SerializeField] private List<EnemySpawn> _wave;
    
    [Tooltip("How many seconds the wave will last")]
    [SerializeField] private float _waveDuration;

    [Tooltip("How many seconds before the wave starts from a break between waves")]
    [SerializeField] private float _waveBreakBeforeDuration;

    public List<EnemySpawn> Wave
    {
        get {return _wave;}
    }

    public float WaveDuration
    {
        get {return _waveDuration;}
    }

    public float WaveBreakBeforeDuration
    {
        get {return _waveBreakBeforeDuration;}
    }

}
