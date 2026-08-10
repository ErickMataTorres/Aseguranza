using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class ComboBoxStyler
    {
        public static void ApplyOutlinedComboBox(
            Panel container,
            ComboBox comboBox)
        {
            container.BackColor =
                Color.White;

            comboBox.DropDownStyle =
                ComboBoxStyle.DropDownList;

            comboBox.FlatStyle =
                FlatStyle.Flat;

            comboBox.BackColor =
                Color.White;

            comboBox.ForeColor =
                AppColors.TextPrimary;

            comboBox.Font =
                AppFonts.Light(10.5F);

            comboBox.Cursor =
                Cursors.Hand;

            InputStyler.ApplyOutlinedInput(
                container,
                comboBox);
        }
    }
}