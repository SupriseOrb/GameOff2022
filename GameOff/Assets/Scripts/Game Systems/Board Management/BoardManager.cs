using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] BoardLane[] _boardLanes;

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
}
