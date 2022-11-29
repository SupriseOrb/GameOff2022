using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialPiece", menuName = "GameOff2022/Tutorial/TutorialPiece", order = 0)]
public class TutorialPiece : ScriptableObject
{
    [SerializeField] private Sprite _image;
    [SerializeField] [TextArea] string _description;

    public Sprite Image
    {
        get {return _image;}
    }

    public string Description
    {
        get {return _description;}
    }

}
