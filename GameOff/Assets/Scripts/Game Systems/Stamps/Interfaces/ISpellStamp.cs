using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellStamp : IStamp
{
    // TODO : Move this to a base spell stamp class because functionality is the same
    // Consider if we even need this
    void SetTile(BoardTile tile);
}
