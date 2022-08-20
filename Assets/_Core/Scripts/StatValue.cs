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
		MaxValue = Value = value;
	}

	public void Refresh()
	{
		SetValue(MaxValue);
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