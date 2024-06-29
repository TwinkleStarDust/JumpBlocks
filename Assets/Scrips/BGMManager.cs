using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    public AudioSource bgmAudioSource; // 背景音乐音频源

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 在加载新场景时不销毁此对象
        }
        else
        {
            Destroy(gameObject); // 如果已经有一个BGMManager实例，则销毁新创建的对象
        }
    }

    // 用于播放背景音乐
    public void PlayBGM()
    {
        if (!bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Play();
        }
    }

    // 用于停止背景音乐
    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }
}
