using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_TimeStop : Item
{
    [SerializeField] float duration;

    public override void SetEffect()
    {
        effect = (player) => StopAllEnemies();
    }

    public void StopAllEnemies()
    {
        // �Ϲ� ���� ���������� ��ƾ ����
        List<Enemy> enemies = new List<Enemy>();
        foreach (Enemy enemyI in StageManager.Instance.ActivateEnemys)
        {
            enemies.Add(enemyI);
        }
        if (enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.TimeStopItemEffect(duration);
            }
        }

        // �������� ���������� ��ƾ ����
        List<Enemy_BossTurret> bossTurrets = new List<Enemy_BossTurret>();

        foreach (Enemy_BossTurret turrets in StageManager.Instance.bossTurrets)
        {
            bossTurrets.Add(turrets);
        }
        // ����ִ� �ͷ��� �ִٸ� ����
        if (bossTurrets.Count > 0)
        {
            // �ͷ��� ȿ�� ����
            foreach (Enemy_BossTurret turret in bossTurrets)
            {
                turret.TimeStopItemEffect(duration);
            }

            // ���뿡 ȿ�� ����
            if (StageManager.Instance.boss != null)
                StageManager.Instance.boss.TimeStopItemEffect(duration);
        }

    }
}
