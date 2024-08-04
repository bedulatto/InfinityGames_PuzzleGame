using UnityEngine;

public class GoToScene : MonoBehaviour
{
    [SerializeReference] string sceneName;

    public void GoToSceneTrigger()
    {
        GameManager.Instance.LoadScene(sceneName);
    }
}