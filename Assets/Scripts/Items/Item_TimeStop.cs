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
        // 일반 몬스터 정지아이템 루틴 실행
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

        // 보스몬스터 정지아이템 루틴 실행
        List<Enemy_BossTurret> bossTurrets = new List<Enemy_BossTurret>();

        foreach (Enemy_BossTurret turrets in StageManager.Instance.bossTurrets)
        {
            bossTurrets.Add(turrets);
        }
        // 살아있는 터렛이 있다면 실행
        if (bossTurrets.Count > 0)
        {
            // 터렛에 효과 실행
            foreach (Enemy_BossTurret turret in bossTurrets)
            {
                turret.TimeStopItemEffect(duration);
            }

            // 몸통에 효과 실행
            if (StageManager.Instance.boss != null)
                StageManager.Instance.boss.TimeStopItemEffect(duration);
        }

    }
}
