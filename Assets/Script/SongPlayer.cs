using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SongPlayer : MonoBehaviour
{
    InputAction inputAction;


    [SerializeField] SongSO CurrentSong;
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;

    [SerializeField] float audioOffset;
    [SerializeField] float visualOffset;

    List<HittableNote> NoteObjects;

    AudioSource source;
    float songTime;
    bool isPlayingSong;

    int noteSpawnIdx = 0;

    void Start()
    {
        inputAction = InputSystem.actions.FindAction("Jump");
        NoteObjects = new List<HittableNote>();
        source = GetComponent<AudioSource>();
        StartCoroutine(StartSong());
    }

    IEnumerator StartSong()
    {
        CurrentSong.SongClip.LoadAudioData();
        isPlayingSong = true;
        songTime = -3 + audioOffset;
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
                songTime = source.time + audioOffset;
            }
            else
            {
                songTime += Time.deltaTime;
            }
        }
        float currentBeat = (songTime / 60.0f) * CurrentSong.BeatsPerMinute;
        if (inputAction.WasPressedThisFrame())
        {
            float closest = float.MaxValue;
            foreach (HittableNote note in NoteObjects)
            {
                closest = Mathf.Min(closest, Mathf.Abs(BeatToSeconds(note.noteTime - songTime)));
            }
            Debug.Log($"Closest note is {closest * 1000}ms off");
        }

        //Spawning new notes
        for (int i = noteSpawnIdx; i < CurrentSong.NoteTimes.Count; i++)
        {
            float spawnBeat = CurrentSong.NoteTimes[i];
            //3 Second window
            if (spawnBeat - currentBeat <= SecondsToBeats(3))
            {
                HittableNote note = Instantiate(NotePrefab, transform);
                note.noteTime = CurrentSong.NoteTimes[i];
                note.transform.position = new Vector3((spawnBeat + visualOffset - currentBeat) * ScrollSpeed, 0, 0);
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
            n.transform.position = new Vector3((n.noteTime + visualOffset - currentBeat) * ScrollSpeed, 0, 0);
        }

    }

    public float BeatToSeconds(float beats)
    {
        if (!CurrentSong) return 0;
        return (beats / CurrentSong.BeatsPerMinute) * 60.0f;
    }
    public float SecondsToBeats(float seconds)
    {
        if (!CurrentSong) return 0;
        return (seconds / 60.0f) * CurrentSong.BeatsPerMinute;
    }
    public float GetCurrentBeatNumber(bool useVisualOffset)
    {
        return SecondsToBeats(songTime + (useVisualOffset ? visualOffset : 0));
    }
}
