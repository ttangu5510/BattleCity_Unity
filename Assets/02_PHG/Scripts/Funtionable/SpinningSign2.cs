using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinningSign : MonoBehaviour
{
   private float rotationSpeed = 15f;

    private void Update()
    {

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

    }
   
}
