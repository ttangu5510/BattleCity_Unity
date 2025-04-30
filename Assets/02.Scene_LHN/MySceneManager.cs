using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using TMPro;

public class MySceneManager : MonoBehaviour
{

    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime;
    [SerializeField] TMP_Text loadingText;
    public static MySceneManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static MySceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeScene(string sceneName)
    {
        if (loadingRoutine == null)
            { 
            StartCoroutine(LoadingRoutine(sceneName));
        }
    }
    Coroutine loadingRoutine;
    IEnumerator LoadingRoutine(string sceneName)
    {
        float timer = 0;
        while (timer<fadeTime)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(0, 1, timer / fadeTime);
            fadeImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }
        
        loadingText.gameObject.SetActive(true);
        yield return null;

        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        Thread.Sleep(2000);
        loadingText.gameObject.SetActive(false);

        timer = 0;
        while (timer < fadeTime)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(1, 0, timer / fadeTime);
            fadeImage.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        loadingRoutine = null;
    }
}


