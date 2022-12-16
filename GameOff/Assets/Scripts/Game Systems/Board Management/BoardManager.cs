using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private WaveManager _waveManager;
    private static BoardManager _instance;
    [SerializeField] private BoardLane[] _boardLanes;    
 
    #region UI
    [SerializeField] private BoolVariable _isHoveringUI;

    [Header("Info Panel")]
    [SerializeField] private GameObject _tileInfoPanel;
    [SerializeField] private TextMeshProUGUI _tileInfoPanelText;
    [SerializeField] private int _tileInfoLaneNum;
    [SerializeField] private int _tileInfoTileNum;
    #endregion
    
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
        for (int i = 0; i < _boardLanes.Length; i++)
        {
            _boardLanes[i].SetValues(i);
        }
    }

    public BoardLane GetLane(int laneNumber)
    {
        return _boardLanes[laneNumber];
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
        if (_isHoveringUI.Value)
        {
            return;
        }
        
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
                        tempText += "\n\n";
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
        AkSoundEngine.PostEvent("Play_UISelect", gameObject);
        _tileInfoPanelText.text = TileInfo;
        _tileInfoPanel.SetActive(true);
    }

    public void HideTileInfo()
    {
        if (_tileInfoPanel.activeSelf)
        {
            AkSoundEngine.PostEvent("Play_UIBack", gameObject);
            _tileInfoLaneNum = _tileInfoTileNum = -1;
            _tileInfoPanel.SetActive(false);
        }
    }

    public void ResetBoardState()
    {
        foreach(BoardLane lane in _boardLanes)
        {
            lane.RemoveLandStamp();
        }
    }

    public void CheckIfWaveIsFinished()
    {
        if(_waveManager.FinishedSpawningWave)
        {
            foreach(BoardLane lane in _boardLanes)
            {
                // TODO : Check if this is comprehensive (e.g. take care of Carriages)
                if (lane.HasEnemies)
                {
                    return;
                }
            }
            _waveManager.FinishWave();
        }
    }
}
