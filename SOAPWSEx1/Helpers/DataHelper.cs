using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAPWSEx1.Helpers
{
    public static class DataHelper
    {
        public static string GetJSONString(DataTable dt)
        {
            string[] stringDataSet = new string[dt.Columns.Count];
            string headString = string.Empty;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                stringDataSet[i] = dt.Columns[i].Caption;
                headString += "\"" + stringDataSet[i] + "\" : \"" + stringDataSet[i] + i.ToString() + "¾" + "\",";
            }

            headString = headString.Substring(0, headString.Length - 1);

            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + dt.TableName + "\" : [");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string TempStr = headString;
                Sb.Append("{");

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    TempStr = TempStr.Replace(dt.Columns[j] + j.ToString() + "¾", dt.Rows[i][j].ToString());
                }
                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");

            return Sb.ToString();
        }
    }
}
