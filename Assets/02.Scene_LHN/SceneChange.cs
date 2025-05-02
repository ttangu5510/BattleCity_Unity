using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        MySceneManager.Instance.ChangeScene(sceneName);
    }
}
