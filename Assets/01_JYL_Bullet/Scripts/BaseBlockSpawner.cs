using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockSpawner : MonoBehaviour
{
    [SerializeField] GameObject BaseBrickSet;
    [SerializeField] Transform beforeBlock;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(beforeBlock.gameObject);
            beforeBlock = Instantiate(BaseBrickSet, beforeBlock.position, beforeBlock.rotation).transform;
        }
    }
}
