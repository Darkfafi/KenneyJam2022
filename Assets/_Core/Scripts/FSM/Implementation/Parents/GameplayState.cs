using UnityEngine;
using RaTweening;
using System.Collections;

public class GameplayState : KenneyJamGameStateBase
{
	[Header("Audio")]
	[SerializeField]
	private MusicSystem _musicSystem = null;

	[SerializeField]
	private AudioClip _outroClip = null;

	private Coroutine _endRoutine = null;

	public override void Initialize(KenneyJamGame parent)
	{
		base.Initialize(parent);

		parent.DisableBottomBar();
		parent.DisableItemsBar();

		parent.ConsumablesSystem.SetEnabled(false);
		parent.InteractionSystem.SetEnabled(false);
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

	protected override void OnEnter()
	{
		StateParent.NavigationSystem.ResumeLoop();
		StateParent.Player.SetState(Character.PlayerState.Walking);

		StateParent.EnableBottomBar();
		StateParent.EnableItemsBar();

		StateParent.ConsumablesSystem.SetEnabled(true);
		StateParent.InteractionSystem.SetEnabled(true);

		_musicSystem.StartSystem();
	}

	protected override void OnExit()
	{
		_musicSystem.StopSystem();

		StateParent.ConsumablesSystem.SetEnabled(false);
		StateParent.InteractionSystem.SetEnabled(false);

		StateParent.NavigationSystem.PauseLoop();

		StateParent.Player.SetState(Character.PlayerState.Idle);
		StateParent.Player.Collider2D.enabled = true;

		if(_endRoutine != null)
		{
			StopCoroutine(_endRoutine);
			_endRoutine = null;
		}

		StateParent.DisableBottomBar();
		StateParent.DisableItemsBar();
	}

	private IEnumerator DoEndRoutine()
	{
		_musicSystem.StopSystem();
		_musicSystem.AudioSource.PlayOneShot(_outroClip);

		StateParent.ConsumablesSystem.SetEnabled(false);
		StateParent.InteractionSystem.SetEnabled(false);

		StateParent.NavigationSystem.PauseLoop();

		StateParent.Player.SetState(Character.PlayerState.Death);
		StateParent.Player.Collider2D.enabled = false;

		yield return new WaitForSeconds(2.5f);
		_endRoutine = null;
		StateParent.GoToNextPhase();
	}
}
