using UnityEngine;
using RaTweening;

public class IntroState : KenneyJamGameStateBase
{
	#region Editor Variables

	[SerializeField]
	private Transform _startWalkPosition = null;

	[Header("Configs")]
	[SerializeField]
	private ItemConfig _staminaConfig = null;

	[SerializeField]
	private ScalerConfig _staminaScaler = null;

	[Header("Audio")]
	[SerializeField]
	private MusicSystem _musicSystem = null;

	[SerializeField]
	private AudioClip _introClip = null;

	#endregion

	public override void Initialize(KenneyJamGame parent)
	{
		base.Initialize(parent);
		parent.DisableMapBar();
	}

	protected override void OnEnter()
	{
		StateParent.DisableMapBar();

		// Apply Stats
		StateParent.RefreshItemsBar();
		StateParent.Stamina.SetMaxValue(_staminaScaler.GetValue(StateParent.Inventory.GetItemCount(_staminaConfig)));

		// Refresh Stats
		StateParent.Stamina.Refresh();
		StateParent.NavigationSystem.ResetLoop();

		// Animation
		StateParent.Player.transform.position = _startWalkPosition.position;
		StateParent.Player.SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

		RaTweenSequence.Create
		(
			StateParent.Player.SpriteRenderer.TweenColorA(1f, 0.5f)
				.ToSequenceEntry(0.7f),
			StateParent.Player.transform.TweenMove(StateParent.PlayerStartPosition, 0.9f)
				.OnStart(()=>StateParent.Player.SetState( Character.PlayerState.Walking))
				.OnComplete(() => StateParent.Player.SetState(Character.PlayerState.Idle))
				.ToSequenceEntry(),
			RaTweenSequence.EntryData.Create(() => StateParent.EnableMapBar()),
			StateParent.Player.transform.TweenPunchPos(Vector3.up * 0.3f, 0.5f, vibrato: 3, elasticity: 0f)
				.SetDelay(0.3f)
				.OnStart(()=> _musicSystem.StartSystem())
				.ToSequenceEntry()
		).SetGroup(this).OnComplete(() => 
		{
			StateParent.GoToNextPhase(false);
		}).OnStart(() => 
		{
			_musicSystem.MusicSource.PlayOneShot(_introClip);
		}).SetDelay(0.5f);
	}

	protected override void OnExit()
	{
		RaTweenBase.KillGroup(this);
		StateParent.Player.transform.position = StateParent.PlayerStartPosition;
	}
}
