using System;
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
	private WorldInteractionSystem _worldInteractionSystem = null;
	[SerializeField]
	private ConsumablesSystem _consumablesSystem = null;

	[SerializeField]
	private Character _player = null;

	[SerializeField]
	private KenneyJamGameStateBase[] _states = null;

	[SerializeField]
	private int _loopIndex = 0;

	[SerializeField]
	private WinState _winState = null;

	[Header("UI Bottom Bar")]
	[SerializeField]
	private RectTransform _bottomBarContainer = null;
	[SerializeField]
	private Text _distanceLabel = null;

	[SerializeField]
	private Image _staminaFillBar = null;

	[Header("UI Items")]
	[SerializeField]
	private RectTransform _itemsContainer = null;
	[SerializeField]
	private ItemElement _itemElementPrefab = null;


	[Header("UI Map")]
	[SerializeField]
	private RectTransform _mapContainer = null;
	[SerializeField]
	private Slider _mapSlider = null;

	#endregion

	#region Variables

	private FiniteStateMachine<KenneyJamGame> _fsm = null;
	private List<ItemElement> _itemElements = new List<ItemElement>();
	private bool _itemBarEnabled = false;

	#endregion

	#region Properties

	public WorldNavigationSystem NavigationSystem => _worldNavigationSystem;
	public WorldInteractionSystem InteractionSystem => _worldInteractionSystem;
	public ConsumablesSystem ConsumablesSystem => _consumablesSystem;

	public Character Player => _player;

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

	public int TotalDistanceTravelled
	{
		get; private set;
	}

	public int TotalXPGained
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
		_itemElementPrefab.gameObject.SetActive(false);

		_fsm = new FiniteStateMachine<KenneyJamGame>(this, _states, false);

		Inventory = new Inventory();
		Stamina = new StatValue(0);

		_worldNavigationSystem.Initialize(this);
		_worldInteractionSystem.Initialize(this);
		_consumablesSystem.Initialize(this);

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

	public void RecordDistanceTravelled(int amount)
	{
		TotalDistanceTravelled += amount;
	}

	public void RecordXPGained(int amount)
	{
		TotalXPGained += amount;
	}

	public void GoToNextPhase(bool win)
	{
		if(win)
		{
			_fsm.SetState(_winState);
		}
		else if(_fsm.CurrentStateIndex >= _loopIndex)
		{
			_fsm.SetState(0);
		}
		else
		{
			_fsm.GoToNextState();
		}
	}

	public void RefreshItemsBar()
	{
		for(int i = 0; i < _itemElements.Count; i++)
		{
			Destroy(_itemElements[i].gameObject);
		}

		_itemElements.Clear();

		foreach(var item in Inventory.GetItemList(x => x.SpecifiedType == ItemConfig.ItemType.Item))
		{
			ItemElement itemElement = Instantiate(_itemElementPrefab, _itemElementPrefab.transform.parent);
			itemElement.gameObject.SetActive(true);
			itemElement.Initialize(item, this);
			_itemElements.Add(itemElement);
		}

		if(_itemBarEnabled)
		{
			_itemsContainer.gameObject.SetActive(_itemElements.Count > 0);
		}
	}

	public void EnableMapBar()
	{
		_mapSlider.value = 0;
		_mapContainer.pivot = new Vector2(_bottomBarContainer.pivot.x, 0f);
		_mapContainer.gameObject.SetActive(true);
	}

	public void DisableMapBar()
	{
		_mapContainer.gameObject.SetActive(false);
	}

	public void EnableItemsBar()
	{
		_itemBarEnabled = true;
		_itemsContainer.pivot = new Vector2(1f, _itemsContainer.pivot.y);
		_itemsContainer.gameObject.SetActive(_itemElements.Count > 0);
	}

	public void DisableItemsBar()
	{
		_itemBarEnabled = false;
		_itemsContainer.gameObject.SetActive(false);
	}

	public void EnableBottomBar()
	{
		_bottomBarContainer.pivot = new Vector2(_bottomBarContainer.pivot.x, 1f);
		_bottomBarContainer.gameObject.SetActive(true);
	}

	public void DisableBottomBar()
	{
		_bottomBarContainer.gameObject.SetActive(false);
	}

	#endregion
}
