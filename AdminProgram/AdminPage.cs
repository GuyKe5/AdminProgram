using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.IO;
using ClosedXML.Excel;

namespace AdminProgram
{
    public partial class AdminPage : Form
    {
        private readonly ApiService apiService;
        private List<User> allUsers;

        public AdminPage()
        {
            InitializeComponent();
            apiService = new ApiService();
            PopulateUserList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            PerformSearch(searchTerm);
        }

        private void PerformSearch(string searchTerm)
        {
            List<User> matchingUsers = allUsers
                .Where(user => user.username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                               user.email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            dgvUsers.DataSource = matchingUsers;
        }
        private async void PopulateUserList()
        {
            allUsers = await apiService.GetAllUsers();

            dgvUsers.DataSource = allUsers;


            dgvUsers.Columns["Password"].Visible = false;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    saveFileDialog.DefaultExt = "xlsx";
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.FileName = "users";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        await ExportToExcel(dgvUsers, saveFileDialog.FileName);
                        MessageBox.Show("Table downloaded successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while downloading the table: " + ex.Message);
            }
        }

        private async Task ExportToExcel(DataGridView dataGridView, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                // Set column headers
                for (int col = 0; col < dataGridView.Columns.Count; col++)
                {
                    worksheet.Cell(1, col + 1).Value = dataGridView.Columns[col].HeaderText;
                }

                // Add rows and data
                for (int row = 0; row < dataGridView.Rows.Count; row++)
                {
                    for (int col = 0; col < dataGridView.Columns.Count; col++)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = dataGridView.Rows[row].Cells[col].Value?.ToString();
                    }
                }

                await Task.Run(() => workbook.SaveAs(filePath));
            }
        }
    }
}
