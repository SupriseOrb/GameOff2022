using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    // TODO: Implement Tile info panel
    [SerializeField] GameObject _tileInfoPanel;
    [SerializeField] TextMeshProUGUI _tileInfoPanelText;
    [SerializeField] BoardLane[] _boardLanes;

    [SerializeField] private int _playerHealth;

    private static BoardManager _instance;

    public static BoardManager Instance
    {
        get{return _instance;}
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start() 
    {
        //Tells each lane what lane number they
        //Each lane then tells each of their tiles their lane and tile number
        for(int i = 0; i < _boardLanes.Length; i++)
        {
            _boardLanes[i]._laneNumber = i;
            _boardLanes[i].SetTileIndexValues();
        }
    }

    //Its kinda wild this returns the boardlane instead of the boardlanes array
    //Not sure we want things to access all lanes however so idk 
    public BoardLane GetLane(int laneNumber)
    {
        return _boardLanes[laneNumber];
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent(out IEnemy enemy))
        {
            _playerHealth -= enemy.GetPlayerHealthDamage();
            enemy.TakeDamage(1000f);
            if(_playerHealth < 0)
            {
                Debug.Log("Game Over bro, Game Over");
            }
        }
    }
}
