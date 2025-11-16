using UnityEngine;

public class HittableNote : MonoBehaviour
{
    [HideInInspector] public float noteTime;


    public void BeHit()
    {
        Debug.Log($"Hit note at {noteTime}");
        Destroy(gameObject); //TODO: Replace with death animation
    }

    public void OnMiss()
    {
        Debug.Log($"Missed note at {noteTime}.");
        Destroy(gameObject); //TODO: add damage and run off effect
    }
}
