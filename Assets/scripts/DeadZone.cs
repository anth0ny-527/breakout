using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        GameManager.instance.LoseLife();
    }
}