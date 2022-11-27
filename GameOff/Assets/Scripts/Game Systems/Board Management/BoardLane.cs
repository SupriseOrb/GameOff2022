using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLane : MonoBehaviour
{
    [SerializeField] private GameObject _currentLandStamp = null;
    [SerializeField] private Vector3 _landStampSpawnPosition;
    [SerializeField] private List<GameObject> _laneEnemies;
    [SerializeField] private BoardTile[] _laneTiles;
    [SerializeField] private GameObject[] _laneUnits;
    [SerializeField] private BoolVariable _leylineActive;
    [SerializeField] private float _leylineMultiplier;
    [SerializeField] public int _laneNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        _landStampSpawnPosition = new Vector3(gameObject.transform.position.x + 14, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called when user places down an lane stamp
    public void ApplyLandStamp(GameObject landStamp)
    {
        if(_currentLandStamp != null)
        {
            RemoveLandStamp();
        }
        //kill all enemies in the lane
        _currentLandStamp = Instantiate(landStamp, _landStampSpawnPosition, Quaternion.identity, gameObject.transform);
        _currentLandStamp.GetComponent<ILandStamp>().SetLane(_laneNumber);
        //_currentLaneSprite = how to get the SO info?
        for(int i = _laneEnemies.Count - 1; i >= 0; i--)
        {
            _laneEnemies[i].GetComponent<IEnemy>().TakeDamage(1000);
        }
        _laneEnemies.Clear();
    }

    //called at the start of a wave
    public void RemoveLandStamp()
    {
        if (_currentLandStamp != null)
        {
            Destroy(_currentLandStamp);
        }
    }

    public void AddEnemyToList(GameObject enemy)
    {
        _laneEnemies.Add(enemy);
        enemy.GetComponent<IEnemy>().SetLane(_laneNumber);
    }

    //Needs to be called when an enemy dies
    public void RemoveEnemyFromList(GameObject enemy)
    {
        _laneEnemies.Remove(enemy);
    }

    //Tells each of its tiles what lane and tile number they are
    public void SetTileIndexValues()
    {
        for(int i = 0; i < _laneTiles.Length; i++)
        {
            _laneTiles[i].LaneNumber = _laneNumber;
            _laneTiles[i].TileNumber = i;
        }
    }

    //Used by certain lane stamps
    public List<GameObject> GetLaneEnemies()
    {
        return _laneEnemies;
    }

    public List<GameObject> GetLaneItems()
    {
        List<GameObject> laneItems = new List<GameObject>();
        for(int i = 2; i < _laneTiles.Length; i++)
        {
            if(_laneTiles[i].GetHeldStamp() != null)
            {
                laneItems.Add(_laneTiles[i].GetHeldStamp());
            }
        }
        return laneItems;
    }

    //Keeping track of player units inside of a lane just in case we need to access them    
    public void SetLaneUnit(GameObject unit, int tileNumber)
    {
        _laneUnits[tileNumber] = unit;
    }

    public GameObject CurrentLandStamp
    {
        get {return _currentLandStamp;}
    }
    
    public GameObject[] GetLaneUnits()
    {
        return _laneUnits;
    }

    //Gets the rightmost tile that does not have a held stamp inside of it
    //Used by the Ink Demon
    public BoardTile GetNearestFreeTile()
    {
        for(int i = 2; i < _laneTiles.Length; i++)
        {
            if(_laneTiles[i].GetHeldStamp() == null)
            {
                return _laneTiles[i]; 
            }
        }
        return null;
    }

    public bool GetLeylineStatus()
    {
        return _leylineActive.Value;
    }

    public void SetLeylineStatus(bool status)
    {
        _leylineActive.Value = status;
    }

    public float GetLeylineMultiplier()
    {
        return _leylineMultiplier;
    }

    public void SetLeylineMultiplier(float multiplier)
    {
        _leylineMultiplier = multiplier;
    }    
}
