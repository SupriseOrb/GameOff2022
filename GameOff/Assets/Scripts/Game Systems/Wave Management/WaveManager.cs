using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveScriptableObject> _waves;
    [SerializeField] private bool _isFirstWave = true;
    [SerializeField] private BoolVariable _isInWave;

    [SerializeField] private bool _finishedSpawningWave;
    public bool FinishedSpawningWave
    {
        get {return _finishedSpawningWave;}
    }

    // TODO : Delete this variable
    [SerializeField] private bool _levelFinished = false;
    [SerializeField] private Transform[] _spawnLocations;
    private Dictionary<int, GameObject[][]> _currentWaveSpawns;
    
    [Tooltip("How far away from each other the enemies will be spawned")]
    [Range(0f, .5f)]
    [SerializeField] private float _enemySpawnDistance = .2f;
    
    [Header("Debug Variables")]
    [SerializeField] private int _currentWaveIndex;
    [SerializeField] private WaveScriptableObject _currentWave;
    [SerializeField] private GameObject[] _currentEnemySpawns;
    [SerializeField] private bool _addedSpawnCorrectly;

    #region Transitions
    [SerializeField] private Canvas _startWaveCanvas;
    [SerializeField] private TextMeshProUGUI _startWaveButtonText;
    [SerializeField] private TextMeshProUGUI _bonusInk;
    [SerializeField] private Canvas _winPanel;
    [SerializeField] private Canvas _losePanel;
    private bool _gameOver = false;
    #endregion

    #region Intensities
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
    #endregion
    
#region Wave Timer Variables
    // TODO : Rename these 2 variables to something that is a bit more clear
    [SerializeField] private int _currentWaveTime;
    [SerializeField] private int _previousWaveTime;

    [SerializeField] private float _baseWaveBreakDuration;
    [SerializeField] private float _currentWaveBreakDuration;

    [SerializeField] private float _baseWaveDuration;
    [SerializeField] private float _currentWaveDuration;
#endregion

    void Awake()
    {
        _isInWave.Reset();
        _finishedSpawningWave = false;
    }

    void Start()
    {
        _currentWaveIndex = 0;
        _currentWaveSpawns = new Dictionary<int, GameObject[][]>();

        _lastIntensitySet = 0;
        BroadcastIntensityChange();

        LoadNextWave();
        _startWaveCanvas.enabled = true;
    }

    // Used to broadcast intensity changes to the music, ambience, and art
    private void BroadcastIntensityChange()
    {
        if (_lastIntensitySet == _currentIntensity)
        {
            return;
        }

        _lastIntensitySet = _currentIntensity;
        AkSoundEngine.SetState("Battle_Intensity", "Battle_Intensity_" + _currentIntensity);
        AkSoundEngine.SetState("Ambience_states", "Ambience_" + _currentIntensity);
        _environmentManager.ChangeIntensity(_currentIntensity);
    }
    public void LoadNextWave()
    {
        _currentWaveSpawns.Clear();

        // TODO : Don't check if the game is over here, do it in Finish Wave
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
            _currentWaveBreakDuration = _baseWaveBreakDuration;
            _currentWaveIndex += 1;

            _baseWaveDuration = _currentWave.WaveDuration;
            _currentWaveDuration = _baseWaveDuration;
        }
        else
        {
            WinGame();
        }
    }
    private void WinGame()
    {
        _winPanel.enabled = true;
        AkSoundEngine.SetState("Music_State", "Win");
        
        FinishGame();
    }

    private void FinishGame()
    {
        _gameOver = true;

        _startWaveCanvas.enabled = false;

        _levelFinished = true;
        _isInWave.Value = false;
        // TODO : Figure out if this call is needed _isWaveComplete.Value = true;
    }

    /*
        When the game isn't over, there are a few situations
        (1) If we're in a wave and we haven't finished spawning enemies
            (a) Spawn the last enemies
            (b) Spawn enemies
        (2) If we are not in the wave (e.g. in the break period)
    */
    private void FixedUpdate()
    {
        if (!_levelFinished && !_gameOver)
        {
            if (_isInWave.Value && !_finishedSpawningWave)
            {
                // 1a
                if (_currentWaveDuration <= 0)
                {
                    SpawnEnemies();
                    _finishedSpawningWave = true;
                }
                // 1b
                else
                {
                    SpawnEnemies();
                    _currentWaveDuration -= Time.deltaTime;
                }
            }
            // 2
            else
            {
                if (_currentWaveBreakDuration <= 0)
                {
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
        _finishedSpawningWave = false;

        if (_currentWaveBreakDuration > 0 && !_isFirstWave)
        {
            DeckManager.Instance.AddInk((int)_currentWaveBreakDuration);
            AkSoundEngine.PostEvent("Play_InkBonus", gameObject);
        }

        AkSoundEngine.PostEvent("Play_WaveStart", gameObject);
        BroadcastIntensityChange();

        _startWaveCanvas.enabled = false;
        
        _isFirstWave = false;
        DeckManager.Instance.IsFirstDraw = false;
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
    }

   
    public void FinishWave()
    {
        _isInWave.Value = false;
        _startWaveCanvas.enabled = true;

        /*
            Check if game is over with _currentWaveIndex < _waves.Count
            If it is over, call WinGame()
            If it is not over, call
                LoadNextWave()
                ResetDeck()
                ResetBoardState()
                Setup the startwavecanvas (maybe create another function for this)
        */
        LoadNextWave();
        
        DeckManager.Instance.ResetDeck();
        BoardManager.Instance.ResetBoardState();
        
        _startWaveButtonText.text = "Start Wave " + _currentWaveIndex;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.TryGetComponent(out IEnemy enemy) && !_gameOver)
        {
            LoseGame();            
        }
    }

    private void LoseGame()
    {
        _losePanel.enabled = true;
        AkSoundEngine.SetState("Music_State", "Lose");
        FinishGame();
    }
}

