using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Upgrade : Item
{

    public override void SetEffect()
    {
        effect = (player) => Upgrade(player);
    }

    private void Upgrade(Player player)
    {
        player.Upgrade();
    }
}
