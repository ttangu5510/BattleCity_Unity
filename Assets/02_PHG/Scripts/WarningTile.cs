using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningTile : MonoBehaviour
{
    [SerializeField] Material tileMaterial;
    [SerializeField] Color warningColor = Color.red;
    [SerializeField] float blinkSpeed = 2f;

    void Update()
    {
        float intensity = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        tileMaterial.SetColor("_EmissionColor", warningColor * intensity);
    }
}
