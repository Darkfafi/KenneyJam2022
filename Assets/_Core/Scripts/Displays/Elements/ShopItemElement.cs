using UnityEngine;
using UnityEngine.UI;

public class ShopItemElement : MonoBehaviour
{
	[SerializeField]
	private KenneyJamGame _kenneyJamGame = null;

	[SerializeField]
	private ItemConfig _config = null;

	[SerializeField]
	private Image _icon = null;

	[SerializeField]
	private Text _priceLabel = null;

	[SerializeField]
	private Text _stockLabel = null;

	public ItemConfig Config => _config;
	public Text PriceLabel => _priceLabel;
	public Text StockLabel => _stockLabel;

	protected void Awake()
	{
		_kenneyJamGame.Inventory.InventoryChangedEvent += RefreshItem;
		RefreshItem();
	}

	protected void OnDestroy()
	{
		_kenneyJamGame.Inventory.InventoryChangedEvent -= RefreshItem;
	}

	private void RefreshItem()
	{
		_icon.sprite = _config.Icon;

		int inStockAmount = _kenneyJamGame.Inventory.GetItemCount(_config);
		_priceLabel.text = _config.GetCost(inStockAmount) + " XP";
		_stockLabel.text = inStockAmount + "/" + _config.MaxStockAmount;
	}
}
