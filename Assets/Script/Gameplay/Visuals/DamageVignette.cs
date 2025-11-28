using UnityEngine;
using UnityEngine.UI;

public class DamageVignetter : MonoBehaviour
{
    Image img;
    GameplayManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();
        gameManager = FindFirstObjectByType<GameplayManager>();
    }

    // Update is called once per frame
    void Update()
    {
       img.color = new Color(1, 1, 1, 1 - gameManager.playerHealth);
    }
}
