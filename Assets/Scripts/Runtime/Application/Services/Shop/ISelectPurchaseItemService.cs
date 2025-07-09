using Runtime.Application.ShopSystem;

namespace Runtime.Application.Services.Shop
{
    public interface ISelectPurchaseItemService
    {
        void SelectPurchasedItem(ShopItemDisplayModel shopItemModel);
    }
}