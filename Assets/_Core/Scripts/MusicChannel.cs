using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Music")]
public class MusicChannel : ScriptableObject
{
	private List<MusicSystem> _systems = new List<MusicSystem>();

	public void Register(MusicSystem system)
	{
		if(!_systems.Contains(system))
		{
			_systems.Add(system);
		}
	}

	public void Unregister(MusicSystem system)
	{
		_systems.Remove(system);
	}

	public void PlaySFX(AudioClip clip)
	{
		if(TryGetMusicSystem(out MusicSystem system))
		{
			system.SFXSource.PlayOneShot(clip);
		}
	}

	public bool TryGetMusicSystem(out MusicSystem system)
	{
		for(int i = 0; i < _systems.Count; i++)
		{
			system = _systems[i];
			if(system != null)
			{
				return true;
			}
		}

		system = null;
		return false;
	}
}
