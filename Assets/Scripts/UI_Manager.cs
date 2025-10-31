using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseGroup;
    private const float FADE_IN_OUT_SECONDS = 0.5f;

    void Start()
    {
        ShowPause(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FadeInOutPause(pauseGroup.alpha == 1 ? false : true));
        }
    }

    private void ShowPause(bool show)
    {
        pauseGroup.alpha = show ? 1 : 0;
        pauseGroup.interactable = show;
        pauseGroup.blocksRaycasts = show;
        Time.timeScale = show ? 0 : 1;
    }

    private IEnumerator FadeInOutPause(bool show)
    {
        float timeElapsed = 0;
        float actualAlpha = pauseGroup.alpha;
        float objectiveAlpha = show ? 1 : 0;

        while(timeElapsed < FADE_IN_OUT_SECONDS)
        {
            pauseGroup.alpha = Mathf.Lerp(actualAlpha, objectiveAlpha, timeElapsed / FADE_IN_OUT_SECONDS);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        ShowPause(show);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        StartCoroutine(FadeInOutPause(false));
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
