using System.Collections;
using UnityEngine;


namespace CJM
{
    public class Item_Invincible : Item
    {
        public float duration; //지속시간

        public override void SetEffect()
        {
            effect = (player) => InvicibleTimeOn(player);
        }

        private void InvicibleTimeOn(Player player)
        {
            player.InvincibleRoutineStart(duration);
        }
    }
}