using System;
using UnityEngine;

public class RewardDisplay : DisplayBase
{
	[SerializeField]
	private KenneyJamGame _game = null;

	private int _oldXP = 0;
	private XPGainer[] _xpGainers = null;
	private Action _onClosed = null;

	public void Init(int oldXP, XPGainer[] xpGainers, Action onClosed)
	{
	
	}

	protected override void OnOpened()
	{

	}

	protected override void OnClosed()
	{
		Action callback = _onClosed;

		// Deinit
		_xpGainers = null;
		_oldXP = 0;
		_onClosed = null;

		// Callback
		callback?.Invoke();
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
