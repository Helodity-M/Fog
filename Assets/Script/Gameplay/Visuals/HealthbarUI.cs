using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Image FillImage;
    [SerializeField] Gradient HealthColor;
    [SerializeField] float ValueChangeSpeed;
    protected float CurValue;
    GameplayManager gameplayManager;


    private void Awake()
    {
        gameplayManager = FindFirstObjectByType<GameplayManager>();
    }
    void Update()
    {
        FillImage.color = HealthColor.Evaluate(CurValue);
        if (UserOptions.NoFail)
        {
            //Force full health
            CurValue = 1;
            healthSlider.value = 1;
            return;
        }
        float targetScore = gameplayManager.playerHealth;
        float difference = targetScore - CurValue;
        float toChange = difference * ValueChangeSpeed * Time.unscaledDeltaTime;
        toChange = Mathf.Clamp(toChange, Mathf.Sign(difference) * -difference, Mathf.Sign(difference) * difference);
        CurValue += toChange;
        healthSlider.value = CurValue;
    }
}

