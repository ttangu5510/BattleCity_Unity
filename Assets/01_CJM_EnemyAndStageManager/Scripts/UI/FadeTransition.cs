using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace CJM
{
    public class FadeTransition : MonoBehaviour
    {
        [SerializeField] Image fadeImage;
        [SerializeField] float fadeTime;
        [SerializeField] public TMP_Text loadingText;
        // Start is called before the first frame update

        Coroutine loadingRoutine;

        public void StartLoading(string sceneName)
        {
            if (loadingRoutine == null)
            {
                loadingRoutine = StartCoroutine(LoadingRoutine(sceneName));
            }
        }

        public IEnumerator LoadingRoutine(string sceneName)
        {
            Debug.Log(sceneName);
            float timer = 0;
            while (timer < fadeTime)
            {
                Color color = fadeImage.color;
                color.a = Mathf.Lerp(0, 1, timer / fadeTime);
                fadeImage.color = color;

                timer += Time.deltaTime;
                yield return null;
            }

            AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
            Debug.Log($"LoadingText : {loadingText}");
            loadingText.text = $"{sceneName}";
            loadingText.gameObject.SetActive(true);
            yield return null;
            Thread.Sleep(1000);
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
}