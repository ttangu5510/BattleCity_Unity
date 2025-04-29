using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletObjectPool : MonoBehaviour
{
    private Stack<PooledObject> bulletStack;
    [SerializeField] int poolSize;

}
