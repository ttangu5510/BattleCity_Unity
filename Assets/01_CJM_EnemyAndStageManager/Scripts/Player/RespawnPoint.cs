﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJM
{
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField] GameObject respawnFBX_Prefab;


        public void PlayerFBX()
        {
            Instantiate(respawnFBX_Prefab, transform);
        }

        public void StopFBX()
        {

        }
    }
}
