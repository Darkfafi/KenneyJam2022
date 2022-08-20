using UnityEngine;
using static RewardDisplay;

public class RewardState : KenneyJamGameStateBase
{
	[SerializeField]
	private RewardDisplay _display = null;

	[SerializeField]
	private int _distanceCostPerXP = 3;

	protected void Awake()
	{
		_distanceCostPerXP = Mathf.Max(1, _distanceCostPerXP);
	}

	protected override void OnEnter()
	{
		int oldXP = StateParent.Inventory.XP;

		// Calculate Rewards
		int distance = Mathf.RoundToInt(StateParent.NavigationSystem.DistanceTravelled);

		XPGainer distanceXPGainer = XPGainer.Create("Distance", distance, distance / _distanceCostPerXP);

		// Apply XP Gain
		StateParent.Inventory.AddXP(distanceXPGainer.XPDelta);

		_display.Init(oldXP, new XPGainer[] { distanceXPGainer }, () => StateParent.GoToNextPhase());
		_display.Open();
	}

	protected override void OnExit()
	{
		_display.Close(true);
	}
}
