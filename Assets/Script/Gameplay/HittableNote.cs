using UnityEngine;

public class HittableNote : MonoBehaviour
{
    [SerializeField] NoteVFX VFXPrefab;
    [HideInInspector] public float noteTime;


    public void BeHit(NoteAccuracy accuracy)
    {
        playVFX(accuracy);
        Destroy(gameObject);
    }

    void playVFX(NoteAccuracy accuracy)
    {
        VFXPrefab = Instantiate(VFXPrefab, transform.position, Quaternion.identity);
        VFXPrefab.PlayVFX(accuracy);
    }

}
