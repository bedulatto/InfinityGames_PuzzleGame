using UnityEngine;

public class LoadNextStage : MonoBehaviour
{
    public void TriggerLoadNextStage()
    {
        GameManager.Instance.LoadNextStage();
    }
}