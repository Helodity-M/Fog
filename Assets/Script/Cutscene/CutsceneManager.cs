using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneSO currentCutscene;

    [SerializeField] Transform characterParent;
    [SerializeField] Image characterPrefab;
    public int dialogueIdx;
    public SpriteRenderer backgroundRenderer;

    List<Image> characterObjects = new List<Image>();

    public TMP_Text Textbox;

    InputAction inputAction;


    bool cutsceneDone = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeCutscene();
        inputAction = InputSystem.actions.FindAction("Jump");
    }


    void InitializeCutscene()
    {
        float canvasWidth = ((Canvas)FindAnyObjectByType(typeof(Canvas))).GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < currentCutscene.characters.Count; i++)
        {
            float ratio = (float)(i + 1) / (currentCutscene.characters.Count + 1) * 2 - 1;

            Debug.Log(ratio);

            CutsceneCharacter c = currentCutscene.characters[i];
            Image newChar = Instantiate(characterPrefab, characterParent);
            characterObjects.Add(newChar);
            newChar.sprite = c.characterSprite;
            //newChar.rectTransform.re = c.characterSprite.textureRect.height;

            newChar.rectTransform.anchoredPosition = new Vector3(ratio * (canvasWidth * 0.5f), 0, 0);
        }
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

        for (int i = 0; i < currentCutscene.characters.Count; i++)
        {
            if (currentCutscene.dialogue[dialogueIdx].SpeakerIdx == i)
            {
                characterObjects[i].color = new Color(1,1,1);
            }
            else
            {
                characterObjects[i].color = new Color(0.8f, 0.8f, 0.8f);
            }
        }

        if (inputAction.WasPressedThisFrame())
        {
            dialogueIdx += 1;
        }
    }
}
