using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLeylineLand : MonoBehaviour, ILandStamp
{
    [SerializeField] private LandStampScriptableObject _leylineSO;
    
    [Header("Debug Values")]
    [SerializeField] private float _leylineMultiplier;
    [SerializeField] private int _laneNumber;

    // Start is called before the first frame update
    void Start()
    {
        LoadBaseStats();
        ActivateStampAbility();
    }

    private void LoadBaseStats()
    {
        _leylineMultiplier = _leylineSO.LandAbilityValue;
    }

    public void ActivateStampAbility()
    {
        BoardManager.Instance.GetLane(_laneNumber).SetLeylineStatus(true);
        BoardManager.Instance.GetLane(_laneNumber).SetLeylineMultiplier(_leylineMultiplier);
    }

    public void SetLane(int laneNumber)
    {
        _laneNumber = laneNumber;
    }

    public string GetStampName()
    {
        return _leylineSO.StampName;
    }

    private void OnDestroy() 
    {
        BoardManager.Instance.GetLane(_laneNumber).SetLeylineStatus(false);
    }

    public void EnableStamp()
    {

    }

    public void DisableStamp()
    {

    }
}
