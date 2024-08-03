using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [Header("Data")]
    [SerializeField] ProgressSO progress;
    [SerializeField] List<StageSO> stageList;

    [Header("Components")]
    [SerializeField] ScreenTransition screenTransition;

    [Header("Scene Transition")]
    [SerializeField] string hubScene = "HubScene";
    [SerializeField] float inTransitionTime = 0.4f;
    [SerializeField] float outTransitionTime = 0.4f;
    [SerializeField] float waitToLoadNextStage = 3;

    private StageSO currentStage;

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

    private void Start()
    {
        NodeManager.OnAllPathCompleted += NodeManager_OnAllPathCompleted;
        NodeManager.OnPathCompleted += NodeManager_OnPathCompleted;
    }

    private void OnDestroy()
    {
        NodeManager.OnAllPathCompleted -= NodeManager_OnAllPathCompleted;
        NodeManager.OnPathCompleted -= NodeManager_OnPathCompleted;
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

    public void WaitAndLoadStage(StageSO stageInfo , float waitDuration)
    {
        StartCoroutine(WaitAndLoadStageRoutine(stageInfo, waitDuration));
    }

    private IEnumerator WaitAndLoadStageRoutine(StageSO stageInfo, float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
        LoadStage(stageInfo);
    }

    [ContextMenu("Trigger")]
    public void TriggerScore()
    {
        progress.AddExperience(currentStage.StageScoreValue);
    }

    public void LoadStage(StageSO stageInfo)
    {
        currentStage = stageInfo;
        LoadScene(stageInfo.SceneName);
    }

    private void NodeManager_OnAllPathCompleted()
    {
        int stageIndex = StageList.IndexOf(currentStage);
        progress.RegisterStageBeat(stageIndex);
        int nextStageIndex = Mathf.Min(progress.LastStagePlayed + 1, StageList.Count - 1);
        WaitAndLoadStage(StageList[nextStageIndex], waitToLoadNextStage);
    }

    private void NodeManager_OnPathCompleted(int pathId)
    {
        TriggerScore();
    }
}
