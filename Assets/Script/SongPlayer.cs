using System.Collections;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    
    public static SongSO CurrentSong;

    [SerializeField] float audioOffset;

    AudioSource source;

    double songStartDSP;
    float songTime;
    public bool SongCoroutineStarted;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void BeginPlayback(float delay)
    {
        StartCoroutine(StartSong(delay));
    }

    IEnumerator StartSong(float delay)
    {
        CurrentSong.SongClip.LoadAudioData();
        SongCoroutineStarted = true;
        songTime = -delay + audioOffset;
        source.clip = CurrentSong.SongClip;
        yield return new WaitForSeconds(delay);
        source.Play();
        songStartDSP = AudioSettings.dspTime + audioOffset;
    }
    void Update()
    {
        if (SongCoroutineStarted)
        {
            if (source.isPlaying)
            {
                songTime = (float)(AudioSettings.dspTime - songStartDSP);
            }
            else
            {
                //Allow proper enough timing before the song starts
                songTime += Time.deltaTime;
            }
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
    public float GetCurrentBeatNumber()
    {
        return SecondsToBeats(songTime);
    }
    public float GetSongTime()
    {
        return songTime;
    }
}
