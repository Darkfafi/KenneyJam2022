using UnityEngine;

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
				// Costs 5
				// 10 seconds of walking added
				// 10 * 1.5 = 15 xp worth of stamina, so + 11 xp
				// (or 15 xp worth of stamina on craft)
				Game.Stamina.ApplyDelta(10);
			}
			else if(item == _spFlaskBigConfig)
			{
				// Costs 10
				// 10 * 3 = 30 (30 seconds of walking added)
				// 30 * 1.5 = 45 xp worth of stamina, so + 15 xp on use
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