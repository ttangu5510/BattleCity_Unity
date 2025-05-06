using System.Collections.Generic;


namespace CJM
{
    public class Item_KillAllEnemies : Item
    {
        public override void SetEffect()
        {
            effect = (player) => AllKill();
        }

        public void AllKill()
        {
            List<Enemy> enemies = new List<Enemy>();

            foreach (Enemy enemyI in StageManager.Instance.ActivateEnemys)
            {
                enemies.Add(enemyI);
            }

            if (enemies.Count > 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.Dead();
                }
            }
        }
    }
}
