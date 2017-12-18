using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using SQLiteDb.Containers;
using SQLiteDb.dsTableAdapters;

namespace SQLiteDb
{
    public class LocalDb
    {
        private static string _DbPath;

        public static void Init(string dbPath)
        {
            _DbPath = dbPath;
        }

        private static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=" + _DbPath);
        }

        public static void CleanResultBox()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM [result_box]", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void CleanTourBox()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM [tour_box]", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand("UPDATE SQLITE_SEQUENCE SET seq = 1 WHERE name = 'tour_box'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void CleanInputBox()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM [input_box]", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void CleanRowBox()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM [raw_box]", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void InsertInputBox(String value, decimal chance)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new input_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    adapter.InsertQuery(value, chance);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void InsertRawBox(String pan, decimal amount)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new raw_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    adapter.InsertQuery(pan, amount);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void InsertRawBox(List<RawItem> items)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new raw_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    foreach (var item in items)
                    {
                        adapter.InsertQuery(item.Pan, item.Amount);
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static List<ds.raw_boxRow> ListRawBox()
        {
            var connection = GetConnection();
            List<ds.raw_boxRow> result = new List<ds.raw_boxRow>();

            try
            {
                using (var adapter = new raw_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    using (var table = new ds.raw_boxDataTable())
                    {
                        adapter.Fill(table);
                        foreach (ds.raw_boxRow row in table.Rows)
                        {
                            result.Add(row);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public static List<String> ListInputBox()
        {
            var connection = GetConnection();
            List<String> result = new List<String>();

            try
            {
                using (var adapter = new input_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    using (var table = new ds.input_boxDataTable())
                    {
                        adapter.Fill(table);
                        foreach (ds.input_boxRow row in table.Rows)
                        {
                            for (int i = 0; i < row.chance; i++)
                            {
                                result.Add(row.pan);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public static List<String> ListInputBox(Dictionary<String, int> winNumbers)
        {
            var connection = GetConnection();
            List<String> result = new List<String>();

            try
            {
                using (var adapter = new input_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    using (var table = new ds.input_boxDataTable())
                    {
                        adapter.Fill(table);
                        foreach (ds.input_boxRow row in table.Rows)
                        {
                            int chance = Convert.ToInt32(row.chance);
                            int existChance;
                            winNumbers.TryGetValue(row.pan, out existChance);
                            chance -= existChance;

                            for (int i = 0; i < chance; i++)
                            {
                                result.Add(row.pan);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public static decimal SumPanInRawBox(String pan)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new raw_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    var rawSum = adapter.Sum(pan);
                    return Convert.ToDecimal(rawSum);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static int CountRawBox()
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new raw_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    var rawSum = adapter.CountRows();
                    return Convert.ToInt32(rawSum);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static bool HasRecord(string pan)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new input_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    var rawSum = adapter.HasRecord(pan);
                    return Convert.ToInt32(rawSum) > 0;
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static ds.input_boxRow GetRecord(string pan)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new input_boxTableAdapter
                {
                    Connection = connection
                })
                {
                    using (var table = new ds.input_boxDataTable())
                    {
                        adapter.FillByPan(table, pan);
                        foreach (ds.input_boxRow row in table.Rows)
                        {
                            return row;
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }
    }
}
