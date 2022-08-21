using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : KenneyJamGameStateBase
{
	[SerializeField]
	private WinDisplay _display = null;

	protected override void OnEnter()
	{
		int distance = Mathf.RoundToInt(StateParent.NavigationSystem.DistanceTravelled);
		StateParent.RecordDistanceTravelled(distance);

		_display.Init(StateParent, () => 
		{
			SceneManager.LoadScene(0);
		});

		_display.Open();
	}

	protected override void OnExit()
	{
		_display.Close(true);
	}
}
