using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFlag : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StageManager.Instance.StageFail();
        }
    }
}
