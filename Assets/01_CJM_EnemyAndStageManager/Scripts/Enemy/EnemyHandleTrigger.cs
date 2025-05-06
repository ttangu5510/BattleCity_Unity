using UnityEngine;

namespace CJM
{
    public class EnemyHandleTrigger : MonoBehaviour
    {

        Enemy enemy;
        //[SerializeField] GameObject onTriggerObj;

        private void Awake()
        {
            enemy = transform.GetComponentInParent<Enemy>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
            {
                enemy.onTriggerObj = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
            {
                enemy.onTriggerObj = null;
            }
        }
    }
}
