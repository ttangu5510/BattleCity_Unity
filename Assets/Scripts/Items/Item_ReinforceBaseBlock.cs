using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ReinforceBaseBlock : Item
{
    public override void SetEffect()
    {
        // ���̽� �� �����丵 �� �߰� ����
        effect = (player) => BaseChange();
    }

    private void BaseChange()
    {
        StageManager.Instance.baseBlock.ChangeState();
    }
}
