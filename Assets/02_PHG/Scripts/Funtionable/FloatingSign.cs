using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSign : MonoBehaviour
{

    public float floatSpeed = 1f; // 떠오르는 속도
    public float floatHeight = 0.5f; // 떠오르는 높이
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + Vector3.up * newY;
    }

}
