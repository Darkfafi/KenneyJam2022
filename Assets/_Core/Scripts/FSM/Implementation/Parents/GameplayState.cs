using UnityEngine;

public class GameplayState : KenneyJamGameStateBase
{
	protected override void OnEnter()
	{
		StateParent.NavigationSystem.ResumeLoop();
		StateParent.Player.SetState(Player.PlayerState.Walking);
	}

	protected void Update()
	{
		if(IsCurrentState)
		{
			StateParent.Player.Stamina.ApplyDelta(-Time.deltaTime);

			if(StateParent.Player.Stamina.IsEmpty)
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
