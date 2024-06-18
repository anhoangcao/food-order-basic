using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using internship.Models;
using Microsoft.Extensions.Logging;

namespace internship.Services
{
    public class ExcelImporter
    {
        private readonly ILogger<ExcelImporter> _logger;
        private readonly string _connectionString;

        public ExcelImporter(ILogger<ExcelImporter> logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public List<MenuItemModel> ImportFromExcel(string filePath)
        {
            List<MenuItemModel> items = new List<MenuItemModel>();
            string excelConnectionString = GetExcelConnectionString(filePath);

            using (OleDbConnection conn = new OleDbConnection(excelConnectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", conn);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    try
                    {
                        items.Add(new MenuItemModel
                        {
                            ItemID = Convert.ToInt32(row["ItemID"]),
                            ItemName = row["ItemName"].ToString(),
                            Description = row["Description"].ToString(),
                            Price = Convert.ToDecimal(row["Price"]),
                            Available = row["Available"].ToString() == "Yes",
                            ImageUrl = row["ImageUrl"].ToString(),
                            CreatedAt = DateTime.Parse(row["CreatedAt"].ToString()),
                            UpdatedAt = DateTime.Parse(row["UpdatedAt"].ToString())
                        });
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Error processing row {i + 1}: {ex.Message}";
                        _logger.LogError(ex, errorMessage);
                        throw new Exception(errorMessage);
                    }
                }
            }

            BulkInsert(items);
            return items;
        }

        private string GetExcelConnectionString(string filePath)
        {
            return $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Xml;HDR=YES;'";
        }

        private void BulkInsert(List<MenuItemModel> items)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ItemName", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));
            dataTable.Columns.Add("Available", typeof(bool));
            dataTable.Columns.Add("ImageUrl", typeof(string));
            dataTable.Columns.Add("CreatedAt", typeof(DateTime));
            dataTable.Columns.Add("UpdatedAt", typeof(DateTime));

            foreach (var item in items)
            {
                dataTable.Rows.Add(item.ItemID, item.ItemName, item.Description, item.Price, item.Available, item.ImageUrl, item.CreatedAt, item.UpdatedAt);
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "MenuItems";
                    bulkCopy.ColumnMappings.Add("ItemID", "ItemID");
                    bulkCopy.ColumnMappings.Add("ItemName", "ItemName");
                    bulkCopy.ColumnMappings.Add("Description", "Description");
                    bulkCopy.ColumnMappings.Add("Price", "Price");
                    bulkCopy.ColumnMappings.Add("Available", "Available");
                    bulkCopy.ColumnMappings.Add("ImageUrl", "ImageUrl");
                    bulkCopy.ColumnMappings.Add("CreatedAt", "CreatedAt");
                    bulkCopy.ColumnMappings.Add("UpdatedAt", "UpdatedAt");
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
