using System;
using UnityEngine;
using UnityEngine.UI;

public class LabelValueElement : MonoBehaviour
{
	[SerializeField]
	private Text _label = null;

	[SerializeField]
	private Text _value = null;

	public float NumberValue
	{
		get; private set;
	}

	public Text Label => _label;
	public Text Value => _value;

	public void SetNumberValue(float value)
	{
		NumberValue = value;
		Value.text = NumberValue.ToString("0");
	}
}
