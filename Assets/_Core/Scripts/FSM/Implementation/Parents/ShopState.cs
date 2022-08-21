using UnityEngine;

public class ShopState : KenneyJamGameStateBase
{
	[SerializeField]
	private ShopDisplay _display = null;

	protected override void OnEnter()
	{
		_display.Init(StateParent, () => StateParent.GoToNextPhase(false));
		_display.Open();
	}

	protected override void OnExit()
	{
		_display.Close(true);
	}
}
