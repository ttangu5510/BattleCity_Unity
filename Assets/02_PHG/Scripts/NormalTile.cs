using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileEnviorment
{


    protected override void SendTileType(IMovable movable)
    {
        Debug.Log("¶¥¹Ù´Ú¿¡ ÂøÁã");
        movable.moveType = MoveType.normal;
    }
}
