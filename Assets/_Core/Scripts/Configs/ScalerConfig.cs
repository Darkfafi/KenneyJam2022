using UnityEngine;

[CreateAssetMenu(menuName = "Configs/ScalerConfig")]
public class ScalerConfig : ScriptableObject
{
	[SerializeField]
	private int _startValue = 0;

	[SerializeField]
	private int _valuePerStep = 10;

	public int GetValue(int amount)
	{
		return _startValue + (_valuePerStep * amount);
	}
}
