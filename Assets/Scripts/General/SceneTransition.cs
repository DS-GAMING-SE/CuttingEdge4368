using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image screenImage;
    [SerializeField]
    private TextMeshProUGUI text;
    [HideInInspector]
    public float transitionInTime;
    [HideInInspector]
    public float waitTime;
    [HideInInspector]
    public float transitionOutTime;
    private State state;
    [HideInInspector]
    public string sceneName;
    private AsyncOperation loadHandle;
    public void Initialize()
    {
        canvas.worldCamera = Camera.main;
        DontDestroyOnLoad(gameObject);
        screenImage.CrossFadeAlpha(0f, 0f, true);
        text.CrossFadeAlpha(0f, 0f, true);
        StartCoroutine(WaitForNextState(transitionInTime));
        screenImage.CrossFadeAlpha(1f, transitionInTime, true);
        text.CrossFadeAlpha(1f, transitionInTime, true);
    }
    private void Update()
    {
        if (!canvas.worldCamera)
        {
            canvas.worldCamera = Camera.main;
        }
    }
    public static void StartTransitionToScene(string sceneName, string label = null, float transitionIn = 0.8f, float wait = 1f, float transitionOut = 0.6f)
    {
        SceneTransition transition = GameObject.Instantiate(Resources.Load<GameObject>("ScreenTransition")).GetComponent<SceneTransition>();

        if (!string.IsNullOrEmpty(label))
        {
            transition.text.text = label;
        }
        transition.transitionInTime = transitionIn;
        transition.waitTime = wait;
        transition.transitionOutTime = transitionOut;
        transition.sceneName = sceneName;
        transition.Initialize();
    }
    private void SetNextStateOnSceneLoaded(AsyncOperation handle)
    {
        StartCoroutine(WaitForNextState(0f));
    }
    private IEnumerator WaitForNextState(float duration)
    {
        yield return new WaitForSeconds(duration);
        // state exit
        switch (state)
        {
            case State.Wait:
                if (!loadHandle.isDone)
                {
                    loadHandle.completed += SetNextStateOnSceneLoaded;
                    yield break;
                }
                break;
        }
        state++;
        // state start
        switch (state)
        {
            case State.Wait:
                loadHandle = SceneManager.LoadSceneAsync(sceneName);
                StartCoroutine(WaitForNextState(waitTime));
                break;
            case State.TransitionOut:
                screenImage.CrossFadeAlpha(0f, transitionOutTime, true);
                text.CrossFadeAlpha(0f, transitionOutTime / 2, true);
                StartCoroutine(WaitForNextState(transitionOutTime));
                break;
            case State.End:
                Destroy(gameObject);
                break;
        }
    }
    private enum State
    {
        TransitionIn,
        Wait,
        TransitionOut,
        End
    }
}
