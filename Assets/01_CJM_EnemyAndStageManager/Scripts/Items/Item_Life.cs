namespace CJM
{
    public class Item_Life : Item
    {
        public override void SetEffect()
        {
            effect = (player) => PlayerManager.Instance.CalculateLife(+1); ;
        }
    }
}