using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Heck.Core.Data
{
    public class HeckGuildModel : CustomMysqlDBContext
    {
        public HeckGuildModel(string ConnectionString) : base(ConnectionString)
        {
        }

        public DataResponse HeckingCreate(string Snowflake)
        {
            DataResponse resp;
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO heckguild (DiscordSnowflake, CreatedDate) VALUES (@snowflake, @date)";
                cmd.Parameters.Add("@snowflake", MySqlDbType.VarChar).Value = Snowflake;
                cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = DateTime.Now;

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            resp = new DataResponse("Heck Guild Created", true);
            return resp;
        }

        public DataResponse HeckingDelete(string Snowflake)
        {
            DataResponse resp;
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE FROM heckguild where DiscordSnowflake = @snowflake";
                cmd.Parameters.Add("@snowflake", MySqlDbType.VarChar).Value = Snowflake;

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            resp = new DataResponse("Heck Guild Deleted", true);
            return resp;
        }

        public DataResponse GetGuildBySnowflake(string Snowflake)
        {
            DataResponse resp = null;
            DataTable dt = null;
            //create query
            string query = string.Format("select * from `heckguild` where DiscordSnowflake = '{0}'", Snowflake);
            if (RunQuery(query, out dt))
            {
                HeckGuild item = ColumnMapper(dt) as HeckGuild;
                resp = new DataResponse("Heck guild Found", true);
                resp.Item = item;
            }
            else
            {
                resp = new DataResponse("Heck guild not found by snowflake (" + Snowflake + ").", false);
            }
            return resp;

        }
        public override ModelBase ColumnMapper(DataTable dt)
        {
            HeckGuild item = new HeckGuild();
            item.ID = Convert.ToInt32(dt.Rows[0][0]);
            item.Snowflake = Convert.ToString(dt.Rows[0][1]);
            item.CreatedDate = Convert.ToDateTime(dt.Rows[0][2]);
            return item;
        }

    }

    public class HeckGuild : ModelBase
    {
        public string Snowflake { get; set; }
    }
}
