using System;
using UnityEngine;

[Serializable]
public class NoteAccuracyValue<T>
{
    [SerializeField] T perfectValue;
    [SerializeField] T greatValue;
    [SerializeField] T okValue;
    [SerializeField] T missValue;


    public T GetValue(NoteAccuracy accuracy)
    {
        switch (accuracy)
        {
            case NoteAccuracy.Perfect:
                return perfectValue;
            case NoteAccuracy.Great:
                return greatValue;
            case NoteAccuracy.OK:
                return okValue;
            case NoteAccuracy.Miss:
                return missValue;
            default:
                return missValue;
        }
    }
}
