using UnityEngine;


namespace CJM
{
    public class GameOverFlag : MonoBehaviour
    {
        [SerializeField] GameObject FlagObject;
        [SerializeField] GameObject BrokenObject;

        private void Start()
        {
            EnemyManager.Instance.gameOverFlag = this.gameObject;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                BrokenObject.SetActive(true);
                FlagObject.SetActive(false);
                StageManager.Instance.StageFail();
            }
        }
    }
}
