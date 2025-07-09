using System;
using System.Collections.Generic;

namespace Application.Services.UserData
{
    [Serializable]
    public class UserInventory
    {
        public int Balance = 150;
        public int UsedGameItemID;
        public List<int> PurchasedGameItemsIDs = new() { 0 };
        
        public void UpdateBalance(int balance) => 
                Balance = balance;
    }
}