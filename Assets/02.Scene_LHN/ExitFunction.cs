using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFunction : MonoBehaviour
{
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
