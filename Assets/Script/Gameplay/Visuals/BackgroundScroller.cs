using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 ScrollSpeed;

    Vector2 offset;
    Material mat;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        offset = Vector2.zero;
    }

    private void Update()
    {
        offset += ScrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}