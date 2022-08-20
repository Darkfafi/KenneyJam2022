using UnityEngine;

[CreateAssetMenu(menuName = "Configs/ItemConfig")]
public class ItemConfig : ScriptableObject
{
	[SerializeField]
	private string _name = "";

	[SerializeField, TextArea]
	private string _description = "";

	[SerializeField]
	private Sprite _icon = null;

	[SerializeField]
	private int[] _costs = new int[] { 1 };

	[SerializeField]
	private int _maxStockAmount = 3;

	[SerializeField]
	private ItemType _type = ItemType.Item;

	public string ItemName => _name;
	public string Description => _description;
	public Sprite Icon => _icon;
	public ItemType SpecifiedType => _type;
	public int MaxStockAmount => _maxStockAmount;

	public int GetCost(int currentStock)
	{
		int index = Mathf.Clamp(currentStock, 0, _costs.Length - 1);
		return _costs[index];
	}

	public enum ItemType
	{
		Item,
		Upgrade
	}
}
