﻿using UnityEngine;
using RaTweening;

public class IntroState : KenneyJamGameStateBase
{
	#region Editor Variables

	[SerializeField]
	private Transform _startWalkPosition = null;

	#endregion

	protected override void OnEnter()
	{
		// Refresh Stats
		StateParent.Stamina.Refresh();
		StateParent.NavigationSystem.ResetLoop();

		// Animation
		StateParent.Player.transform.position = _startWalkPosition.position;
		StateParent.Player.SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

		RaTweenSequence.Create
		(
			StateParent.Player.SpriteRenderer.TweenColorA(1f, 0.5f)
				.SetDelay(0.5f)
				.ToSequenceEntry(0.7f),
			StateParent.Player.transform.TweenMove(StateParent.PlayerStartPosition, 0.8f)
				.OnStart(()=>StateParent.Player.SetState( Player.PlayerState.Walking))
				.OnComplete(() => StateParent.Player.SetState(Player.PlayerState.Idle))
				.ToSequenceEntry(),
			StateParent.Player.transform.TweenPunchPos(Vector3.up * 0.3f, 0.5f, vibrato: 3, elasticity: 0f)
				.SetDelay(0.3f)
				.ToSequenceEntry()
		).SetGroup(this).OnComplete(() => 
		{
			StateParent.GoToNextPhase();
		});
	}

	protected override void OnExit()
	{
		RaTweenBase.KillGroup(this);
		StateParent.Player.transform.position = StateParent.PlayerStartPosition;
	}
}
