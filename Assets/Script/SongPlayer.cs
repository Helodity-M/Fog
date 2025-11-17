using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SongPlayer : MonoBehaviour
{
    InputAction inputAction;
    
    [SerializeField] SongSO CurrentSong;
    List<Tuple<GameObject, float>> NoteList;
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;

    [SerializeField] float audioOffset;
    [SerializeField] float visualOffset;

    [Header("Timing")]
    [SerializeField] float perfectTiming = 0.1f;
    [SerializeField] float greatTiming = 0.25f;
    [SerializeField] float okTiming = 0.4f;
    [SerializeField] float missTiming = 0.5f;

    List<HittableNote> NoteObjects;

    float lastBeat = -1;
    [Header("Debug")]
    [SerializeField] bool metronomeDebug;
    [SerializeField] AudioClip metronomeClip;
    AudioSource source;

    double songStartDSP;
    float songTime;
    bool isPlayingSong;

    int noteSpawnIdx = 0;

    void Start()
    {
        //Application.targetFrameRate = 60;
        NoteList = CurrentSong.Parse();
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
        songStartDSP = AudioSettings.dspTime + audioOffset;
    }
    void Update()
    {
        if (isPlayingSong)
        {
            if (source.isPlaying)
            {
                songTime = (float)(AudioSettings.dspTime - songStartDSP);
                if (metronomeDebug && Mathf.Floor(SecondsToBeats(songTime)) > lastBeat)
                {
                    AudioSource.PlayClipAtPoint(metronomeClip, Vector3.zero);
                    lastBeat++;
                }
            }
            else
            {
                songTime += Time.deltaTime;
            }
        }
        float currentBeat = SecondsToBeats(songTime);
        if (inputAction.WasPressedThisFrame())
        {
            float closest = missTiming; //Max gap from a note to "hit" it
            HittableNote closestNote = null;
            foreach (HittableNote note in NoteObjects)
            {
                float offset = Mathf.Abs(BeatToSeconds(note.noteTime - currentBeat));
                if (closest > offset)
                {
                    closest = offset;
                    closestNote = note;
                }
            }
            
            if(closestNote)
            {
                closestNote.BeHit(GetAccuracy(closest));
                NoteObjects.Remove(closestNote);
            }

            if (NoteList[NoteList.Count - 1].Item2 < SecondsToBeats(songTime))
            {
                SceneManager.LoadScene(0);
            }
        }

        //Spawning new notes
        for (int i = noteSpawnIdx; i < NoteList.Count; i++)
        {
            float spawnBeat = NoteList[i].Item2;
            //3 Second window
            if (spawnBeat - currentBeat <= SecondsToBeats(3))
            {
                HittableNote note = Instantiate(NotePrefab, transform);
                note.noteTime = NoteList[i].Item2;
                note.transform.position = new Vector3((spawnBeat + visualOffset - currentBeat) * ScrollSpeed, 0, 0);
                NoteObjects.Add(note);

                noteSpawnIdx++;
            } else
            {
                break;
            }

        }
        //Updating note positions + removing missed notes
        for (int i = 0; i < NoteObjects.Count; i++)
        {
            HittableNote n = NoteObjects[i];
            if (currentBeat - missTiming > n.noteTime)
            {
                n.OnMiss();
                NoteObjects.Remove(n);
                i--;
                continue;
            }
            n.transform.position = new Vector3((n.noteTime + visualOffset - currentBeat) * ScrollSpeed, 0, 0);
        }
    }

    public NoteAccuracy GetAccuracy(float offsetSeconds)
    {
        float absOffset = Mathf.Abs(offsetSeconds);

        if (absOffset < perfectTiming)
        {
            return NoteAccuracy.Perfect;
        }

        if(absOffset < greatTiming)
        {
            return NoteAccuracy.Great;
        }

        if(absOffset < okTiming)
        {
            return NoteAccuracy.OK;
        }
        return NoteAccuracy.Miss;
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
