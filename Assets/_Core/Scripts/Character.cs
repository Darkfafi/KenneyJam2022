using RaTweening;
using UnityEngine;

public class Character : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private SpriteRenderer _spriteRenderer = null;

	[SerializeField]
	private RaTweenerComponent _idleAnimation = null;

	[SerializeField]
	private RaTweenerComponent _walkAnimation = null;

	[SerializeField]
	private RaTweenerComponent _deathAnimation = null;

	[SerializeField]
	private Rigidbody2D _rigidbody2D = null;

	[SerializeField]
	private Collider2D _collider2D = null;

	#endregion

	#region Properties

	public SpriteRenderer SpriteRenderer => _spriteRenderer;

	public Rigidbody2D Rigidbody2D => _rigidbody2D;

	public Collider2D Collider2D => _collider2D;

	public PlayerState State
	{
		get; private set;
	}

	#endregion

	#region Lifecycle

	protected void Awake()
	{
		State = PlayerState.Idle;
		GetStateComponent(State).Play();
	}

	#endregion

	#region Public Methods

	public void SetState(PlayerState state)
	{
		if(State != state)
		{
			SpriteRenderer.transform.localScale = Vector3.one;
			SpriteRenderer.color = Color.white;

			GetStateComponent(State)?.Stop(true);
			State = state;
			GetStateComponent(State)?.Play();
		}
	}

	#endregion

	#region Private Methods

	private RaTweenerComponent GetStateComponent(PlayerState state)
	{
		switch(state)
		{
			case PlayerState.None:
				return null;
			case PlayerState.Idle:
				return _idleAnimation;
			case PlayerState.Walking:
				return _walkAnimation;
			case PlayerState.Death:
				return _deathAnimation;
		}
		throw new System.NotImplementedException($"State {state} does not have a RaTweenAnimation Implmentation!");
	}

	#endregion

	#region Nested

	public enum PlayerState
	{
		None		= 0,
		Idle		= 1,
		Walking		= 2,
		Death		= 3,
	}

	#endregion
}
