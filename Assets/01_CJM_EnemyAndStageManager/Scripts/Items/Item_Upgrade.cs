using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CJM
{

    public class Item_Upgrade : Item
    {
        [SerializeField] int score_Bonus;

        public override void SetEffect()
        {
            effect = (player) => Upgrade(player);
        }

        private void Upgrade(Player player)
        {
            player.Upgrade(score_Bonus);
        }
    }
}
