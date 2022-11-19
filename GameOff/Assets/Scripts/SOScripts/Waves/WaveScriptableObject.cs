using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "GameOff2022/Wave", order = 0)]
public class WaveScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawn
    {
        public List<GameObject> _enemies; //Prefab of enemy that will be spawned
        public float _timeToSpawn; //What time the enemy will be spawned
        public int _laneNum; //What lane the enemy will spawn in (should this be rng?)
        public bool _isEnemySummoned; //Has the enemy been summoned?
    }

    [SerializeField] private List<EnemySpawn> _wave;

    public List<EnemySpawn> Wave
    {
        get {return _wave;}
    }

    public EnemySpawn GetEnemySpawn(int index)
    {
        return _wave[index];
    }

    private void OnEnable() //Resets SO values
    {
        foreach (EnemySpawn spawn in _wave)
        {
            spawn._isEnemySummoned = false;
        }
    }

}
