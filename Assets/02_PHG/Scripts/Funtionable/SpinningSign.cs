using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinningSign2 : MonoBehaviour
{
   private float rotationSpeed = 50f;

    private void Update()
    {

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

    }
   
}
