using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveScriptableObject> _waves;
    [SerializeField] private Transform[] _spawnLocations;
    [SerializeField] private BoolVariable _isInWave;
    private Dictionary<int, GameObject[]> _currentWaveSpawns;

    [Header("Debug Variables")]
    [SerializeField] private WaveScriptableObject _currentWave;
    [SerializeField] private GameObject[] _currentEnemySpawns;
    [SerializeField] private int _currentWaveIndex;
    //[SerializeField] private List<int> _spawnTimes;

#region Wave Timer Variables
    //Remove _isInWaveValueDebug
    [SerializeField] private bool _isInWaveValueDebug;
    [SerializeField] private float _baseWaveDuration;
    [SerializeField] private float _currentWaveDuration;
    [SerializeField] private int _currentWaveTime;
    [SerializeField] private int _previousWaveTime;
    [SerializeField] private float _baseWaveBreakDuration;
    [SerializeField] private float _currentWaveBreakDuration;
#endregion

    [SerializeField] private bool _addedSpawnCorrectly;
    [SerializeField] private bool _levelFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentWaveIndex = 0;
        _currentWaveSpawns = new Dictionary<int, GameObject[]>();
        LoadNextWave();
    }

    // We may want to add in a way to determine if all the enemies in a wave have died
    // This way we can disable card draw and stuff only when no enemies are present
    private void FixedUpdate()
    {
        if(!_levelFinished)
        {
            _isInWaveValueDebug = _isInWave.Value;
            //TODO:: Figure when to reset these current timer values
            if(_isInWave.Value)
            {
                if(_currentWaveDuration <= 0)
                {
                    //Wave is over
                    LoadNextWave();
                }
                else
                {
                    SpawnEnemies();
                    _currentWaveDuration -= Time.deltaTime;
                }
            }
            else
            {
                if(_currentWaveBreakDuration <= 0)
                {
                    //Break is over
                    StartWave();
                }
                else
                {
                    _currentWaveBreakDuration -= Time.deltaTime;
                }
            }
        }
    }

    public void StartWave()
    {
        //Add ink equal to the time left in the countdown
        _isInWave.Value = true;
    }

    public void LoadNextWave()
    {
        //Clear last wave information
        _currentWaveSpawns.Clear();

        if(_currentWaveIndex < _waves.Count)
        {
            //Load next wave information
            _currentWave = _waves[_currentWaveIndex];
            foreach(WaveScriptableObject.EnemySpawn spawn in _currentWave.Wave)
            {
                _addedSpawnCorrectly = _currentWaveSpawns.TryAdd(spawn._timeToSpawn, spawn._enemies);
                if(_addedSpawnCorrectly != true)
                {
                    Debug.LogError("Enemies are already being spawned at this time: " + spawn._timeToSpawn);
                }
            }

            _baseWaveBreakDuration = _currentWave.WaveBreakBeforeDuration;
            _baseWaveDuration = _currentWave.WaveDuration;
            _currentWaveBreakDuration = _baseWaveBreakDuration;
            _currentWaveDuration = _baseWaveDuration;
            _isInWave.Value = false;
            _currentWaveIndex += 1;
        }
        else
        {
            FinishLevel();
        }
    }

    private void SpawnEnemies()
    {
        _currentWaveTime = Mathf.FloorToInt(_baseWaveDuration - _currentWaveDuration);
        if(_previousWaveTime != _currentWaveTime)    
        {
            _previousWaveTime = _currentWaveTime;
            if(_currentWaveSpawns.ContainsKey(_currentWaveTime))
            {
                _currentEnemySpawns = _currentWaveSpawns[_currentWaveTime];
                for(int i = 0; i < 3; i++) //We always know the size of the spawns will be 3
                {
                    if(_currentEnemySpawns[i] != null)
                    {
                        GameObject spawnedEnemy = Instantiate(_currentEnemySpawns[i], _spawnLocations[i].position, Quaternion.identity);
                        BoardManager.Instance.GetLane(i).AddEnemyToList(spawnedEnemy);
                    }
                } 
            }
        }
#region Backup Code
/*
        for (int i = 0; i < _waves[_currentWaveIndex].Wave.Count; i++)
        {
            if (_waves[_currentWaveIndex].Wave[i]._timeToSpawn < (_baseWaveDuration - _currentWaveDuration))
            {
                if (_waves[_currentWaveIndex].Wave[i]._isEnemySummoned)
                {

                }
                else if (!_waves[_currentWaveIndex].Wave[i]._isEnemySummoned)
                {    
                    BoardLane lane = BoardManager.Instance.GetLane(_waves[_currentWaveIndex].Wave[i]._laneNum);
                    //Add enemy to the _laneEnemies list
                    foreach (GameObject g in _waves[_currentWaveIndex].Wave[i]._enemies)
                    {
                        Instantiate(g, _summonLocations[_waves[_currentWaveIndex].Wave[i]._laneNum].position, Quaternion.identity);
                    }
                    Debug.Log("Enemy summoned!");
                    _waves[_currentWaveIndex].Wave[i]._isEnemySummoned = true;
                }
                        
            }
        }
*/
#endregion
    }

    private void FinishLevel()
    {
        _levelFinished = true;
        Debug.Log("YOU WIN SMILE");
    }
}

