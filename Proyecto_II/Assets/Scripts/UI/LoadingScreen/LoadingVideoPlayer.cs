using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Jone Sainz Egea
// 19/05/2025
public class LoadingVideoPlayer : MonoBehaviour
{
    public static LoadingVideoPlayer Instance;

    [Header("Components")]
    public VideoPlayer videoPlayer;

    public bool IsPlaying => videoPlayer != null && videoPlayer.isPlaying;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.isLooping = true;
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
    }
}
