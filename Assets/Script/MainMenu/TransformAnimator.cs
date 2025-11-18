using UnityEngine;

public class TransformAnimator : MonoBehaviour
{
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;

    [SerializeField] float duration;
    float durationRemaining = 0;


    [SerializeField] bool playOnStart;

    private void Awake()
    {
        if (playOnStart)
        {
            durationRemaining = duration;
        }
    }

    private void Update()
    {
        durationRemaining -= Time.deltaTime;
        if (durationRemaining < 0)
            durationRemaining = 0;

        float time = 1 - (durationRemaining / duration);
        time = Mathf.Sin((time * Mathf.PI) / 2); //Ease Out

        transform.localPosition = Vector2.Lerp(startPos, endPos, time);
    }
}
