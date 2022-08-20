using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KenneyJamGame : MonoBehaviour, IStatesParent
{
	#region Editor Variables

	[Header("Game")]
	[SerializeField]
	private WorldNavigationSystem _worldNavigationSystem = null;

	[SerializeField]
	private Player _player = null;

	[SerializeField]
	private KenneyJamGameStateBase[] _states = null;

	[Header("UI")]
	[SerializeField]
	private Text _distanceLabel = null;

	[SerializeField]
	private Image _staminaFillBar = null;

	#endregion

	#region Variables

	private FiniteStateMachine<KenneyJamGame> _fsm = null;

	#endregion

	#region Properties

	public WorldNavigationSystem NavigationSystem => _worldNavigationSystem;

	public Player Player => _player;

	public Vector3 PlayerStartPosition
	{
		get; private set;
	}

	// Stats
	public Inventory Inventory
	{
		get; private set;
	}

	public StatValue Stamina
	{
		get; private set;
	}

	#endregion

	#region Lifecycle

	protected void OnValidate()
	{
		_states = GetComponentsInChildren<KenneyJamGameStateBase>(false);
	}

	protected void Awake()
	{
		_fsm = new FiniteStateMachine<KenneyJamGame>(this, _states, false);

		Inventory = new Inventory();
		Stamina = new StatValue(0);

		_worldNavigationSystem.Initialize();

		PlayerStartPosition = Player.transform.position;
	}

	protected void Start()
	{
		_fsm.StartStateMachine();
	}

	protected void Update()
	{
		_distanceLabel.text = _worldNavigationSystem.DistanceTravelled.ToString("0") + "m";
		_staminaFillBar.transform.localScale = new Vector3(Stamina.NormalizedValue, 1f, 1f);
	}

	#endregion

	#region Public Methods

	public void GoToNextPhase()
	{
		if(!_fsm.GoToNextState())
		{
			_fsm.SetState(0);
		}
	}

	#endregion
}
