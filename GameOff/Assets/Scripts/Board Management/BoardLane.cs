using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLane : MonoBehaviour
{
    [SerializeField] private LandStampScriptableObject _currentLaneStamp = null;
    [SerializeField] private Sprite _currentLaneSprite;
    [SerializeField] private List<GameObject> _laneEnemies;
    [SerializeField] private BoardTile[] _laneTiles;
    [SerializeField] private GameObject[] _laneUnits;
    [SerializeField] public int _laneNumber;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called when user places down an lane stamp
    public void ApplyLaneStamp(LandStampScriptableObject laneStamp)
    {
        //kill all enemies in the lane
        _currentLaneStamp = laneStamp;
        _currentLaneSprite = _currentLaneStamp.StampSprite;
        foreach(GameObject enemy in _laneEnemies)
        {
            enemy.GetComponent<IEnemy>().TakeDamage(1000);
        }
        _laneEnemies.Clear();
    }

    //called at the start of a wave
    public void RemoveLaneStamp()
    {
        _currentLaneStamp = null;
        _currentLaneSprite = null;
    }

    //Needs to be called when an enemy dies
    public void RemoveEnemyFromList(GameObject enemy)
    {
        _laneEnemies.Remove(enemy);
    }

    //Tells each of its tiles what lane and tile number they are
    public void SetTileLanes()
    {
        for(int i = 0; i < _laneTiles.Length; i++)
        {
            _laneTiles[i]._laneNumber = _laneNumber;
            _laneTiles[i]._tileNumber = i;
        }
    }

    //its here in case we need it, not sure we do tho
    // It might be needed by certain lane stamps
    public BoardTile[] GetLaneTiles()
    {
        return _laneTiles;
    }

    //Used by certain lane stamps
    public List<GameObject> GetLaneEnemies()
    {
        return _laneEnemies;
    }

    //Keeping track of player units inside of a lane just in case we need to access them    
    public void SetLaneUnit(GameObject unit, int tileNumber)
    {
        _laneUnits[tileNumber] = unit;
    }

    //Gets the rightmost tile that does not have a held stamp inside of it
    //Used by the Ink Demon
    public BoardTile GetFurthestFreeTile(int lane)
    {
        for(int i = _laneTiles.Length - 1; i >= 0; i--)
        {
            if(_laneTiles[i].GetHeldStamp() == null)
            {
                return _laneTiles[i]; 
            }
        }
        return null;
    }
}
