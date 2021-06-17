using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreloaderManager : MonoBehaviour
{
    [SerializeField] private Image image = default;
    [SerializeField] private Text text = default;

    private void Start()
    {
        StartCoroutine(Loader());
    }

    IEnumerator Loader()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            image.fillAmount = progress;
            text.text = ((int)(100 * progress)).ToString() + "%";

            yield return null;
        }
    }



}
