using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    InputAction inputAction;

    public float playerHealth { get; private set; }

    [SerializeField] SongPlayer player;

    [Header("Health")]
    [SerializeField] NoteAccuracyValue<float> healthChangeValues;
    
    [Header("Timing")]
    [SerializeField] NoteAccuracyValue<float> timingValues;
    [SerializeField] float visualOffset;

    [Header("Notes")]
    [SerializeField] HittableNote NotePrefab;
    [SerializeField] float ScrollSpeed;
    List<HittableNote> NoteObjects;
    List<Tuple<GameObject, float>> NoteList;
    int noteSpawnIdx = 0;

    [Header("Score")]
    [SerializeField] NoteAccuracyValue<int> scoreValues;
    ScoreKeeper scoreKeeper;

    [Header("Debug")]
    [SerializeField] SongSO DefaultSong;

    private void Start()
    {
        //In case we dont set something oops
        if (SongPlayer.CurrentSong == null)
        {
            SongPlayer.CurrentSong = DefaultSong;
            SongPlayer.IsFreeplay = true;
        }
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
            NoteAccuracy accuracy = GetAccuracy(Mathf.Abs(closest), timingValues);
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
        if (SongPlayer.CurrentSong.CompletionCutscene != null && !SongPlayer.IsFreeplay)
        {
            CutsceneManager.currentCutscene = SongPlayer.CurrentSong.CompletionCutscene;
            FadeManager.Instance.FadeToScene("Cutscene");
        }
        else
        {
            FadeManager.Instance.FadeToScene("MainMenu");
        }
    }


    public NoteAccuracy GetAccuracy(float value, NoteAccuracyValue<float> noteValues)
    {
        if (value < noteValues.GetValue(NoteAccuracy.Perfect))
        {
            return NoteAccuracy.Perfect;
        }

        if (value < noteValues.GetValue(NoteAccuracy.Great))
        {
            return NoteAccuracy.Great;
        }

        if (value < noteValues.GetValue(NoteAccuracy.OK))
        {
            return NoteAccuracy.OK;
        }
        return NoteAccuracy.Miss;
    }

    public void ModifyHealth(NoteAccuracy accuracy)
    {
        playerHealth = Mathf.Min(1, playerHealth + healthChangeValues.GetValue(accuracy));
    }
}
