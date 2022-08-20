using UnityEngine;

public class StatValue
{
	public float MaxValue
	{
		get; private set;
	}

	public float Value
	{
		get; private set;
	}

	public float NormalizedValue => Value / MaxValue;

	public bool IsEmpty => Mathf.Approximately(Value, 0f);

	public StatValue(float value)
	{
		SetMaxValue(value);
	}

	public void Refresh()
	{
		SetValue(MaxValue);
	}

	public void SetMaxValue(float maxValue)
	{
		Value = MaxValue = Mathf.Max(0f, maxValue);
	}

	public void SetValue(float value)
	{
		Value = Mathf.Clamp(value, 0, MaxValue);
	}

	public void ApplyDelta(float delta)
	{
		SetValue(Value + delta);
	}
}
