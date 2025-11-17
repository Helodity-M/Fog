using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotePattern", menuName = "Scriptable Objects/NotePattern")]
public class NotePatternSO : ScriptableObject
{
    public GameObject NoteObject;
    public List<float> NoteTimes;
}
