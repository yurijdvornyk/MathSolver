using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultDataItem
    {
        public string Title { get; private set; }
        public List<string> ColumnTitles { get; private set; }
        public List<List<object>> ResultItemMatrix { get; private set; }

        public object[,] GetResultItemMatrix()
        {
            int rows = ResultItemMatrix.Count;
            int columns = 0;
            ResultItemMatrix.ForEach(columnList =>
            {
                if (columnList.Count > columns)
                {
                    columns = columnList.Count;
                }
            });
            object[,] result = new object[rows, columns];
            for (int i = 0; i < ResultItemMatrix.Count; ++i)
            {
                for (int j = 0; j < ResultItemMatrix[i].Count; ++j)
                {
                    result[i, j] = ResultItemMatrix[i][j];
                }
            }
            return result;
        }

        public class Builder
        {
            private ResultDataItem item;

            public static Builder Create()
            {
                return new Builder();
            }

            public ResultDataItem Build()
            {
                return item;
            }

            private Builder()
            {
                item = new ResultDataItem();
            }

            public Builder Title(string title)
            {
                item.Title = title;
                return this;
            }

            public Builder ColumnTitles(params string[] columnTitles)
            {
                item.ColumnTitles = new List<string>(columnTitles);
                return this;
            }

            public Builder Matrix(object[,] matrix)
            {
                List<List<object>> rows = new List<List<object>>();
                for (int i = 0; i < matrix.GetLength(0); ++i)
                {
                    List<object> row = new List<object>();
                    for (int j = 0; j < matrix.GetLength(1); ++j)
                    {
                        row.Add(matrix[i, j]);
                    }
                    rows.Add(row);
                }
                item.ResultItemMatrix = rows;
                return this;
            }
        }
    }
}