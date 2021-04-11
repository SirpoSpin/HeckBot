using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Heck.Core.Data
{
    public class HeckParameterModel : CustomMysqlDBContext
    {
        public HeckParameterModel(string ConnectionString) : base(ConnectionString)
        {
        }

        public DataResponse GetParamByName(string Name)
        {
            DataResponse resp = null;
            DataTable dt = null;
            //create query
            string query = string.Format("select * from `heckparameter` where PARAM_NAME = '{0}'", Name);
            if (RunQuery(query, out dt))
            {
                HeckParameter item = ColumnMapper(dt) as HeckParameter;
                resp = new DataResponse("Heck Paramter Found", true);
                resp.Item = item;
            }
            else
            {
                resp = new DataResponse("Heck Paramter not found by Name (" + Name + ").", false);
            }
            return resp;

        }

        public override ModelBase ColumnMapper(DataTable dt)
        {
            HeckParameter item = new HeckParameter();
            item.ID = Convert.ToInt32(dt.Rows[0][0]);
            item.Param_Name = Convert.ToString(dt.Rows[0][1]);
            item.Param_Value = Convert.ToString(dt.Rows[0][2]);
            item.LastUpdateDate = Convert.ToDateTime(dt.Rows[0][3]);
            return item;
        }

    }

    public class HeckParameter: ModelBase
    {
        public string Param_Name { get; set; }
        public string Param_Value { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
