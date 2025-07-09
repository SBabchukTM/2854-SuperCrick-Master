using System.Collections.Generic;
using R3;

namespace Application.Services.UserData
{
    public interface IUserInventoryService
    {
        Subject<int> BalanceChangedSubject { get; }

        void UpdateBalance(int balance);

        int GetBalance();

        void UpdateUsedGameItemID(int userGameItemID);

        int GetUsedGameItemID();

        void AddPurchasedGameItemID(int userGameItemID);

        List<int> GetPurchasedGameItemsIDs();
    }
}