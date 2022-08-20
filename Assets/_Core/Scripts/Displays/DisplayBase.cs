using RaTweening;
using UnityEngine;

public abstract class DisplayBase : MonoBehaviour
{
	[SerializeField]
	private RaTweenerComponent _openAnimation = null;

	[SerializeField]
	private RaTweenerComponent _closeAnimation = null;

	[SerializeField]
	private Transform _content = null;

	public bool IsOpen
	{
		get; private set;
	}

	protected void Awake()
	{
		_content.gameObject.SetActive(false);
	}

	public void Open()
	{
		if(IsOpen)
		{
			return;
		}

		IsOpen = true;

		RaTweenBase.CompleteGroup(this);

		_content.gameObject.SetActive(true);
		_openAnimation.Play().OnComplete(() => OnOpened()).SetGroup(this);
	}

	public void Close(bool instant)
	{
		if(!IsOpen)
		{
			return;
		}

		IsOpen = false;

		RaTweenBase.CompleteGroup(this);

		if(instant)
		{
			OnClosed();
			_content.gameObject.SetActive(false);
		}
		else
		{

			_closeAnimation.Play().OnComplete(() =>
			{
				OnClosed();
				_content.gameObject.SetActive(false);
			}).SetGroup(this);
		}
	}


	protected abstract void OnOpened();
	protected abstract void OnClosed();
}
