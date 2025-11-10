using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] SongSO CurrentSong;
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;

    List<HittableNote> NoteObjects;

    AudioSource source;
    float songTime;
    bool isPlayingSong;

    int noteSpawnIdx = 0;

    void Start()
    {
        NoteObjects = new List<HittableNote>();
        source = GetComponent<AudioSource>();
        StartCoroutine(StartSong());
    }

    IEnumerator StartSong()
    {
        CurrentSong.SongClip.LoadAudioData();
        isPlayingSong = true;
        songTime = -3;
        source.clip = CurrentSong.SongClip;
        yield return new WaitForSeconds(3);
        source.Play();
    }

    void Update()
    {
        if (isPlayingSong)
        {
            if (source.isPlaying)
            {
                songTime = source.time;
            }
            else
            {
                songTime += Time.deltaTime;
            }
        }
        float currentBeat = (songTime / 60.0f) * CurrentSong.BeatsPerMinute;
        Debug.Log($"Beat: {Mathf.Floor(currentBeat)}:{currentBeat % 1}");

        //Spawning new notes
        for(int i = noteSpawnIdx; i < CurrentSong.NoteTimes.Count; i++)
        {
            float spawnBeat = CurrentSong.NoteTimes[i];
            //3 Second window
            if (spawnBeat - currentBeat <= 3 * (CurrentSong.BeatsPerMinute / 60.0f))
            {
                HittableNote note = Instantiate(NotePrefab, transform);
                note.noteTime = CurrentSong.NoteTimes[i]; ;
                note.transform.position = new Vector3((spawnBeat - currentBeat) * ScrollSpeed, 0, 0);
                NoteObjects.Add(note);

                noteSpawnIdx++;
            } else
            {
                break;
            }

        }
        //Updating note positions
        foreach(HittableNote n in NoteObjects)
        {
            n.transform.position = new Vector3((n.noteTime - currentBeat) * ScrollSpeed, 0, 0);
        }

    }


    public float GetCurrentBeatNumber()
    {
        if (!CurrentSong) return 0;
        return (songTime / 60.0f) * CurrentSong.BeatsPerMinute;
    }
}
