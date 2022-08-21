using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinDisplay : DisplayBase
{
	[SerializeField]
	private Text _distanceTravelledLabel = null;

	[SerializeField]
	private Text _xpGainedLabel = null;

	[SerializeField]
	private Text _buttonLabel = null;

	[SerializeField]
	private float _startDuration = 1f;

	[SerializeField]
	private float _convertDuration = 3f;

	[Header("Audio")]
	[SerializeField]
	private MusicSystem _musicSystem = null;
	[SerializeField]
	private AudioClip _rewardClip = null;
	[SerializeField]
	private AudioClip _winClip = null;

	private KenneyJamGame _game = null;
	private Action _onClosed = null;
	private Coroutine _countdownRoutine = null;

	public void Init(KenneyJamGame game, Action onClosed)
	{
		if(_game != null)
		{
			return;
		}

		_game = game;
		_onClosed = onClosed;

		EndCountDown();
		_buttonLabel.text = "Skip";

		_distanceTravelledLabel.text = $"{0}m";
		_xpGainedLabel.text = $"{0}";
	}

	public void OnButtonPressed()
	{
		if(_countdownRoutine != null)
		{
			EndCountDown();
		}
		else
		{
			Close(false);
		}
	}

	protected override void OnOpened()
	{
		_musicSystem.MusicSource.PlayOneShot(_winClip);
		_countdownRoutine = StartCoroutine(CountDownRoutine());
	}

	protected override void OnClosed()
	{
		Action callback = _onClosed;
		_onClosed = null;
		callback?.Invoke();
	}

	private IEnumerator CountDownRoutine()
	{
		_buttonLabel.text = "Skip";

		yield return new WaitForSeconds(_startDuration);

		float duration = _convertDuration;
		float counter = 0f;
		float audioCounter = 0f;

		while(counter < duration)
		{
			float normalizedTime = counter / duration;

			_distanceTravelledLabel.text = $"{ Mathf.Lerp(0, _game.TotalDistanceTravelled, normalizedTime).ToString("0") }m";
			_xpGainedLabel.text = $"{Mathf.Lerp(0, _game.TotalXPGained, normalizedTime).ToString("0") }";

			counter = Mathf.Clamp(counter + Time.deltaTime, 0f, duration);

			audioCounter += Time.deltaTime;
			if(audioCounter > 0.1f)
			{
				audioCounter = 0f;
				_musicSystem.SFXSource.PlayOneShot(_rewardClip);
			}

			yield return null;
		}

		EndCountDown();
	}

	private void EndCountDown()
	{
		if(_countdownRoutine != null)
		{
			StopCoroutine(_countdownRoutine);
			_countdownRoutine = null;
		}

		_distanceTravelledLabel.text = $"{_game.TotalDistanceTravelled}m";
		_xpGainedLabel.text = $"{_game.TotalXPGained}";

		_buttonLabel.text = "End Run";
	}
}