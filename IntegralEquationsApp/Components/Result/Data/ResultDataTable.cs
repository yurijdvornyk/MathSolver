using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralEquationsApp.Components.Result.Data
{
    public class ResultDataTable : DataTable
    {
        private const string RESULTS = "Results";

        public ResultDataTable(string tableName = RESULTS) : base(tableName) { }

        public void SetDataColumns(List<string> dataColumnsTitles)
        {
            foreach (string title in dataColumnsTitles)
            {
                if (string.IsNullOrEmpty(title))
                {
                    Columns.Add();
                }
                else
                {
                    Columns.Add(title);
                }
            }
        }

        public void SetData(object[,] data)
        {
            for (int i = 0; i < data.GetLength(0); ++i)
            {
                DataRow row = NewRow();
                for (int j = 0; j < data.GetLength(1); ++j)
                {
                    row[j] = data[i, j];
                }
                Rows.Add(row);
            }
        }

        public DataView AsDataView()
        {
            return DataTableExtensions.AsDataView(this);
        }
    }
}
