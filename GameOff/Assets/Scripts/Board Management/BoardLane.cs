using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLane : MonoBehaviour
{
    [SerializeField] LandStampScriptableObject currentLaneStamp = null;
    [SerializeField] Sprite currentLaneSprite;
    [SerializeField] List<GameObject> laneEnemies;
    [SerializeField] IStamp[] laneUnits;
    [SerializeField] BoardTile[] laneTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyLaneStamp(LandStampScriptableObject laneStamp)
    {
        //kill all enemies in the lane
        currentLaneStamp = laneStamp;
        currentLaneSprite = currentLaneStamp.StampSprite;
    }

    //called at the start of a wave
    public void RemoveLaneStamp()
    {
        currentLaneStamp = null;
    }
}
