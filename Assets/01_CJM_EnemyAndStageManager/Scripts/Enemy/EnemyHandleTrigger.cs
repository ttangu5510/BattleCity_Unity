using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandleTrigger : MonoBehaviour
{

    Enemy enemy;
    [SerializeField] GameObject onTriggerObj;

    private void Awake()
    {
        enemy = transform.GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
        {
            Debug.Log("º® Ã³¸®");

            if (onTriggerObj == null) enemy.RandomDirSet();

            onTriggerObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
        {
            onTriggerObj = null;
        }
    }
}
