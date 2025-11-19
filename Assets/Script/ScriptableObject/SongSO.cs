using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "Scriptable Objects/Song")]
public class SongSO : ScriptableObject
{
    public CutsceneSO CompletionCutscene;
    public AudioClip SongClip;

    public int BeatsPerMinute;
    [SerializeField] List<float> NoteTimes;
    [SerializeField] List<NotePatternSO> NotePatterns;



    public List<Tuple<GameObject, float>> Parse()
    {
        List<Tuple<GameObject, float>> parsedSong = new();
        if (NotePatterns.Count != NoteTimes.Count)
        {
            Debug.LogError("Error in parsing song: There is not an equal number of patterns and pattern times!");
            return parsedSong;
        }
        for (int i = 0; i < NotePatterns.Count; i++)
        {
            float patternOffset = NoteTimes[i];


            for (int n = 0; n < NotePatterns[i].NoteTimes.Count; n++)
            {
                parsedSong.Add(
                    new Tuple<GameObject, float>(
                        NotePatterns[i].NoteObject,
                        NotePatterns[i].NoteTimes[n] + patternOffset
                    )
                );

            }
        }
        return parsedSong;
    }
}
