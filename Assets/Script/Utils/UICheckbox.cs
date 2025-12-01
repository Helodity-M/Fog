///
/// Checkbox Util from https://github.com/Helodity/bisyntho-source
///
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UICheckbox : MonoBehaviour
{
    [SerializeField] Sprite EnabledSprite;
    [SerializeField] Sprite DisabledSprite;
    [SerializeField] Image CheckboxObject;

    [Header("Events")]
    [SerializeField] UnityEvent<bool> OnValueChanged;

    public bool CheckboxEnabled { get; protected set; }


    private void Awake()
    {
        CheckboxEnabled = GetStartEnabled();
        UpdateSprite();
    }

    protected abstract bool GetStartEnabled();

    public void Toggle()
    {
        CheckboxEnabled = !CheckboxEnabled;
        UpdateSprite();
        UpdateSave();
        OnValueChanged?.Invoke(CheckboxEnabled);
    }
    public void SetState(bool state)
    {
        bool fireEv = state != CheckboxEnabled;
        CheckboxEnabled = state;
        UpdateSprite();
        UpdateSave();
        if (fireEv)
            OnValueChanged?.Invoke(CheckboxEnabled);
    }

    void UpdateSprite()
    {
        CheckboxObject.sprite = CheckboxEnabled ? EnabledSprite : DisabledSprite;
    }
    protected abstract void UpdateSave();
}
