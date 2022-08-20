using RaTweening;
using UnityEngine;

public class Player : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private SpriteRenderer _spriteRenderer = null;

	[SerializeField]
	private RaTweenerComponent _idleAnimation = null;

	[SerializeField]
	private RaTweenerComponent _walkAnimation = null;

	#endregion

	#region Variables

	private bool _isInitialized = false;

	#endregion

	#region Properties

	public SpriteRenderer SpriteRenderer => _spriteRenderer;

	public PlayerState State
	{
		get; private set;
	}

	public StatValue Stamina
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

	public void Initialize(int staminaStartValue)
	{
		if(_isInitialized)
		{
			return;
		}

		Stamina = new StatValue(staminaStartValue);

		_isInitialized = true;
	}

	public void SetState(PlayerState state)
	{
		if(State != state)
		{
			GetStateComponent(State).Stop(true);
			State = state;
			GetStateComponent(State).Play();
		}
	}

	#endregion

	#region Private Methods

	private RaTweenerComponent GetStateComponent(PlayerState state)
	{
		switch(state)
		{
			case PlayerState.Idle:
				return _idleAnimation;
			case PlayerState.Walking:
				return _walkAnimation;
		}
		throw new System.NotImplementedException($"State {state} does not have a RaTweenAnimation Implmentation!");
	}

	#endregion

	#region Nested

	public enum PlayerState
	{
		Idle		= 1,
		Walking		= 2
	}

	#endregion
}
