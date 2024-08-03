using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class StageInfo
    {
        public string DisplayName;
        public string SceneName;
    }

    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] ScreenTransition screenTransition;
    [SerializeField] List<StageSO> stageList;
    [SerializeField] string hubScene = "HubScene";
    [SerializeField] float inTransitionTime = 0.4f;
    [SerializeField] float outTransitionTime = 0.4f;

    public List<StageSO> StageList => stageList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneRoutine(hubScene));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public IEnumerator LoadSceneRoutine(string sceneName)
    {
        screenTransition.EnterTransition(inTransitionTime);
        yield return new WaitForSeconds(inTransitionTime);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
        {
            screenTransition.UpdateProgress(loadOperation.progress);
            yield return null;
        }

        screenTransition.LeaveTransition(outTransitionTime);
    }

    public void WaitAndLoadScene(string sceneName, float waitDuration)
    {
        StartCoroutine(WaitAndLoadSceneRoutine(sceneName, waitDuration));
    }

    private IEnumerator WaitAndLoadSceneRoutine(string sceneName, float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
        LoadScene(sceneName);
    }
}
