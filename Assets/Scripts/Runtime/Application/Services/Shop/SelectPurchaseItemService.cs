using Application.Services.UserData;
using Runtime.Application.ShopSystem;

namespace Runtime.Application.Services.Shop
{
    public class SelectPurchaseItemService : ISelectPurchaseItemService
    {
        private readonly IUserInventoryService _userInventoryService;
        
        private ShopItemDisplayModel _shopItem;
        private ShopItemDisplayModel _previousItem;

        public SelectPurchaseItemService(IUserInventoryService userInventoryService) =>
                _userInventoryService = userInventoryService;
        
        public void SelectPurchasedItem(ShopItemDisplayModel shopItemModel)
        {
            SetItem(shopItemModel);
            UpdateStates();
        }
        
        private void SetItem(ShopItemDisplayModel shopItemModel)
        {
            if(_shopItem != null)
                _previousItem = _shopItem;

            _shopItem = shopItemModel;
        }

        private void UpdateStates()
        {
            _shopItem?.SetShopItemState(ShopItemState.Selected);
            _previousItem?.SetShopItemState(ShopItemState.Purchased);
            _userInventoryService.UpdateUsedGameItemID(_shopItem.ShopItem.ItemID);
        }
    }
}