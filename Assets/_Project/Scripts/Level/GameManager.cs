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

    [SerializeField] ProgressSO playerProgress;
    [SerializeField] ScreenTransition screenTransition;
    [SerializeField] string hubScene = "HubScene";
    [SerializeField] List<StageInfo> stageInfoList;
    [SerializeField] float inTransitionTime = 0.4f;
    [SerializeField] float outTransitionTime = 0.4f;

    public List<StageInfo> StageInfoList => stageInfoList;

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

    private void Start()
    {
        playerProgress.OnLastStageChanged += PlayerProgress_OnLastStageChanged;
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

    private void PlayerProgress_OnLastStageChanged()
    {
        int stageIndex = playerProgress.LastStage;
        stageIndex = Mathf.Min(stageIndex, stageInfoList.Count - 1);
        StartCoroutine(WaitAndLoadScene(stageInfoList[stageIndex].SceneName, 2));
    }

    private IEnumerator WaitAndLoadScene(string sceneName, float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
        LoadScene(sceneName);
    }
}
