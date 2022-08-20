using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardDisplay : DisplayBase
{
	[SerializeField]
	private LabelValueElement _xpGainerPrefab = null;

	[SerializeField]
	private LabelValueElement _xpTarget = null;

	[SerializeField]
	private Text _buttonLabel = null;

	[SerializeField]
	private float _startDuration = 1f;

	[SerializeField]
	private float _convertDuration = 2f;

	private int _oldXP = 0;
	private int _newXP = 0;
	private Action _onClosed = null;
	private Coroutine _countdownRoutine = null;

	private Dictionary<XPGainer, LabelValueElement> _xpGainerDisplays = new Dictionary<XPGainer, LabelValueElement>();

	protected override void Awake()
	{
		base.Awake();
		_xpGainerPrefab.gameObject.SetActive(false);
	}

	public void Init(int oldXP, XPGainer[] xpGainers, Action onClosed)
	{
		Clear();

		_oldXP = oldXP;
		_newXP = _oldXP;

		_onClosed = onClosed;

		_xpTarget.SetNumberValue(oldXP);

		for(int i = 0; i < xpGainers.Length; i++)
		{
			XPGainer gainer = xpGainers[i];
			LabelValueElement display = Instantiate(_xpGainerPrefab, _xpGainerPrefab.transform.parent);
			display.gameObject.SetActive(true);
			display.Label.text = gainer.Label;
			display.SetNumberValue(gainer.Value);
			_xpGainerDisplays[gainer] = display;
			_newXP += gainer.XPDelta;
		}
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
		_countdownRoutine = StartCoroutine(CountDownRoutine());
	}

	protected override void OnClosed()
	{
		Action callback = _onClosed;

		// Deinit
		Clear();

		// Callback
		callback?.Invoke();
	}

	private void Clear()
	{
		EndCountDown();

		_buttonLabel.text = "Skip";

		foreach(var pair in _xpGainerDisplays)
		{
			Destroy(pair.Value.gameObject);
		}
		_xpGainerDisplays.Clear();

		_oldXP = 0;
		_newXP = 0;
		_onClosed = null;
	}

	private IEnumerator CountDownRoutine()
	{
		_buttonLabel.text = "Skip";

		yield return new WaitForSeconds(_startDuration);

		float duration = _convertDuration;
		float counter = 0f;

		while(counter < duration)
		{
			float normalizedTime = counter / duration;
			foreach(var pair in _xpGainerDisplays)
			{
				pair.Value.SetNumberValue(Mathf.Lerp(pair.Key.Value, 0, normalizedTime));
			}
			_xpTarget.SetNumberValue(Mathf.Lerp(_oldXP, _newXP, normalizedTime));
			counter = Mathf.Clamp(counter + Time.deltaTime, 0f, duration);
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

		foreach(var pair in _xpGainerDisplays)
		{
			pair.Value.SetNumberValue(0);
		}

		_xpTarget.SetNumberValue(_newXP);

		_buttonLabel.text = "Continue";
	}

	public struct XPGainer
	{
		public string Label;
		public int Value;
		public int XPDelta;

		public static XPGainer Create(string label, int value, int xp)
		{
			return new XPGainer()
			{
				Label = label,
				Value = value,
				XPDelta = xp
			};
		}
	}
}
