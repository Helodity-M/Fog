using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneSO", menuName = "Scriptable Objects/CutsceneSO")]
public class CutsceneSO : ScriptableObject
{
    public Sprite BackgroundSprite;
    public List<CutsceneCharacter> characters;
    public List<DialogueBox> dialogue;
}

[Serializable]
public class CutsceneCharacter
{
    public Sprite characterRenderer;
}


[Serializable]
public class DialogueBox
{
    public string Text;
    public int SpeakerIdx;
}