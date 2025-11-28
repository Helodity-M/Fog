using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    InputAction inputAction;

    public float playerHealth { get; private set; }

    [SerializeField] SongPlayer player;

    [Header("Health")]
    [SerializeField] NoteAccuracyValue healthChangeValues;
    

    [Header("Timing")]
    [SerializeField] NoteAccuracyValue timingValues;
    [SerializeField] float visualOffset;

    [Header("Notes")]
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;
    List<HittableNote> NoteObjects;
    List<Tuple<GameObject, float>> NoteList;
    int noteSpawnIdx = 0;

    [Header("Score")]
    [SerializeField] NoteAccuracyValue scoreValues;
    ScoreKeeper scoreKeeper;

    private void Start()
    {
        playerHealth = 1;
        NoteList = SongPlayer.CurrentSong.Parse();
        inputAction = InputSystem.actions.FindAction("Jump");
        NoteObjects = new List<HittableNote>();
        player.BeginPlayback(2);
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
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
        float closest = timingValues.GetValue(NoteAccuracy.Miss); //Max gap from a note to "hit" it
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
            NoteAccuracy accuracy = timingValues.GetAccuracy(Mathf.Abs(closest));
            closestNote.BeHit(accuracy);
            ModifyHealth(accuracy);
            NoteObjects.Remove(closestNote);
            scoreKeeper.ModifyScore((int)scoreValues.GetValue(accuracy));
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
            if (currentBeat - timingValues.GetValue(NoteAccuracy.Miss) > n.noteTime)
            {
                n.OnMiss();
                ModifyHealth(NoteAccuracy.Miss);
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


    public void ModifyHealth(NoteAccuracy accuracy)
    {
        playerHealth = Mathf.Min(1, playerHealth + healthChangeValues.GetValue(accuracy));
    }
}
