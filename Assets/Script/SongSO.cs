using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "Scriptable Objects/Song")]
public class SongSO : ScriptableObject
{
    public AudioClip SongClip;

    public int BeatsPerMinute;
    public List<decimal> NoteTimes;

}
