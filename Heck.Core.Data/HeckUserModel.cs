using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Heck.Core.Data
{
    public class HeckUserModel : CustomMysqlDBContext
    {
        public HeckUserModel(string ConnectionString) : base(ConnectionString)
        {
        }

        public DataResponse HeckingCreate(string Snowflake, string Username, int GuildID)
        {
            DataResponse resp;
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO heckuser (GuildID, DiscordSnowflake, username, CreatedDate, AvailableHecks) VALUES (@guildid, @snowflake, @username, @date, 100)";
                cmd.Parameters.Add("@snowflake", MySqlDbType.VarChar).Value = Snowflake;
                cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = Username;
                cmd.Parameters.Add("@guildid", MySqlDbType.Int32).Value = GuildID;

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            resp = new DataResponse("Heck User Created", true);
            return resp;
        }

        public DataResponse HeckingDelete(string Snowflake, int GuildID)
        {
            DataResponse resp;
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE FROM heckuser where DiscordSnowflake = @snowflake And GuildID = @guildID";
                cmd.Parameters.Add("@snowflake", MySqlDbType.VarChar).Value = Snowflake;
                cmd.Parameters.Add("@guildid", MySqlDbType.Int32).Value = GuildID;

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            resp = new DataResponse("Heck User Deleted", true);
            return resp;
        }

        public DataResponse GetUserBySnowflakeAndGuildID(string Snowflake, int GuildID)
        {
            DataResponse resp = null;
            DataTable dt = null;
            //create query
            string query = string.Format("select * from `heckuser` where DiscordSnowflake = '{0}' AND GuildID = {1}", Snowflake, GuildID.ToString());
            if (RunQuery(query, out dt))
            {
                HeckUser item = ColumnMapper(dt) as HeckUser;
                resp = new DataResponse("Heck user Found", true);
                resp.Item = item;
            }
            else
            {
                resp = new DataResponse("Heck user not found by snowflake (" + Snowflake + ") and guild id (" + GuildID.ToString() + ").", false);
            }
            return resp;

        }

        public DataResponse GetUserByID(int UserID)
        {
            DataResponse resp = null;
            DataTable dt = null;
            //create query
            string query = string.Format("select * from `heckuser` where ID = {0}", UserID.ToString());
            if (RunQuery(query, out dt))
            {
                HeckUser item = ColumnMapper(dt) as HeckUser;
                resp = new DataResponse("Heck user Found", true);
                resp.Item = item;
            }
            else
            {
                resp = new DataResponse("Heck user not found by ID (" + UserID.ToString() + ").", false);
            }
            return resp;

        }
        public List<HeckUser> GetAllUsersByGuildID(int GuildID)
        {
            List<HeckUser> lst = new List<HeckUser>();
            DataTable dt = null;
            //create query
            string query = string.Format("select * from `heckuser` where GuildID = {0}", GuildID.ToString());
            if (RunQuery(query, out dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    HeckUser item = ColumnMapper(dr) as HeckUser;
                    lst.Add(item);
                }
            }
            else
            {
                Console.WriteLine("Heck users not found by Guild ID (" + GuildID.ToString() + ").", false);
            }
            return lst;

        }
        public ModelBase ColumnMapper(DataRow dr)
        {
            HeckUser item = new HeckUser();
            item.ID = Convert.ToInt32(dr[0]);
            item.GuildID = Convert.ToInt32(dr[1]);
            item.Snowflake = Convert.ToString(dr[2]);
            item.UserName = Convert.ToString(dr[3]);
            item.AvailableHecks = Convert.ToInt32(dr[4]);
            item.CreatedDate = Convert.ToDateTime(dr[5]);
            return item;
        }

        public override ModelBase ColumnMapper(DataTable dt)
        {
            HeckUser item = new HeckUser();
            item.ID = Convert.ToInt32(dt.Rows[0][0]);
            item.GuildID = Convert.ToInt32(dt.Rows[0][1]);
            item.Snowflake = Convert.ToString(dt.Rows[0][2]);
            item.UserName = Convert.ToString(dt.Rows[0][3]);
            item.AvailableHecks = Convert.ToInt32(dt.Rows[0][4]);
            item.CreatedDate = Convert.ToDateTime(dt.Rows[0][5]);
            return item;
        }

    }

    public class HeckUser : ModelBase
    {
        public int GuildID { get; set; }
        public string Snowflake { get; set; }
        public int AvailableHecks { get; set; }
        public string UserName { get; set; }
    }
}
