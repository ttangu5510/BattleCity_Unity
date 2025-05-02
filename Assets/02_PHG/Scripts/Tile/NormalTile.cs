public class NormalTile : TileEnviorment
{


    protected override void SendTileType(IMovable movable)
    {
        movable.moveType = MoveType.normal;
    }
}
