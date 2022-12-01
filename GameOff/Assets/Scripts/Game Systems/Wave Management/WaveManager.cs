using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveScriptableObject> _waves;
    [SerializeField] private Transform[] _spawnLocations;
    [SerializeField] private BoolVariable _isInWave;
    private Dictionary<int, GameObject[][]> _currentWaveSpawns;
    [SerializeField] private Canvas _winPanel;
    [SerializeField] private Canvas _losePanel;
    [SerializeField] private TextMeshProUGUI _startWaveButtonText;
    private bool _gameOver = false;

    private const int MAXINTENSITYLEVEL = 3;
    private int _lastIntensitySet;
    private int _currentIntensity
    {
        get
        {
            for (int multiplier = 1; multiplier <= MAXINTENSITYLEVEL; multiplier++)
            {
                if (_currentWaveIndex <= (Mathf.RoundToInt(_waves.Count/(float)MAXINTENSITYLEVEL)*multiplier))
                {
                    return multiplier;
                }
            }
            return MAXINTENSITYLEVEL;
        }
    }
    [SerializeField] private EnvironmentManager _environmentManager;

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
    [Tooltip("How far away from each other the enemies will be spawned")]
    [Range(0f, .5f)]
    [SerializeField] private float _enemySpawnDistance = .2f;

    [SerializeField] private bool _addedSpawnCorrectly;
    [SerializeField] private bool _levelFinished = false;

    [SerializeField] private Canvas _startWaveCanvas;
    [SerializeField] private TextMeshProUGUI _bonusInk;
    [SerializeField] private bool _isFirstWave = true;
    [SerializeField] private BoolVariable _isWaveComplete;

    // Start is called before the first frame update
    void Start()
    {
        _currentWaveIndex = 0;
        _currentWaveSpawns = new Dictionary<int, GameObject[][]>();
        LoadNextWave();
        _startWaveCanvas.enabled = true;
        //Clear last wave information
        _lastIntensitySet = 0;
        BroadcastIntensityChange();
    }

    // We may want to add in a way to determine if all the enemies in a wave have died
    // This way we can disable card draw and stuff only when no enemies are present
    private void FixedUpdate()
    {
        if (!_levelFinished && !_gameOver)
        {
            _isInWaveValueDebug = _isInWave.Value;
            // TODO : Figure when to reset these current timer values
            if (_isInWave.Value)
            {
                if(!_isWaveComplete.Value)
                {
                    if (_currentWaveDuration <= 0)
                    {
                        SpawnEnemies();
                        //FinishWave();
                        _isWaveComplete.Value = true;
                    }
                    else
                    {
                        SpawnEnemies();
                        _currentWaveDuration -= Time.deltaTime;
                    }
                }
            }
            else
            {
                if (_currentWaveBreakDuration <= 0)
                {
                    // TODO : Do we want players to choose when they start regardless of the _currentWaveBreakDuration?
                    //Break is over
                    StartWave();
                }
                else
                {
                    if(!_isFirstWave)
                    {
                        _bonusInk.text = "Get +" + Mathf.Min((int)_currentWaveBreakDuration, 50);
                        _currentWaveBreakDuration -= Time.deltaTime;
                    }
                    else
                    {
                        _bonusInk.text = "Get +0";
                    }
                }
            }
        }
    }

    public void StartWave()
    {
        _isInWave.Value = true;
        _isWaveComplete.Value = false;
        //Add ink equal to the time left in the countdown
        if (_currentWaveBreakDuration > 0)
        {
            
            if(!_isFirstWave)
            {
                DeckManager.Instance.AddInk((int)_currentWaveBreakDuration);
                AkSoundEngine.PostEvent("Play_InkBonus", gameObject);
            }
        }
        AkSoundEngine.PostEvent("Play_WaveStart", gameObject);
        BroadcastIntensityChange();
        _startWaveCanvas.enabled = false;
        _isFirstWave = false;
        DeckManager.Instance.IsFirstDraw = false;
    }

    public void LoadNextWave()
    {
        _currentWaveSpawns.Clear();

        if (_currentWaveIndex < _waves.Count)
        {
            //Load next wave information
            _currentWave = _waves[_currentWaveIndex];
            foreach (WaveScriptableObject.EnemySpawn spawn in _currentWave.Wave)
            {
                GameObject[][] enemies = new GameObject[3][];
                enemies[0] = spawn._laneOneEnemies;
                enemies[1] = spawn._laneTwoEnemies;
                enemies[2] = spawn._laneThreeEnemies;
                _addedSpawnCorrectly = _currentWaveSpawns.TryAdd(spawn._timeToSpawn, enemies);
                if (_addedSpawnCorrectly != true)
                {
                    Debug.LogError("Enemies are already being spawned at this time: " + spawn._timeToSpawn);
                }
            }

            _baseWaveBreakDuration = _currentWave.WaveBreakBeforeDuration;
            _baseWaveDuration = _currentWave.WaveDuration;
            _currentWaveBreakDuration = _baseWaveBreakDuration;
            _currentWaveDuration = _baseWaveDuration;
            _currentWaveIndex += 1;
        }
        else
        {
            WinGame();
        }
    }

    private void SpawnEnemies()
    {
        _currentWaveTime = Mathf.FloorToInt(_baseWaveDuration - _currentWaveDuration);
        if (_previousWaveTime != _currentWaveTime)    
        {
            _previousWaveTime = _currentWaveTime;
            if (_currentWaveSpawns.ContainsKey(_currentWaveTime))
            {
                for(int spawnLaneNumber = 0; spawnLaneNumber < 3; spawnLaneNumber++) //We always know the size of the lanes will be 3
                {
                    _currentEnemySpawns = _currentWaveSpawns[_currentWaveTime][spawnLaneNumber]; //Get all the enemies to be spawned in this lane
                    for (int enemyIndex = 0; enemyIndex < _currentEnemySpawns.Length; enemyIndex++) 
                    {
                        if (_currentEnemySpawns[enemyIndex] != null)
                        {
                            Vector3 currentSpawnLocation = new Vector3(_spawnLocations[spawnLaneNumber].position.x + (enemyIndex * _enemySpawnDistance), _spawnLocations[spawnLaneNumber].position.y, _spawnLocations[spawnLaneNumber].position.z);
                            GameObject spawnedEnemy = Instantiate(_currentEnemySpawns[enemyIndex], currentSpawnLocation, Quaternion.identity);
                            BoardManager.Instance.GetLane(spawnLaneNumber).AddEnemyToList(spawnedEnemy);
                        }
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

    private void WinGame()
    {
        _winPanel.enabled = true;
        AkSoundEngine.SetState("Music_State", "Win");
        _startWaveCanvas.enabled = false;
        FinishGame();
    }
    public void FinishWave()
    {
        _isInWave.Value = false;
        _startWaveCanvas.enabled = true;
        LoadNextWave();
        DeckManager.Instance.ResetDeck();
        BoardManager.Instance.ResetBoardState();
        _startWaveButtonText.text = "Start Wave " + _currentWaveIndex;
    }

    private void FinishGame()
    {
        _levelFinished = true;
        _isInWave.Value = false;
        _gameOver = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.TryGetComponent(out IEnemy enemy) && !_gameOver)
        {
            
            /*_playerHealth -= enemy.GetPlayerHealthDamage();
            enemy.TakeDamage(1000f);
            if (_playerHealth < 0)
            {
                Debug.Log("Game Over bro, Game Over");
            }*/

            _losePanel.enabled = true;
            AkSoundEngine.SetState("Music_State", "Lose");
            FinishGame();
        }
    }

    // Used to broadcast intensity changes to the music, ambience, and art
    private void BroadcastIntensityChange()
    {
        if (_lastIntensitySet == _currentIntensity)
        {
            return;
        }
        Debug.Log("Set Intensity");
        _lastIntensitySet = _currentIntensity;
        AkSoundEngine.SetState("Battle_Intensity", "Battle_Intensity_" + _currentIntensity);
        AkSoundEngine.SetState("Ambience_states", "Ambience_" + _currentIntensity);
        _environmentManager.ChangeIntensity(_currentIntensity);
    }
}

