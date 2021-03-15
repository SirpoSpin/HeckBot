using Discord.Commands;
using Heck.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeckBot.Modules
{
    public interface IHeckUtils
    {
        HeckUser GetHeckUser(string SnowFlake, int GuildID, string UserName, bool CreateIfNotExists);
        HeckGuild GetHeckGuild(string SnowFlake, bool CreateIfNotExists);
        bool CreateHeck(int UserID, int HeckedUserID, string Name, float Value);
        bool CreateHeckUser(string SnowFlake, string Username, int GuildID);
        bool CreateHeckGuild(string SnowFlake);
        UserHeckTotal GetUserHeckTotal(int UserID);
        bool ResetAvailableHecks();
        List<UserHeckTotal> GetLeaderboard(SocketCommandContext Context);
    }
}
