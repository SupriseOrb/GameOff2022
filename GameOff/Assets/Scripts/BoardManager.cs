using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] (int, int) boardDimensions;
    [SerializeField] BoardLane[] boardLane;

/*    public enum upgradeAmount
    {
        Upgradebase = 0,
        path1 = 1,
        path2 = 2,
        parh3 = 3
    }
    public upgradeAmount currUpgrade = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateStampAbility()
    {
        //maybe use switch case
        if(currUpgrade == 0)
        {
            //do basic stuff
        }
        else if(currUpgrade == upgradeAmount.path1)
        {
            //call upgrade 1 helper function
        }
    }

    public void DisableAbility()
    {

    }

    public void EnableAbility()
    {

    }*/
}
