using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandleTrigger : MonoBehaviour
{

    Enemy enemy;
    //[SerializeField] GameObject onTriggerObj;

    private void Awake()
    {
        enemy = transform.GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
        {
            if (enemy.onTriggerObj == null) enemy.RandomDirSet();

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
