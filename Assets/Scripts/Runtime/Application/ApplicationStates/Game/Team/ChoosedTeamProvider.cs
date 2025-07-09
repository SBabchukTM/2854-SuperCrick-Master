using System.Linq;

namespace Application.Game
{
    public class ChoosedTeamProvider : IChoosedTeamProvider
    {
        public ReactiveDictionary<ChoosedTeamModel, int> Teams { get; } = new();

        public void AddTeam(ChoosedTeamModel choosedTeamModel) =>
                Teams.Add(choosedTeamModel, 0);

        public ChoosedTeamModel GetTeam(TeamTypeId teamTypeId) =>
                Teams.Dictionary.Where(team => team.Key.TeamTypeId == teamTypeId).Select(team => team.Key).FirstOrDefault();

        public int GetScoreThroughId(TeamTypeId teamTypeId) =>
                Teams[GetTeam(teamTypeId)];

        public void AddScore(ChoosedTeamModel choosedTeamModel) =>
                Teams[choosedTeamModel]++;

        public void Reset()
        {
            Teams[GetTeam(TeamTypeId.Player)] = 0;
            Teams[GetTeam(TeamTypeId.Enemy)] = 0;
        }

        public void Cleanup() =>
                Teams.Dictionary.Clear();
    }
}