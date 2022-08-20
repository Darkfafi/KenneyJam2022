using UnityEngine;
using RaTweening;
using System.Collections;

public class GameplayState : KenneyJamGameStateBase
{
	private Coroutine _endRoutine = null;

	protected override void OnEnter()
	{
		StateParent.NavigationSystem.ResumeLoop();
		StateParent.Player.SetState(Character.PlayerState.Walking);
	}

	protected void Update()
	{
		if(IsCurrentState && 
			StateParent.NavigationSystem.IsRunning && 
			_endRoutine == null)
		{
			StateParent.Stamina.ApplyDelta(-Time.deltaTime);

			if(StateParent.Stamina.IsEmpty)
			{
				_endRoutine = StartCoroutine(DoEndRoutine());
			}
		}
	}

	protected override void OnExit()
	{
		StateParent.NavigationSystem.PauseLoop();
		StateParent.Player.SetState(Character.PlayerState.Idle);
		StateParent.Player.Collider2D.enabled = true;

		if(_endRoutine != null)
		{
			StopCoroutine(_endRoutine);
			_endRoutine = null;
		}
	}

	private IEnumerator DoEndRoutine()
	{
		StateParent.NavigationSystem.PauseLoop();
		StateParent.Player.SetState(Character.PlayerState.Death);
		StateParent.Player.Collider2D.enabled = false;

		yield return new WaitForSeconds(2.5f);
		_endRoutine = null;
		StateParent.GoToNextPhase();
	}
}
