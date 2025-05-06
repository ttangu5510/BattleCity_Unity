using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ReinforceBaseBlock : Item
{
    public override void SetEffect()
    {
        // 베이스 블럭 리팩토링 후 추가 가능
        effect = (player) => BaseChange();
    }

    private void BaseChange()
    {
        Debug.Log(StageManager.Instance);
        Debug.Log(StageManager.Instance.baseBlock);
        StageManager.Instance.baseBlock.ChangeState();
    }
}
