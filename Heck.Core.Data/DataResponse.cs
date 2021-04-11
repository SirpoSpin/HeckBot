using System;
using System.Collections.Generic;
using System.Text;

namespace Heck.Core.Data
{
    public class DataResponse
    {
        public DataResponse(string message, bool result)
        {
            Message = message;
            Result = result;
        }
        public ModelBase Item { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
    }
    public class DataResponseTextRecordList : DataResponse
    {
        public DataResponseTextRecordList(string message, bool result) : base(message, result)
        { }
        public List<HeckTextRecord> List { get; set; }
    }
}
