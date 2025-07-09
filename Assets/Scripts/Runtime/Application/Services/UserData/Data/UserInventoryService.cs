using System.Collections.Generic;
using R3;

namespace Application.Services.UserData
{
    public class UserInventoryService : IUserInventoryService
    {
        private readonly UserDataService _userDataService;
        public Subject<int> BalanceChangedSubject { get; } = new();

        public UserInventoryService(UserDataService userDataService) =>
                _userDataService = userDataService;

        public void UpdateBalance(int balance)
        {
            _userDataService.GetUserData().UserInventory.UpdateBalance(balance);
            BalanceChangedSubject?.OnNext(balance);
        }

        public int GetBalance() => 
                _userDataService.GetUserData().UserInventory.Balance;

        public void UpdateUsedGameItemID(int userGameItemID) =>
                _userDataService.GetUserData().UserInventory.UsedGameItemID = userGameItemID;

        public int GetUsedGameItemID() =>
                _userDataService.GetUserData().UserInventory.UsedGameItemID;

        public void AddPurchasedGameItemID(int userGameItemID) =>
                _userDataService.GetUserData().UserInventory.PurchasedGameItemsIDs.Add(userGameItemID);

        public List<int> GetPurchasedGameItemsIDs() =>
                _userDataService.GetUserData().UserInventory.PurchasedGameItemsIDs;
    }
}