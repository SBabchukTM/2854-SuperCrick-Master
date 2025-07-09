using System.Linq;
using Application.Services.UserData;
using Core;

namespace Runtime.Application.ShopSystem
{
    public class ShopService 
    {
        private readonly IUserInventoryService _userInventoryService;
        private readonly ISettingProvider _settingProvider;

        private ShopSetup _shopSetup;

        public ShopService(IUserInventoryService userInventoryService, ISettingProvider settingProvider)
        {
            _userInventoryService = userInventoryService;
            _settingProvider = settingProvider;
        }

        public ShopItem GetUsedShopItem() =>
                _settingProvider
                        .Get<ShopSetup>().ShopItems
                        .First(x => x.ItemID == _userInventoryService.GetUsedGameItemID());

        public void PurchaseShopItem(ShopItemDisplayView shopItemDisplayView)
        {
            _userInventoryService.AddPurchasedGameItemID(shopItemDisplayView.GetShopItemModel().ShopItem.ItemID);
            _userInventoryService.UpdateUsedGameItemID(shopItemDisplayView.GetShopItemModel().ShopItem.ItemID);
            _userInventoryService.UpdateBalance(_userInventoryService.GetBalance() - shopItemDisplayView.GetShopItemModel().ShopItem.ItemPrice);
        }

        public bool CanPurchaseItem(ShopItem shopItem) => _userInventoryService.GetBalance() >= shopItem.ItemPrice;

        public bool IsPurchased(ShopItem shopItem) => _userInventoryService.GetPurchasedGameItemsIDs().Contains(shopItem.ItemID);

        public bool IsSelected(ShopItem shopItem) =>
                _userInventoryService.GetUsedGameItemID() == shopItem.ItemID;
    }
}