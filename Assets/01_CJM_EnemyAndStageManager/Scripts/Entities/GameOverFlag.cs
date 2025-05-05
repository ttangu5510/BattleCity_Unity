using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFlag : MonoBehaviour
{
    [SerializeField] GameObject FlagObject;
    [SerializeField] GameObject BrokenObject;
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
