namespace Application.Game
{
    public interface IChoosedTeamProvider : ICleanup, IReset
    {
        void AddTeam(ChoosedTeamModel choosedTeamModel);

        void AddScore(ChoosedTeamModel choosedTeamModel);

        ChoosedTeamModel GetTeam(TeamTypeId teamTypeId);

        ReactiveDictionary<ChoosedTeamModel, int> Teams { get; }

        int GetScoreThroughId(TeamTypeId teamTypeId);
    }
}