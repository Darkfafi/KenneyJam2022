using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemElement : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Text _label;

	private ItemConfig _config = null;
	private KenneyJamGame _game = null;

	protected void OnDestroy()
	{
		if(_game == null)
		{
			return;
		}

		_game.Inventory.InventoryChangedEvent -= OnInventoryChanged;
		_game = null;
		_config = null;
	}

	public void Initialize(ItemConfig config, KenneyJamGame game)
	{
		if(_game != null)
		{
			return;
		}

		_config = config;
		_game = game;

		_game.Inventory.InventoryChangedEvent += OnInventoryChanged;
		RefreshItem();
	}

	public void UseItem()
	{
		_game.ConsumablesSystem.TryUseItem(_config);
	}

	private void RefreshItem()
	{
		_icon.sprite = _config.Icon;
		_label.text = $"{_game.Inventory.GetItemCount(_config)}/{_config.MaxStockAmount}";
	}

	private void OnInventoryChanged()
	{
		RefreshItem();
	}
}
