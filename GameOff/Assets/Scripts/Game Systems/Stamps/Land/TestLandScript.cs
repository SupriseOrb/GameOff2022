using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLandScript : MonoBehaviour, ILandStamp
{
    [SerializeField] private LandStampScriptableObject _landSO;

    public void ActivateStampAbility()
    {
        //I'll need to update this audio post event later - Collin
        AkSoundEngine.PostEvent("Play_BramblePatch", gameObject);
    }

    public void SetLane(int laneNumber)
    {

    }

    public void DisableStamp()
    {

    }

    public void EnableStamp()
    {

    }
}
