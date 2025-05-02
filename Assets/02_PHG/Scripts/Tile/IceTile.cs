public class IceTile : TileEnviorment
{

    protected override void SendTileType(IMovable movable)
    {
        movable.moveType = MoveType.iceSlide;
    }
}
