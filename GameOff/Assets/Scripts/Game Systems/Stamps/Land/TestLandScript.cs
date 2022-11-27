using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLandScript : MonoBehaviour, ILandStamp
{
    [SerializeField] private LandStampScriptableObject _landSO;
    [SerializeField] private CardScriptableObject _cardSO;

    public void ActivateStampAbility()
    {
        //I'll need to update this audio post event later - Collin
        AkSoundEngine.PostEvent("Play_BramblePatch", gameObject);
    }

    public void SetLane(int laneNumber)
    {

    }

    public string GetStampName()
    {
        return "";
    }

    public void DisableStamp()
    {

    }

    public void EnableStamp()
    {

    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.LAND, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescription;
        return name + "\n" + description;
    }
}
