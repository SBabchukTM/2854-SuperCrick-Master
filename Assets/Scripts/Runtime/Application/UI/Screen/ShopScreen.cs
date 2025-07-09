using System.Collections.Generic;
using Application.Services.UserData;
using R3;
using Runtime.Application.ShopSystem;
using Runtime.Core.Extensions;
using TMPro;
using UnityEngine;
using Zenject;

namespace Application.UI
{
    public class ShopScreen : UiScreen
    {
        public readonly Subject<Unit> BackPressedSubject = new();
        
        [SerializeField] private SimpleButton _goBackButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private RectTransform _shopItemsParent;

        private IUserInventoryService _userInventoryService;
        
        [Inject]
        public void Construct(IUserInventoryService userInventoryService) =>
                _userInventoryService = userInventoryService;
        
        private void Start()
        {
            SubscribeToEvents();
            UpdateBalance(_userInventoryService.GetBalance());
        }

        public void SetShopItems(List<ShopItemDisplayView> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_shopItemsParent, false);
        }

        private void UpdateBalance(int balance) => 
                _balanceText.text = balance.ToString();

        private void SubscribeToEvents()
        {
            _goBackButton.Button.SubscribeButtonClick(BackPressedSubject);
            _userInventoryService.BalanceChangedSubject
                    .Subscribe(UpdateBalance)
                    .AddTo(this);
        }
    }
}