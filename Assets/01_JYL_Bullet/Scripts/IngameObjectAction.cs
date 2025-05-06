using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameObjectAction : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rig = gameObject.GetComponent<Rigidbody>();
        if(collision.gameObject.tag == "Bullet")
        {
            Vector3 dir = transform.position - collision.transform.position;
            Destroy(gameObject, 2f);
            rig.AddForce((dir+Vector3.up) * 15, ForceMode.Impulse);
            int num = LayerMask.GetMask("Props");
            //gameObject.layer
        }
        if(collision.gameObject.tag =="Player"|| collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject, 2f);
            gameObject.layer = LayerMask.NameToLayer("Props");
        }
    }
}
