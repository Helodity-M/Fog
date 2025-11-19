using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class GameplayManager : MonoBehaviour
{
    InputAction inputAction;

    [SerializeField] SongPlayer player;

    [Header("Timing")]
    [SerializeField] float perfectTiming = 0.1f;
    [SerializeField] float greatTiming = 0.25f;
    [SerializeField] float okTiming = 0.4f;
    [SerializeField] float missTiming = 0.5f;
    [SerializeField] float visualOffset;

    [Header("Notes")]
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;
    List<HittableNote> NoteObjects;
    List<Tuple<GameObject, float>> NoteList;
    int noteSpawnIdx = 0;



    private void Start()
    {
        NoteList = SongPlayer.CurrentSong.Parse();
        inputAction = InputSystem.actions.FindAction("Jump");
        NoteObjects = new List<HittableNote>();
        player.BeginPlayback(3);
    }


    private void Update()
    {
        if (inputAction.WasPerformedThisFrame())
        {
            TryHitNote();
        }
        SpawnNewNotes();
        UpdateNotePosition();
        if (player.GetCurrentBeatNumber() > SongPlayer.CurrentSong.EndBeat)
        {
            EndSong();
        }
    }
    void TryHitNote()
    {
        float closest = missTiming; //Max gap from a note to "hit" it
        HittableNote closestNote = null;
        foreach (HittableNote note in NoteObjects)
        {
            float offset = Mathf.Abs(player.BeatToSeconds(note.noteTime) - player.GetSongTime());
            if (closest > offset)
            {
                closest = offset;
                closestNote = note;
            }
        }

        if (closestNote)
        {
            closestNote.BeHit(GetAccuracy(closest));
            NoteObjects.Remove(closestNote);
        }
    }
    void SpawnNewNotes()
    {
        float currentBeat = player.GetCurrentBeatNumber();

        for (int i = noteSpawnIdx; i < NoteList.Count; i++)
        {
            float spawnBeat = NoteList[i].Item2;
            //5 Second window
            if (spawnBeat - currentBeat <= player.SecondsToBeats(5))
            {
                HittableNote note = Instantiate(NotePrefab, transform);
                note.noteTime = NoteList[i].Item2;
                note.transform.position = new Vector3((spawnBeat + visualOffset - currentBeat) * ScrollSpeed, 0, 0);
                NoteObjects.Add(note);

                noteSpawnIdx++;
            }
            else
            {
                break;
            }

        }
    }
    //Updating note positions + removing missed notes
    void UpdateNotePosition()
    {
        float currentBeat = player.GetCurrentBeatNumber();
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

    void EndSong()
    {
        if (SongPlayer.CurrentSong.CompletionCutscene != null)
        {
            CutsceneManager.currentCutscene = SongPlayer.CurrentSong.CompletionCutscene;
            SceneManager.LoadScene("Cutscene");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public NoteAccuracy GetAccuracy(float offsetSeconds)
    {
        float absOffset = Mathf.Abs(offsetSeconds);

        if (absOffset < perfectTiming)
        {
            return NoteAccuracy.Perfect;
        }

        if (absOffset < greatTiming)
        {
            return NoteAccuracy.Great;
        }

        if (absOffset < okTiming)
        {
            return NoteAccuracy.OK;
        }
        return NoteAccuracy.Miss;
    }
}
