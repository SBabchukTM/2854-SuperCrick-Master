using System.Threading;
using Application.Services.Audio;
using Application.Services.UserData;
using Application.UI;
using Core.Services.Audio;
using Core.UI;
using Runtime.Application.ShopSystem;

namespace Runtime.Application.Services.Shop
{
    public class ProcessPurchaseService : IProcessPurchaseService
    {
        private readonly ShopService _shopService;
        private readonly IPurchaseEffectsService _purchaseEffectsService;
        private readonly ISelectPurchaseItemService _selectPurchaseItemService;
        private readonly IUiService _uiService;
        private readonly IShopItemsDisplayService _shopItemsDisplayService;
        private readonly IAudioService _audioService;
        private readonly IUserInventoryService _userInventoryService;

        private ShopSetup _shopItemStateConfig;

        public ProcessPurchaseService(ShopService shopService, IPurchaseEffectsService purchaseEffectsService, 
                ISelectPurchaseItemService selectPurchaseItemService, IUiService uiService, 
                IShopItemsDisplayService shopItemsDisplayService, IAudioService audioService, IUserInventoryService userInventoryService)
        {
            _shopService = shopService;
            _purchaseEffectsService = purchaseEffectsService;
            _selectPurchaseItemService = selectPurchaseItemService;
            _uiService = uiService;
            _shopItemsDisplayService = shopItemsDisplayService;
            _audioService = audioService;
            _userInventoryService = userInventoryService;
        }

        public void SetShopSetup(ShopSetup shopSetup) =>
                _shopItemStateConfig = shopSetup;

        public void ProcessPurchaseAttempt(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
        {
            var shopItemModel = shopItemDisplayView.GetShopItemModel();
            
            switch (shopItemModel.ItemState)
            {
                case ShopItemState.NotPurchased:
                    ProcessPurchase(shopItemDisplayView, cancellationToken);
                    break;
                case ShopItemState.Purchased:
                    SelectItem(shopItemModel);
                    UpdateStatus();
                    break;
                case ShopItemState.Selected:
                    UpdateStatus();
                    _userInventoryService.UpdateUsedGameItemID(shopItemDisplayView.GetShopItemModel().ShopItem.ItemID);
                    break;
            }
        }

        private async void ProcessPurchase(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
        {
            if(!_shopService.CanPurchaseItem(shopItemDisplayView.GetShopItemModel().ShopItem))
            {
                _purchaseEffectsService.PlayFailedPurchaseAttemptEffect(shopItemDisplayView, cancellationToken);
                return;
            }

            var popup = await _uiService
                    .ShowPopup(ConstPopups.ItemPurchasePopup, new ItemPurchasePopupData { ShopItem = shopItemDisplayView.GetShopItemModel().ShopItem },
                            cancellationToken) as ItemPurchasePopup;

            Subscribe(shopItemDisplayView, popup);
        }

        private void Subscribe(ShopItemDisplayView shopItemDisplayView, ItemPurchasePopup popup)
        {
            popup.OnAcceptPressedEvent += () => { OnAcceptButtonPressed(shopItemDisplayView, popup); };
            popup.OnDenyPressedEvent += () => OnDenyButtonPressed(shopItemDisplayView, popup);
        }

        private void SelectItem(ShopItemDisplayModel shopDisplayModel)
        {
            PlaySound(ConstAudio.SelectSound, _shopItemStateConfig.PurchaseEffectSettings.PlaySoundOnSelectPurchased);
            _selectPurchaseItemService.SelectPurchasedItem(shopDisplayModel);
        }

        private void PlaySound(string sound, bool condition)
        {
            if (condition)
                _audioService.PlaySound(sound);
        }

        private void UpdateStatus() =>
                _shopItemsDisplayService.UpdateItemsStatus();

        private static void DestroyPopup(ItemPurchasePopup popup) =>
                popup.DestroyPopup();

        private void OnDenyButtonPressed(ShopItemDisplayView shopItemDisplayView, ItemPurchasePopup popup)
        {
            popup.OnAcceptPressedEvent -= () => { OnAcceptButtonPressed(shopItemDisplayView, popup); };
            DestroyPopup(popup);
        }

        private void OnAcceptButtonPressed(ShopItemDisplayView shopItemDisplayView, ItemPurchasePopup popup)
        {
            _shopService.PurchaseShopItem(shopItemDisplayView);

            SelectItem(shopItemDisplayView.GetShopItemModel());
            PlaySound(ConstAudio.PurchaseSound, condition: _shopItemStateConfig.PurchaseEffectSettings.PlaySoundOnPurchase);
            UpdateStatus();
            DestroyPopup(popup);
        }
    }
}