using UnityEngine;

public class WorldInteractionSystem : MonoBehaviour
{
	[SerializeField]
	private CursorUtil _cursorUtil = null;

	public KenneyJamGame Game
	{
		get; private set;
	}

	public bool IsEnabled
	{
		get; private set;
	}

	public void Initialize(KenneyJamGame game)
	{
		if(Game != null)
		{
			return;
		}

		Game = game;
		_cursorUtil.SetCursor(CursorUtil.CursorType.Default);
		IsEnabled = true;
	}

	public void SetEnabled(bool enabled)
	{
		IsEnabled = enabled;
	}

	protected void Update()
	{
		if(!IsEnabled)
		{
			return;
		}

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if(hit.collider != null && hit.collider.transform.GetComponentInChildren<IInteractable>() is IInteractable interactable)
		{
			switch(interactable.InteractableType)
			{
				case InteractableType.Attack:
					_cursorUtil.SetCursor(CursorUtil.CursorType.Attack);
					break;
				case InteractableType.Interactable:
					_cursorUtil.SetCursor(CursorUtil.CursorType.Interact);
					break;
				case InteractableType.None:
					_cursorUtil.SetCursor(CursorUtil.CursorType.Default);
					break;
			}

			if(Input.GetMouseButtonDown(0))
			{
				interactable.Interact();
			}
		}
		else
		{
			_cursorUtil.SetCursor(CursorUtil.CursorType.Default);
		}
	}
}


