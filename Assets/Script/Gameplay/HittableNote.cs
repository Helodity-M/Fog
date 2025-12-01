using UnityEngine;

public class HittableNote : MonoBehaviour
{
    [SerializeField] NoteVFX VFXPrefab;
    [HideInInspector] public float noteTime;


    public void BeHit(NoteAccuracy accuracy)
    {
        Debug.Log($"Hit note at {noteTime} ({accuracy})");
        playVFX(accuracy);
        Destroy(gameObject);
    }

    public void OnMiss()
    {
        Debug.Log($"Missed note at {noteTime}.");
        playVFX(NoteAccuracy.Miss);
        Destroy(gameObject);
    }

    void playVFX(NoteAccuracy accuracy)
    {
        VFXPrefab = Instantiate(VFXPrefab, transform.position, Quaternion.identity);
        VFXPrefab.PlayVFX(accuracy);
    }

}
