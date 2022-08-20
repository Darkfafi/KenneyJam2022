using RaTweening;
using UnityEngine;

public class DisplayBase : MonoBehaviour
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

	protected virtual void Awake()
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

		OnOpen();

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

		OnClose();

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

	protected virtual void OnOpen()
	{
	}

	protected virtual void OnOpened()
	{
	
	}

	protected virtual void OnClose()
	{
	}

	protected virtual void OnClosed()
	{

	}
}
