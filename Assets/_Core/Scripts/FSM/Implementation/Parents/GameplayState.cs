using UnityEngine;
using RaTweening;

public class GameplayState : KenneyJamGameStateBase
{
	protected override void OnEnter()
	{
		StateParent.NavigationSystem.ResumeLoop();
		StateParent.Player.SetState(Player.PlayerState.Walking);
	}

	protected void Update()
	{
		if(IsCurrentState && StateParent.NavigationSystem.IsRunning)
		{
			StateParent.Stamina.ApplyDelta(-Time.deltaTime);

			if(StateParent.Stamina.IsEmpty)
			{
				StateParent.GoToNextPhase();
			}
		}
	}

	protected override void OnExit()
	{
		StateParent.NavigationSystem.PauseLoop();
		StateParent.Player.SetState(Player.PlayerState.Idle);
	}
}
