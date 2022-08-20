using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory
{
	public event Action InventoryChangedEvent;

	private Dictionary<ItemConfig, int> _items = new Dictionary<ItemConfig, int>();

	public int XP
	{
		get; private set;
	}

	// XP

	public void AddXP(int amount)
	{
		amount = Mathf.Max(0, amount);
		XP += amount;
		InventoryChangedEvent?.Invoke();
	}

	public bool CanAffordXP(int amount)
	{
		amount = Mathf.Max(0, amount);
		return XP >= amount;
	}

	public bool DrainXP(int amount)
	{
		amount = Mathf.Max(0, amount);
		if(CanAffordXP(amount))
		{
			XP -= amount;
			InventoryChangedEvent?.Invoke();
			return true;
		}
		return false;
	}

	// Items

	public int GetItemCount(ItemConfig config)
	{
		if(_items.TryGetValue(config, out int count))
		{
			return count;
		}
		return 0;
	}

	public bool HasItem(ItemConfig config) => GetItemCount(config) > 0;

	public bool CanAddItem(ItemConfig config)
	{
		return GetItemCount(config) < config.MaxStockAmount;
	}

	public bool AddItem(ItemConfig config)
	{
		if(CanAddItem(config))
		{
			_items.TryGetValue(config, out int value);
			_items[config] = value + 1;
			InventoryChangedEvent?.Invoke();
			return true;
		}
		return false;
	}

	public bool CanDrainItem(ItemConfig config)
	{
		return HasItem(config);
	}

	public bool DrainItem(ItemConfig config)
	{
		if(CanDrainItem(config))
		{
			_items.TryGetValue(config, out int value);
			_items[config] = value - 1;
			InventoryChangedEvent?.Invoke();
			return true;
		}
		return false;
	}
}