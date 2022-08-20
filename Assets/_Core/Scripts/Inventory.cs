using UnityEngine;

public class Inventory
{
	public int XP
	{
		get; private set;
	}

	public void AddXP(int amount)
	{
		amount = Mathf.Max(0, amount);
		XP += amount;
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
			return true;
		}
		return false;
	}
}