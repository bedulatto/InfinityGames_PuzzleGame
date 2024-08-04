using UnityEngine;

public class ReturnToHub : MonoBehaviour
{
    public void TriggerReturnToHub()
    {
        GameManager.Instance.LoadScene();
    }
}
