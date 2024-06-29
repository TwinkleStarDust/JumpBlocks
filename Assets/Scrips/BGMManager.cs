using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    public AudioSource bgmAudioSource; // ����������ƵԴ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �ڼ����³���ʱ�����ٴ˶���
        }
        else
        {
            Destroy(gameObject); // ����Ѿ���һ��BGMManagerʵ�����������´����Ķ���
        }
    }

    // ���ڲ��ű�������
    public void PlayBGM()
    {
        if (!bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Play();
        }
    }

    // ����ֹͣ��������
    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }
}
