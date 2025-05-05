using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameObjectAction : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Vector3 dir = transform.position - collision.transform.position;
            Destroy(gameObject, 3f);
            gameObject.GetComponent<Rigidbody>().AddForce(dir * 15, ForceMode.Impulse);

        }
        if(collision.gameObject.tag =="Player"|| collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject, 3f);
        }
    }
}
