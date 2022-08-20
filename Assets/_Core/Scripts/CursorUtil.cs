using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Utils/CursorUtil")]
public class CursorUtil : ScriptableObject
{
	[SerializeField]
	private Texture2D _defaultCursor = null;

	[SerializeField]
	private Texture2D _attackCursor = null;

	[SerializeField]
	private Texture2D _interactCursor = null;

	public void SetCursor(CursorType type)
	{
		Cursor.SetCursor(GetCursorTexture(type), Vector2.zero, CursorMode.Auto);
	}

	public Texture2D GetCursorTexture(CursorType type)
	{
		switch(type)
		{
			case CursorType.Default:
				return _defaultCursor;
			case CursorType.Attack:
				return _attackCursor;
			case CursorType.Interact:
				return _interactCursor;
		}
		return null;
	}

	public enum CursorType
	{
		Default,
		Attack,
		Interact,
	}
}
