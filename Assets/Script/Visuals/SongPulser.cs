using UnityEngine;

public class SongPulser : MonoBehaviour
{
    [SerializeField] float Amplitude;

    Vector3 baseScale;
    SongPlayer player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = (SongPlayer)FindAnyObjectByType(typeof(SongPlayer));
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float beats = player.GetCurrentBeatNumber();
        float fraction = beats % 1;
        if(fraction < 0) fraction += 1;
        fraction = Mathf.Sin((fraction * Mathf.PI) / 2); // Add Easing
        transform.localScale = Vector3.Lerp(baseScale * (1 + Amplitude), baseScale, fraction);
    }
}
