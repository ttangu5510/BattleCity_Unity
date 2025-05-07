using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] GameObject respawnFBX_Prefab;

    GameObject fbx;

    public void PlayerFBX()
    {
        fbx = Instantiate(respawnFBX_Prefab, transform);
    }

    public void StopFBX()
    {
        if (fbx != null) Destroy(fbx);
    }
}
