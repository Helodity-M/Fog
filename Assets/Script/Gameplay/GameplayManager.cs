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

    [Header("Environment")]
    [SerializeField] Vector2 SpawnOffset;

    [Header("Score")]
    [SerializeField] NoteAccuracyValue<int> scoreValues;
    ScoreKeeper scoreKeeper;

    [SerializeField] CutsceneSO LoseCutscene;

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
        Instantiate(SongPlayer.CurrentSong.EnvironmentPrefab, SpawnOffset, Quaternion.identity);
        inputAction = InputSystem.actions.FindAction("Jump");
        NoteObjects = new List<HittableNote>();
        player.BeginPlayback(2);
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
    }


    private void Update()
    {
        if (!IsAlive())
        {
            CutsceneManager.currentCutscene = LoseCutscene;
            FadeManager.Instance.FadeToScene("Cutscene");
        }

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
        float closest = timingValues.GetValue(NoteAccuracy.OK); //Max gap from a note to "hit" it
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
            HitNote(closestNote, accuracy);
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
                note.noteTime = spawnBeat;
                note.transform.position = GetNotePosition(spawnBeat);
                NoteObjects.Add(note);

                noteSpawnIdx++;
            }
            else
            {
                //Notes are already sorted, so if we cant spawn this one yet we cant spawn any more
                break;
            }

        }
    }

    Vector3 GetNotePosition(float noteBeat)
    {
        float currentBeat = player.GetCurrentBeatNumber();
        return new Vector3(player.BeatToSeconds(noteBeat + visualOffset - currentBeat) * ScrollSpeed, 0, 0);
    }

    //Updating note positions + removing missed notes
    void UpdateNotePosition()
    {
        float currentBeat = player.GetCurrentBeatNumber();
        for (int i = 0; i < NoteObjects.Count; i++)
        {
            HittableNote n = NoteObjects[i];
            if (currentBeat - timingValues.GetValue(NoteAccuracy.OK) > n.noteTime)
            {
                HitNote(n, NoteAccuracy.Miss);
                i--;
                continue;
            }
            n.transform.position = GetNotePosition(n.noteTime);
        }
    }
    void HitNote(HittableNote note, NoteAccuracy accuracy)
    {
        note.BeHit(accuracy);
        ModifyHealth(accuracy);
        scoreKeeper.ModifyScore(scoreValues.GetValue(accuracy));
        NoteObjects.Remove(note);
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

    bool IsAlive()
    {
        return playerHealth > 0 || UserOptions.NoFail;
    }

    public void ModifyHealth(NoteAccuracy accuracy)
    {
        playerHealth = Mathf.Min(1, playerHealth + healthChangeValues.GetValue(accuracy));
    }
}
