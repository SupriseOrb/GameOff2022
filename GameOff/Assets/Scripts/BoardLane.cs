using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLane : MonoBehaviour
{
    [SerializeField] LaneStampScriptableObject currentLaneStamp = null;
    [SerializeField] Sprite currentLaneSprite;
    [SerializeField] List<GameObject> laneEnemies;
    [SerializeField] IStamp[] laneUnits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyLaneStamp(LaneStampScriptableObject laneStamp)
    {
        //kill all enemies in the lane
        currentLaneStamp = laneStamp;
        currentLaneSprite = currentLaneStamp.StampImage;
    }

    //called at the start of a wave
    public void RemoveLaneStamp()
    {
        currentLaneStamp = null;
    }
}
