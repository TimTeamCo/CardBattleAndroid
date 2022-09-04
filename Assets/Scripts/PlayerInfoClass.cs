using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class PlayerInfoClass : IPlayerInfo
{
    public PlayerInfo _playerInfo;
    public string _externalIds;

    public async Task GetPlayerInfoAsync()
    {
        _playerInfo = await AuthenticationService.Instance.GetPlayerInfoAsync();
        _externalIds = GetExternalIds(_playerInfo);
    }
    
    string GetExternalIds(PlayerInfo playerInfo)
    {
        var sb = new StringBuilder();
        if (playerInfo.Identities != null)
        {
            foreach (var id in playerInfo.Identities)
                sb.Append(id.TypeId + " ");

            return sb.ToString();
        }

        return "None";
    }
}