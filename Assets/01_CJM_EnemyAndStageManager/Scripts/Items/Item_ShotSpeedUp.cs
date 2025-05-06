using UnityEngine;


namespace CJM
{
    public class Item_ShotSpeedUp : Item
    {
        [SerializeField] float shotSpeedReinforce;
        public override void SetEffect()
        {
            effect = (player) => PlayerManager.Instance.SpeedControl(0, shotSpeedReinforce);
        }
    }
}
