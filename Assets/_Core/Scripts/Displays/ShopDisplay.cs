using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDisplay : DisplayBase
{
	[SerializeField]
	private ShopItemElement[] _shopItems = null;

	[SerializeField]
	private LabelValueElement _xpLabel = null;

	[Header("Item Display")]
	[SerializeField]
	private Transform _itemDisplayContainer = null;
	[SerializeField]
	private Text _titleLabel;
	[SerializeField]
	private Image _icon = null;
	[SerializeField]
	private Text _typeLabel;
	[SerializeField]
	private Text _descriptionLabel;
	[SerializeField]
	private Text _costLabel;
	[SerializeField]
	private Text _stockLabel;
	[SerializeField]
	private Button _buyButton;

	private KenneyJamGame _game = null;
	private ShopItemElement _highlightedElement = null;
	private Action _onClosed = null;

	public void Init(KenneyJamGame game, Action onClosed)
	{
		if(_game != null)
		{
			return;
		}

		_game = game;
		_onClosed = onClosed;

		_game.Inventory.InventoryChangedEvent += RefreshXP;
		RefreshXP();
	}

	private void RefreshXP()
	{
		_xpLabel.SetNumberValue(_game.Inventory.XP);
	}

	public void BuyButtonPressed()
	{
		if(IsOpen && _highlightedElement != null && _highlightedElement.CanBuy)
		{
			_game.Inventory.DrainXP(_highlightedElement.Cost);
			_game.Inventory.AddItem(_highlightedElement.Config);
		}
	}

	protected override void OnOpen()
	{
		_highlightedElement = null;
		_itemDisplayContainer.gameObject.SetActive(false);
		for(int i = 0; i < _shopItems.Length; i++)
		{
			_shopItems[i].Init(_game, OnShopItemClicked);
		}
	}

	protected override void OnClose()
	{
		_itemDisplayContainer.gameObject.SetActive(false);
		_highlightedElement = null;
	}

	protected override void OnClosed()
	{
		for(int i = 0; i < _shopItems.Length; i++)
		{
			_shopItems[i].DeInit();
		}

		Action callback = _onClosed;

		_game.Inventory.InventoryChangedEvent -= RefreshXP;
		_game = null;
		_onClosed = null;

		callback?.Invoke();
	}

	private void OnShopItemClicked(ShopItemElement element)
	{
		if(IsOpen)
		{
			_highlightedElement = element;
			_itemDisplayContainer.gameObject.SetActive(true);
			_titleLabel.text = element.Config.ItemName;
			_icon.sprite = element.Config.Icon;
			_typeLabel.text = element.Config.SetType.ToString();
			_descriptionLabel.text = element.Config.Description;
			_costLabel.text = $"Cost: {element.PriceLabel.text}";
			_stockLabel.text = $"Stock: {element.StockLabel.text}";
			_buyButton.interactable = element.CanBuy;
		}
	}
}
