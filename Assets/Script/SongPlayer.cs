using UnityEngine;
using static Unity.VisualScripting.Member;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] SongSO CurrentSong;
    AudioSource source;
    float songTime;
    bool isPlayingSong;


    float cameraOffBeatSize;
    float cameraOnBeatSize;

    void Start()
    {
        source = GetComponent<AudioSource>();
        StartSong();
        cameraOffBeatSize = Camera.main.orthographicSize;
        cameraOnBeatSize = Camera.main.orthographicSize - 1;
    }

    void StartSong()
    {
        source.clip = CurrentSong.SongClip;
        source.Play();
        songTime = 0;
        isPlayingSong = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayingSong)
        {
            songTime = source.time;
        }
        float beats = (songTime / 60.0f) * CurrentSong.BeatsPerMinute;
        Camera.main.orthographicSize = Mathf.Lerp(cameraOnBeatSize, cameraOffBeatSize, beats % 1);
        Debug.Log($"Beat: {(songTime / 60.0f) * CurrentSong.BeatsPerMinute}");
    }
}
