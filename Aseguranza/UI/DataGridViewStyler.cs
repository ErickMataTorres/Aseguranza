using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class DataGridViewStyler
    {
        public static void ApplyCatalogStyle(
            DataGridView dataGridView,
            int headerHeight = 40,
            int rowHeight = 38)
        {
            // =====================================================
            // CONFIGURACIÓN GENERAL
            // =====================================================

            dataGridView.BackgroundColor =
                Color.White;

            dataGridView.BorderStyle =
                BorderStyle.None;

            dataGridView.GridColor =
                AppColors.Border;

            dataGridView.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            dataGridView.RowHeadersVisible =
                false;

            dataGridView.AllowUserToAddRows =
                false;

            dataGridView.AllowUserToDeleteRows =
                false;

            dataGridView.AllowUserToResizeRows =
                false;

            dataGridView.MultiSelect =
                false;

            dataGridView.ReadOnly =
                true;

            dataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridView.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // =====================================================
            // ENCABEZADOS
            // =====================================================

            dataGridView.EnableHeadersVisualStyles =
                false;

            dataGridView.ColumnHeadersHeight =
                headerHeight;

            dataGridView.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .BackColor =
                    AppColors.Primary;

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .ForeColor =
                    Color.White;

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .SelectionBackColor =
                    AppColors.Primary;

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .SelectionForeColor =
                    Color.White;

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .Font =
                    AppFonts.Regular(
                        10F,
                        FontStyle.Bold);

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .Alignment =
                    DataGridViewContentAlignment.MiddleLeft;

            dataGridView
                .ColumnHeadersDefaultCellStyle
                .Padding =
                    new Padding(
                        8,
                        0,
                        0,
                        0);

            // =====================================================
            // FILAS
            // =====================================================

            dataGridView
                .DefaultCellStyle
                .BackColor =
                    Color.White;

            dataGridView
                .DefaultCellStyle
                .ForeColor =
                    AppColors.TextPrimary;

            dataGridView
                .DefaultCellStyle
                .Font =
                    AppFonts.Light(10F);

            dataGridView
                .DefaultCellStyle
                .SelectionBackColor =
                    AppColors.Selection;

            dataGridView
                .DefaultCellStyle
                .SelectionForeColor =
                    AppColors.TextPrimary;

            dataGridView
                .DefaultCellStyle
                .Padding =
                    new Padding(
                        8,
                        0,
                        8,
                        0);

            dataGridView
                .AlternatingRowsDefaultCellStyle
                .BackColor =
                    AppColors.AlternateRow;

            dataGridView.RowTemplate.Height =
                rowHeight;
        }
    }
}