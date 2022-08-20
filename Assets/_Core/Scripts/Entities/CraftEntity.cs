using UnityEngine;

public class CraftEntity : MonoBehaviour, IChunkEntity, IInteractable
{
	[SerializeField]
	private ItemConfig _requirement = null;

	[SerializeField]
	private ItemConfig _gain = null;

	[SerializeField]
	private GameObject _objectBeforeCraft;

	[SerializeField]
	private GameObject _objectAfterCraft;

	public bool IsConsumed
	{
		get; private set;
	}

	public InteractableType InteractableType
	{
		get
		{
			InteractableType type = InteractableType.None;

			if(IsConsumed)
			{
				return type;
			}

			if(_chunk == null)
			{
				return type;
			}

			if(_requirement != null && !_chunk.Game.Inventory.HasItem(_requirement))
			{
				return type;
			}

			if(!_chunk.Game.Inventory.CanAddItem(_gain))
			{
				return type;
			}

			return InteractableType.Interactable;
		}
	}

	private WorldChunk _chunk = null;

	public void Init(WorldChunk chunk)
	{
		_chunk = chunk;
		SetConsumed(false);
	}

	public void Deinit()
	{
		_chunk = null;
	}

	public void Interact()
	{
		switch(InteractableType)
		{
			case InteractableType.Interactable:
				if(_requirement != null && _requirement.SpecifiedType == ItemConfig.ItemType.Item)
				{
					if(_chunk.Game.Inventory.DrainItem(_requirement))
					{
						if(_chunk.Game.Inventory.AddItem(_gain))
						{
							SetConsumed(true);
						}
					}
				}
				else
				{
					if(_chunk.Game.Inventory.AddItem(_gain))
					{
						SetConsumed(true);
					}
				}
				break;
		}
	}

	private void SetConsumed(bool value)
	{
		IsConsumed = value;

		if(_objectBeforeCraft)
		{
			_objectBeforeCraft.SetActive(!IsConsumed);
		}

		if(_objectAfterCraft)
		{
			_objectAfterCraft.SetActive(IsConsumed);
		}

		if(IsConsumed)
		{
			_chunk.Game.RefreshItemsBar();
		}
	}
}
