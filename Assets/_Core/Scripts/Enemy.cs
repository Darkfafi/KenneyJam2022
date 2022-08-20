using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaTweening;

public class Enemy : MonoBehaviour, IChunkChild
{
	[SerializeField]
	private Character _character = null;

	[SerializeField]
	private float _speed = 5f;

	[SerializeField]
	private float _staminaDrain = 10f;

	[SerializeField]
	private float _viewRadius = 3f;

	private WorldChunk _chunk = null;
	private bool _isAlive = false;

	public void Init(WorldChunk chunk)
	{
		_chunk = chunk;
		_character.SetState(Character.PlayerState.Idle);
		_isAlive = true;
	}

	public void Kill()
	{
		if(_isAlive)
		{
			_isAlive = false;
			_character.SetState(Character.PlayerState.Death);
			_character.Collider2D.enabled = false;
			//_character.SpriteRenderer.transform.TweenPunchScale(Vector2.one * 0.2f, 0.5f, elasticity: 0.2f);
			//_character.SpriteRenderer.TweenColorA(0, 0.7f).SetDelay(0.1f);
		}
	}

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

	public void Deinit()
	{
		_chunk = null;
		_isAlive = false;
	}
}
