using UnityEngine;

public class TransformAnimator : MonoBehaviour
{
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;

    [SerializeField] float duration;
    float durationRemaining = 0;


    [SerializeField] bool playOnStart;
    bool reverse = false;

    private void Awake()
    {
        if (playOnStart)
        {
            StartAnimation();
        }
    }

    public void StartAnimation(bool r = false)
    {
        reverse = r;
        durationRemaining = duration;
    }

    private void Update()
    {
        durationRemaining -= Time.deltaTime;
        if (durationRemaining < 0)
            durationRemaining = 0;

        float time = 1 - (durationRemaining / duration);
        time = Mathf.Sin((time * Mathf.PI) / 2); //Ease Out

        Vector2 start = reverse ? endPos : startPos;
        Vector2 end = reverse ? startPos : endPos;


        transform.localPosition = Vector2.Lerp(start, end, time);
    }
}
