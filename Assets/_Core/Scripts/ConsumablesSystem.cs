﻿using UnityEngine;

public class ConsumablesSystem : MonoBehaviour
{
	[SerializeField]
	private ItemConfig _spFlashConfig = null;

	[SerializeField]
	private ItemConfig _spFlaskBigConfig = null;

	public KenneyJamGame Game
	{
		get; private set;
	}

	public bool IsEnabled
	{
		get; private set;
	}

	public void Initialize(KenneyJamGame game)
	{
		if(Game != null)
		{
			return;
		}

		Game = game;
		IsEnabled = true;
	}

	public void SetEnabled(bool enabled)
	{
		IsEnabled = enabled;
	}

	public bool TryUseItem(ItemConfig item)
	{
		if(Game == null || !IsEnabled)
		{
			return false;
		}

		if(Game.Inventory.DrainItem(item))
		{
			if(item == _spFlashConfig)
			{
				// 4 * 2.5 = 10 (14 seconds of walking added)
				// Anything above * 3 is a positive loop
				// Can't have a positive feedback loop because they can be crafted. 
				Game.Stamina.ApplyDelta(_spFlashConfig.GetCost(0) * 2.5f);
			}
			else if(item == _spFlaskBigConfig)
			{
				// 10 * 3 = 30 (30 seconds of walking added)
				Game.Stamina.ApplyDelta(_spFlaskBigConfig.GetCost(0) * 3f);
			}
			else
			{
				throw new System.NotImplementedException($"Item {item} not implemented");
			}

			return true;
		}

		return false;
	}
}