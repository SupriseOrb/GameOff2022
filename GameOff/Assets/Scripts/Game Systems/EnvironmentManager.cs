using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] GameObject[] _lighting;
    [SerializeField] private int _currentIntensity = 0;

    [SerializeField] Material[] _treeMaterials;
    [SerializeField] Material[] _oceanMaterials;
    [SerializeField] SpriteRenderer[] _treeBGSR;
    [SerializeField] SpriteRenderer[] _treeFGSR;
    [SerializeField] SpriteRenderer _oceanSR;
    
    /* 
        Update the intensities for
            1) Lighting
            2) Ocean
            3) Trees
    */

    public void ChangeIntensity(int newIntensity)
    {
        // Note: New intensity is 1-index while _currentIntensity is 0 index
        if ((newIntensity - 1) == _currentIntensity)
        {
            return;
        }

        _currentIntensity = newIntensity - 1;
        
        // 1) Lighting
        for (int i = 0; i < _lighting.Length; i++)
        {
            bool isActive = i == (_currentIntensity);
            _lighting[i].SetActive(isActive);
        }

        // 2) Ocean
        _oceanSR.material = _oceanMaterials[_currentIntensity];

        // 3) Trees
        for (int i = 0; i < _treeBGSR.Length; i++)
        {
            _treeBGSR[i].material = _treeMaterials[_currentIntensity];
            _treeFGSR[i].material = _treeMaterials[_currentIntensity];
        }
    }
}
