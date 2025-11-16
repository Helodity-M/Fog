using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SongPlayer : MonoBehaviour
{
    InputAction inputAction;
    
    [SerializeField] SongSO CurrentSong;
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;

    [SerializeField] float audioOffset;
    [SerializeField] float visualOffset;
    [SerializeField] float maxHitTime = 0.5f;


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
        Application.targetFrameRate = 60;
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
            float closest = maxHitTime; //Max gap from a note to "hit" it
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
                closestNote.BeHit();
                NoteObjects.Remove(closestNote);
            }

            if (CurrentSong.NoteTimes[CurrentSong.NoteTimes.Count - 1] < SecondsToBeats(songTime))
            {
                SceneManager.LoadScene(0);
            }
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
        //Updating note positions + removing missed notes
        for (int i = 0; i < NoteObjects.Count; i++)
        {
            HittableNote n = NoteObjects[i];
            if (currentBeat - maxHitTime > n.noteTime)
            {
                n.OnMiss();
                NoteObjects.Remove(n);
                i--;
                continue;
            }
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
