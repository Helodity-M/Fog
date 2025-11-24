using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] float degreesPerSecond;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward *  degreesPerSecond * Time.deltaTime);
    }
}
