using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFlag : MonoBehaviour
{
    [SerializeField] GameObject BrokenObject;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Instantiate(BrokenObject,transform.position,transform.rotation);
            Destroy(gameObject);
            StageManager.Instance.StageFail();
        }
    }
}
