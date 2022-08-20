using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IChunkEntity, IInteractable
{
	#region Editor Variables

	[SerializeField]
	private ItemConfig _weakness = null;

	[SerializeField]
	private Character _character = null;

	[SerializeField]
	private float _speed = 5f;

	[SerializeField]
	private float _staminaDrain = 10f;

	[SerializeField]
	private float _viewRadius = 3f;

	#endregion

	#region Variables

	private WorldChunk _chunk = null;
	private bool _isAlive = false;

	#endregion

	#region Properties

	public InteractableType InteractableType
	{
		get; private set;
	}

	#endregion

	#region Lifecycle

	protected void Update()
	{
		if(_chunk != null && _isAlive)
		{
			Character player = _chunk.Game.Player;
			if(Vector2.Distance(player.transform.position, transform.position) < _viewRadius)
			{
				_character.SetState(Character.PlayerState.Walking);
				Vector2 delta = player.transform.position - transform.position;
				transform.Translate(delta.normalized * (Time.deltaTime * _speed));
			}
		}
	}

	protected void OnCollisionEnter2D(Collision2D collision)
	{
		if(_chunk != null && _isAlive)
		{
			if(collision.gameObject == _chunk.Game.Player.gameObject)
			{
				_chunk.Game.Stamina.ApplyDelta(-_staminaDrain);
				Kill();
			}
		}
	}

	protected void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, _viewRadius);
	}

	#endregion

	#region Public Methods

	public void Init(WorldChunk chunk)
	{
		_chunk = chunk;
		_character.SetState(Character.PlayerState.Idle);
		_isAlive = true;
		RefreshInteractibility();
	}

	public void Deinit()
	{
		_chunk = null;
		_isAlive = false;
		RefreshInteractibility();
	}

	public void Kill()
	{
		if(_isAlive)
		{
			_isAlive = false;
			_character.SetState(Character.PlayerState.Death);
			_character.Collider2D.enabled = false;
			RefreshInteractibility();
		}
	}

	public void Interact()
	{
		switch(InteractableType)
		{
			case InteractableType.Attack:
				Kill();
				break;
		}
	}

	#endregion

	#region Private Methods

	private void RefreshInteractibility()
	{
		InteractableType = InteractableType.None;
		
		if(!_isAlive)
		{
			return;
		}

		if(_chunk == null)
		{
			return;
		}

		if(!_chunk.Game.Inventory.HasItem(_weakness))
		{
			return;
		}

		InteractableType = InteractableType.Attack;
	}

	#endregion
}
