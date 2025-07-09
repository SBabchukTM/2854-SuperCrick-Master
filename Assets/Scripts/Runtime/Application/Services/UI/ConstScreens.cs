namespace Application.UI
{
    public static class ConstScreens
    {
        public const string UiServiceViewContainer = nameof(UiServiceViewContainer);
        public const string SplashScreen = nameof(SplashScreen);
        public const string TitleScreen = nameof(TitleScreen);
        public const string GameScreen = nameof(GameScreen);
        public const string LevelSelectionScreen = nameof(LevelSelectionScreen);
        public const string SettingsScreen = nameof(SettingsScreen);
        public const string ShopScreen = "ShopScreen"; 
        public const string ChooseTeamScreen = nameof(ChooseTeamScreen);
    }
    public enum GameStateTypeId
    {
        RunningState = 1,
        PausedState = 0,
    }
}