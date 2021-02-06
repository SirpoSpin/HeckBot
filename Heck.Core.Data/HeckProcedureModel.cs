using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Heck.Core.Data
{
    public class HeckProcedureModel : CustomMysqlDBContext
    {
        public HeckProcedureModel(string ConnectionString) : base(ConnectionString)
        {
        }

        public DataResponse GetUserHeckTotal(int UserID)
        {
            DataResponse resp;
            connection.Open();
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("GetUserHeckTotals"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
            UserHeckTotal item = ColumnMapper(dt) as UserHeckTotal;
            if (item != null)
            {
                HeckUserModel mdl = new HeckUserModel(null);
                DataResponse usrResp = mdl.GetUserByID(UserID);
                if (usrResp.Result)
                    item.User = usrResp.Item as HeckUser;
                mdl.Dispose();
                resp = new DataResponse("User Heck Total Found", true);
                resp.Item = item;
            }
            else
                resp = new DataResponse("User Heck Total could not be found.", true);
            return resp;
        }

        public bool ResetAvailableHecks()
        {
            connection.Open();
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("ResetAvailableHecks"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
            int responseCode = Convert.ToInt32(dt.Rows[0][0]);
            if (responseCode == 1)
                return true;
            else
                return false;
        }
        public override ModelBase ColumnMapper(DataTable dataTable)
        {
            if (dataTable.Columns[0].ColumnName == "Hecks")
            {
                return UserHeckTotalColumnMapper(dataTable);
            }
            return null;

        }

        private UserHeckTotal UserHeckTotalColumnMapper(DataTable dataTable)
        {
            UserHeckTotal item = new UserHeckTotal();
            item.HeckTotal = float.Parse(Convert.ToString(dataTable.Rows[0][0]));
            item.WeightTotal = float.Parse(Convert.ToString(dataTable.Rows[0][1]));
            item.HecksWeighted = float.Parse(Convert.ToString(dataTable.Rows[0][2]));
            item.AvailableHecks = float.Parse(Convert.ToString(dataTable.Rows[0][3]));
            return item;
        }
    }

    public class UserHeckTotal : ModelBase
    {
        public float HeckTotal { get; set; }
        public float WeightTotal { get; set; }
        public float HecksWeighted { get; set; }
        public float AvailableHecks { get; set; }
        public HeckUser User { get; set; }
    }
}
