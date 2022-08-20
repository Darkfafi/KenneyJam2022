using UnityEngine;

public class ConsumablesSystem : MonoBehaviour
{
	[SerializeField]
	private ItemConfig _spFlashConfig = null;

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
				Game.Stamina.ApplyDelta(10f);
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


