using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileEnviorment
{


    protected override void SendTileType(IMovable movable)
    {
        Debug.Log("���ٴڿ� ����");
        movable.moveType = MoveType.normal;
    }
}
