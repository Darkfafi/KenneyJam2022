using System.Collections;
using UnityEngine;
using RaTweening;

public class MusicSystem : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioSource _internalSource = null;
    [SerializeField]
    private AudioSource _musicSource = null;
    [SerializeField]
    private AudioSource _sfxSource = null;

    [SerializeField]
    private AudioClip[] _clips = null;

    [SerializeField]
    private MusicChannel _channel = null;

    private Coroutine _systemRoutine = null;

    public bool IsRunning => _systemRoutine != null;

    public AudioSource MusicSource => _musicSource;
    public AudioSource SFXSource => _sfxSource;

	protected void Awake()
	{
        _channel.Register(this);
    }

	protected void OnDestroy()
	{
        _channel.Unregister(this);
	}

	public void StartSystem()
    {
        if(IsRunning)
        {
            return;
        }

        _musicSource.TweenVolume(0f, 0.35f);
        _systemRoutine = StartCoroutine(DoSystemRoutine());
    }

	public void StopSystem()
    {
        if(!IsRunning)
        {
            return;
        }

        StopCoroutine(_systemRoutine);
        _systemRoutine = null;

        _internalSource.Stop();
        _internalSource.clip = null;

        _musicSource.TweenVolume(default, 0.1f)
            .SetEndRef(_internalSource);
    }

    private IEnumerator DoSystemRoutine()
    {
        int index = 0;
        _internalSource.loop = false;

        while(true)
        {
            _internalSource.clip = _clips[index % _clips.Length];
            _internalSource.Play();
            index++;
            yield return new WaitForSeconds(_internalSource.clip.length);
        }
    }
}
