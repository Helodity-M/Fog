///
/// Stat tracker Util from https://github.com/Helodity/bisyntho-source
///

using TMPro;
using UnityEngine;

public abstract class StatTracker : MonoBehaviour
{
    TMP_Text textbox;
    public string preText;
    public float ValueChangeSpeed;
    protected float CurValue;

    private void Awake()
    {
        textbox = GetComponent<TMP_Text>();
    }

    void Update()
    {
        int targetScore = GetTargetValue();
        float difference = Mathf.Ceil(targetScore - CurValue);
        float toChange = difference * ValueChangeSpeed * Time.unscaledDeltaTime;
        toChange = Mathf.Clamp(toChange, Mathf.Sign(difference) * -difference, Mathf.Sign(difference) * difference);
        CurValue += toChange;
        textbox.text = GetText();
    }
    protected virtual string GetText()
    {
        return preText + Mathf.Floor(CurValue);
    }
    protected abstract int GetTargetValue();
}