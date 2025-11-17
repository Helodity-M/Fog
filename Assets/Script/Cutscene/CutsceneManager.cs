using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneSO currentCutscene;

    [SerializeField] SpriteRenderer characterPrefab;
    public int dialogueIdx;
    public SpriteRenderer backgroundRenderer;

    public TMP_Text Textbox;

    InputAction inputAction;


    bool cutsceneDone = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(currentCutscene);
        inputAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueIdx >= currentCutscene.dialogue.Count)
        {
            SceneManager.LoadScene("Gameplay");
            return;
        }
        Textbox.text = currentCutscene.dialogue[dialogueIdx].Text;

        if (inputAction.WasPressedThisFrame())
        {
            dialogueIdx += 1;
        }
    }
}
