using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemElement : MonoBehaviour
{

	[SerializeField]
	private ItemConfig _config = null;

	[SerializeField]
	private Image _icon = null;

	[SerializeField]
	private Text _priceLabel = null;

	[SerializeField]
	private Text _stockLabel = null;

	private KenneyJamGame _kenneyJamGame = null;
	private Action<ShopItemElement> _onClickCallback = null;

	public ItemConfig Config => _config;
	public Text PriceLabel => _priceLabel;
	public Text StockLabel => _stockLabel;

	public bool CanBuy
	{
		get; private set;
	}

	public int Cost
	{
		get; private set;
	}

	public void Init(KenneyJamGame game, Action<ShopItemElement> onClickCallback)
	{
		if(_kenneyJamGame != null)
		{
			return;
		}

		_kenneyJamGame = game;
		_onClickCallback = onClickCallback;

		_kenneyJamGame.Inventory.InventoryChangedEvent += RefreshItem;
		RefreshItem();
	}

	public void DeInit()
	{
		if(_kenneyJamGame == null)
		{
			return;
		}

		_kenneyJamGame.Inventory.InventoryChangedEvent -= RefreshItem;
		_kenneyJamGame = null;

		_onClickCallback = null;
	}

	protected void OnDestroy()
	{
		DeInit();
	}

	public void OnClick()
	{
		_onClickCallback?.Invoke(this);
	}

	private void RefreshItem()
	{
		_icon.sprite = _config.Icon;

		int inStockAmount = _kenneyJamGame.Inventory.GetItemCount(_config);
		Cost = _config.GetCost(inStockAmount);
		_priceLabel.text = Cost + " XP";
		_stockLabel.text = inStockAmount + "/" + _config.MaxStockAmount;
		CanBuy = _kenneyJamGame.Inventory.CanAddItem(_config) && _kenneyJamGame.Inventory.CanAffordXP(Cost);
	}
}
