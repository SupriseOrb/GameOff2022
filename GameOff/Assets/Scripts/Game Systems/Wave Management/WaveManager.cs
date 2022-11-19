using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveScriptableObject> _waves;
    [SerializeField] private List<int> _spawnTimes;
    [SerializeField] private float _baseWaveTimer;
    [SerializeField] private float _currentWaveTimer;
    [SerializeField] private BoolVariable _isInWave;
    [SerializeField] private Transform[] _summonLocations;
    [SerializeField] private int _currentWave;

    // Start is called before the first frame update
    void Start()
    {
        _currentWaveTimer = _baseWaveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        InWave();
    }

    private void InWave()
    {
        if (_isInWave.Value)
        {
            if (_currentWaveTimer > 0)
            {
                for (int i = 0; i < _waves[_currentWave].Wave.Count; i++)
                {
                    if (_waves[_currentWave].Wave[i]._timeToSpawn < (_baseWaveTimer - _currentWaveTimer))
                    {
                        if (_waves[_currentWave].Wave[i]._isEnemySummoned)
                        {

                        }
                        else if (!_waves[_currentWave].Wave[i]._isEnemySummoned)
                        {    
                            BoardLane lane = BoardManager.Instance.GetLane(_waves[_currentWave].Wave[i]._laneNum);
                            //Add enemy to the _laneEnemies list
                            foreach (GameObject g in _waves[_currentWave].Wave[i]._enemies)
                            {
                                Instantiate(g, _summonLocations[_waves[_currentWave].Wave[i]._laneNum].position, Quaternion.identity);
                            }
                            Debug.Log("Enemy summoned!");
                            _waves[_currentWave].Wave[i]._isEnemySummoned = true;
                        }
                                
                    }
                }
            }
            _currentWaveTimer -= Time.deltaTime;
        }
        else
        {
            _isInWave.Value = false;
            //Start timer for between waves
        }
    }

    //Called by the button the player can press to start the wave early and skip the timer
    private void StartWave()
    {
        //Based on how long the current timer in between waves is, give the player x amt of ink
        //Set the current timer in between waves to 0
        _isInWave.Value = true;
    }
}

