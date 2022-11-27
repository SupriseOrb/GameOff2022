using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    // TODO: Implement Tile info panel
    [Header("Info Panel")]
    [SerializeField] private GameObject _tileInfoPanel;
    [SerializeField] private TextMeshProUGUI _tileInfoPanelText;
    [SerializeField] private int _tileInfoLaneNum;
    [SerializeField] private int _tileInfoTileNum;


    [Header("Misc")]
    [SerializeField] private BoardLane[] _boardLanes;

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
        for (int i = 0; i < _boardLanes.Length; i++)
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
        if (other.TryGetComponent(out IEnemy enemy))
        {
            _playerHealth -= enemy.GetPlayerHealthDamage();
            enemy.TakeDamage(1000f);
            if (_playerHealth < 0)
            {
                Debug.Log("Game Over bro, Game Over");
            }
        }
    }

    /*
        Toggle the Info Panel based on a few scenarios
        Scenario 1) Info Panel is Opened + Click on Same Tile = Close Info Panel
        Scenario 2) Click on different tile that has either a land or item = Open Panel
        Scenario 3) Click on different tile that does not have a land or item = Close Panel
        Scenario 4) Click on different tile that has a unit = Open Panel 
    */
    public void ToggleTileInfo(int laneNum, int tileNum, string TileInfo, bool isBoardTile = false)
    {
        if (laneNum == _tileInfoLaneNum && tileNum == _tileInfoTileNum)
        {
            // Scenario 1
            HideTileInfo();
        }
        else
        {
            _tileInfoLaneNum = laneNum;
            _tileInfoTileNum = tileNum;
            if (isBoardTile)
            {
                string tempText = TileInfo;
                GameObject landStamp = GetLane(_tileInfoLaneNum).CurrentLandStamp;
                if (landStamp != null)
                {
                    if (tempText != "")
                    {
                        tempText += "\n";
                    }

                    tempText += landStamp.GetComponent<ILandStamp>().GetTileDescription();                    
                }

                // Scenario 2
                if (tempText != "")
                {
                    ShowTileInfo(tempText);
                }
                // Scenario 3
                else
                {
                    HideTileInfo();
                }
            }
            else
            {
                // Scenario 4
                ShowTileInfo(TileInfo);
            }
            
        }
    }
    
    private void ShowTileInfo(string TileInfo)
    {        
        _tileInfoPanelText.text = TileInfo;
        _tileInfoPanel.SetActive(true);
    }

    public void HideTileInfo()
    {
        _tileInfoLaneNum = _tileInfoTileNum = -1;
        _tileInfoPanel.SetActive(false);

    }

    //Clears all land stamps from the boardlanes
    public void ResetBoardState()
    {
        foreach(BoardLane lane in _boardLanes)
        {
            lane.RemoveLandStamp();
        }
    }
}
