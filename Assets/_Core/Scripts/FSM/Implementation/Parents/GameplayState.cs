public class GameplayState : KenneyJamGameStateBase
{
	protected override void OnEnter()
	{
		StateParent.NavigationSystem.ResumeLoop();
		StateParent.Player.SetState(Player.PlayerState.Walking);
	}

	protected override void OnExit()
	{
		StateParent.NavigationSystem.PauseLoop();
		StateParent.Player.SetState(Player.PlayerState.Idle);
	}
}
