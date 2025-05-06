using UnityEngine;


namespace CJM
{
    public class Item_SpeedUp : Item
    {
        [SerializeField] float moveSpeedReinforce;
        public override void SetEffect()
        {
            effect = (player) => PlayerManager.Instance.SpeedControl(moveSpeedReinforce, 0);
        }
    }
}