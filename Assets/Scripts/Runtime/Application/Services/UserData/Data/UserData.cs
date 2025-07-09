using System;
using System.Collections.Generic;

namespace Application.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new();
        public UserProgressData UserProgressData = new();
        public SettingsData SettingsData = new();
        public GameData GameData = new();
        public UserInventory UserInventory = new UserInventory();
    }
}