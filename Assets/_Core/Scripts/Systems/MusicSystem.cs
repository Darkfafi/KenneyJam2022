using System.Collections;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioSource _audioSource = null;

    [SerializeField]
    private AudioClip[] _clips = null;

    private Coroutine _systemRoutine = null;

    public bool IsRunning => _systemRoutine != null;

    public AudioSource AudioSource => _audioSource;

    public void StartSystem()
    {
        if(IsRunning)
        {
            return;
        }

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
        
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    private IEnumerator DoSystemRoutine()
    {
        int index = 0;
        _audioSource.loop = false;

        while(true)
        {
            _audioSource.clip = _clips[index % _clips.Length];
            _audioSource.Play();
            index++;
            yield return new WaitForSeconds(_audioSource.clip.length);
        }
    }
}
