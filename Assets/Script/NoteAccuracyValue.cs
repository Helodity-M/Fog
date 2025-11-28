using System;
using UnityEngine;

[Serializable]
public class NoteAccuracyValue
{
    [SerializeField] float perfectValue;
    [SerializeField] float greatValue;
    [SerializeField] float okValue;
    [SerializeField] float missValue;


    public float GetValue(NoteAccuracy accuracy)
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

    public NoteAccuracy GetAccuracy(float value)
    {
        if (value < perfectValue)
        {
            return NoteAccuracy.Perfect;
        }

        if (value < greatValue)
        {
            return NoteAccuracy.Great;
        }

        if (value < okValue)
        {
            return NoteAccuracy.OK;
        }
        return NoteAccuracy.Miss;
    }

}
