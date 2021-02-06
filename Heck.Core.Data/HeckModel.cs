using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Heck.Core.Data
{
    public class HeckModel : CustomMysqlDBContext
    {
        public HeckModel(string ConnectionString) : base(ConnectionString)
        {
        }

        public DataResponse HeckingCreate(int UserID, int HeckedUserID, string Name, float Value)
        {
            Value = Value + 0.00f;
            DataResponse resp;
            connection.Open();
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("InsertHeck"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.Add("@HeckedUserID", MySqlDbType.Int32).Value = UserID;
                cmd.Parameters.Add("@Reason", MySqlDbType.VarChar).Value = Name;
                cmd.Parameters.Add("@HeckValue", MySqlDbType.Decimal).Value = Value;
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }

            }
            int responseCode = Convert.ToInt32(dt.Rows[0][0]);
            if(responseCode == 1)
                resp = new DataResponse("Heck Created", true);
            else
                resp = new DataResponse("Heck Not created :(", false);

            return resp;
        }

        public DataResponse HeckingDelete(int UserID, string Name)
        {
            DataResponse resp;
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE FROM heck where UserID = @userid AND Name = @name";
                cmd.Parameters.Add("@userid", MySqlDbType.Int32).Value = UserID;
                cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = Name;

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            resp = new DataResponse("Heck Deleted", true);
            return resp;
        }
        public override ModelBase ColumnMapper(DataTable dt)
        {
            Heck item = new Heck();
            item.ID = Convert.ToInt32(dt.Rows[0][0]);
            item.UserID = Convert.ToInt32(dt.Rows[0][1]);
            item.Name = Convert.ToString(dt.Rows[0][2]);
            item.Value = float.Parse(Convert.ToString(dt.Rows[0][3]));
            item.CreatedDate = Convert.ToDateTime(dt.Rows[0][4]);
            item.ExpiryDate = Convert.ToDateTime(dt.Rows[0][5]);
            return item;
        }

    }

    public class Heck : ModelBase
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
