using Discord;
using Discord.Commands;
using Heck.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeckBot.Modules
{
    public class LocalHeckUtil : IHeckUtils
    {
        public bool CreateHeck(int UserID, int HeckedUserID, string Name, float Value)
        {
            HeckModel model = new HeckModel(null);
            DataResponse resp = model.HeckingCreate(UserID, HeckedUserID, Name, Value);
            return resp.Result;
        }

        public bool CreateHeckGuild(string SnowFlake)
        {
            HeckGuildModel model = new HeckGuildModel(null);
            DataResponse resp = model.HeckingCreate(SnowFlake);
            return resp.Result;
        }

        public bool CreateHeckUser(string SnowFlake, string Username, int GuildID)
        {
            HeckUserModel model = new HeckUserModel(null);
            DataResponse resp = model.HeckingCreate(SnowFlake, Username, GuildID);
            return resp.Result;
        }

        public HeckGuild GetHeckGuild(string SnowFlake, bool CreateIfNotExists)
        {
            HeckGuild item = null;
            HeckGuildModel model = new HeckGuildModel(null);
            DataResponse resp = model.GetGuildBySnowflake(SnowFlake);
            if (!resp.Result && CreateIfNotExists)
            {
                if (CreateHeckGuild(SnowFlake))
                    resp = model.GetGuildBySnowflake(SnowFlake);
            }
            if (resp != null)
                item = resp.Item as HeckGuild;
            model.Dispose();
            return item;
        }

        public HeckUser GetHeckUser(string SnowFlake, int GuildID, string UserName, bool CreateIfNotExists)
        {
            HeckUser item = null;
            HeckUserModel model = new HeckUserModel(null);
            DataResponse resp = model.GetUserBySnowflakeAndGuildID(SnowFlake, GuildID);
            if (!resp.Result && CreateIfNotExists)
            {
                if (CreateHeckUser(SnowFlake, UserName, GuildID))
                    resp = model.GetUserBySnowflakeAndGuildID(SnowFlake, GuildID);
            }
            if (resp != null)
                item = resp.Item as HeckUser;
            model.Dispose();
            return item;
        }

        public UserHeckTotal GetUserHeckTotal(int UserID)
        {
            UserHeckTotal item = null;
            HeckProcedureModel model = new HeckProcedureModel(null);
            DataResponse resp = model.GetUserHeckTotal(UserID);
            if (resp != null)
                item = resp.Item as UserHeckTotal;
            model.Dispose();
            return item;
        }

        public List<HeckTextRecord> GetHecksReceived(int UserID)
        {
            HeckProcedureModel model = new HeckProcedureModel(null);
            DataResponseTextRecordList resp = model.GetReceivedHecks(UserID);
            model.Dispose();
            return resp.List ?? new List<HeckTextRecord>();
        }

        public List<HeckTextRecord> GetHecksSent(int UserID)
        {
            HeckProcedureModel model = new HeckProcedureModel(null);
            DataResponseTextRecordList resp = model.GetSentHecks(UserID);
            model.Dispose();
            return resp.List ?? new List<HeckTextRecord>();
        }

        public List<UserHeckTotal> GetLeaderboard(SocketCommandContext Context)
        {
            IGuild dGuild = Context.Guild;
            HeckGuild guild = GetHeckGuild(dGuild.Id.ToString(), true);
            List<HeckUser> usrs = null;
            List<UserHeckTotal> lst = new List<UserHeckTotal>();
            HeckUserModel mdl = new HeckUserModel(null);
            usrs = mdl.GetAllUsersByGuildID(guild.ID);
            mdl.Dispose();
            foreach (HeckUser usr in usrs)
            {
                UserHeckTotal ttl = GetUserHeckTotal(usr.ID);
                if (ttl != null)
                {
                    IUser dUser = null;
                    try
                    {
                        dUser = Context.Guild.GetUser(Convert.ToUInt64(usr.Snowflake));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured while getting guild user by " + usr.Snowflake + ". Message: " + ex.Message);
                    }
                    if (dUser == null)
                    {
                        Console.WriteLine("Could not find user in guild by snowflake: " + usr.Snowflake);
                        continue;
                    }
                    if (dUser.IsBot)
                        continue;
                    ttl.User.UserName = dUser.Username;
                    lst.Add(ttl);
                }
            }
            lst = lst.OrderByDescending(o => o.HecksWeighted).ToList();
            return lst;
        }

        public bool ResetAvailableHecks()
        {
            HeckProcedureModel model = new HeckProcedureModel(null);
            model.ResetAvailableHecks();
            model.Dispose();
            return true;
        }

        public HeckParameter GetHeckParameterByName(string Name)
        {
            HeckParameter item = null;
            HeckParameterModel model = new HeckParameterModel(null);
            DataResponse resp = model.GetParamByName(Name);
            if (resp != null)
                item = resp.Item as HeckParameter;
            model.Dispose();
            return item;
        }
    }
}
