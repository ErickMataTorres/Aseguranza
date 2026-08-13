using System.Drawing;
using System.Drawing.Drawing2D;
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

            comboBox.DrawMode =
                DrawMode.OwnerDrawFixed;

            comboBox.ItemHeight =
                24;

            comboBox.BackColor =
                Color.White;

            comboBox.ForeColor =
                AppColors.TextPrimary;

            comboBox.Font =
                AppFonts.Light(
                    10.5F);

            comboBox.Cursor =
                Cursors.Hand;

            comboBox.IntegralHeight =
                false;

            comboBox.MaxDropDownItems =
                12;

            // Dejamos espacio visual para nuestra flecha.
            comboBox.Location =
                new Point(
                    8,
                    7);

            comboBox.Size =
                new Size(
                    container.Width - 16,
                    28);

            comboBox.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            InputStyler.ApplyOutlinedInput(
                container,
                comboBox);

            // =====================================================
            // DIBUJO DEL TEXTO / ELEMENTOS
            // =====================================================

            comboBox.DrawItem +=
                (_, e) =>
                {
                    if (e.Bounds.Width <= 0 ||
                        e.Bounds.Height <= 0)
                    {
                        return;
                    }

                    bool seleccionado =
                        (e.State &
                         DrawItemState.Selected) ==
                        DrawItemState.Selected;

                    Color fondo =
                        seleccionado
                            ? AppColors.HoverBackground
                            : Color.White;

                    Color texto =
                        comboBox.Enabled
                            ? AppColors.TextPrimary
                            : SystemColors.GrayText;

                    using SolidBrush brushFondo =
                        new SolidBrush(
                            fondo);

                    e.Graphics.FillRectangle(
                        brushFondo,
                        e.Bounds);

                    string valor =
                        e.Index >= 0 &&
                        e.Index < comboBox.Items.Count
                            ? comboBox.GetItemText(
                                comboBox.Items[e.Index])
                            : comboBox.Text;

                    Rectangle areaTexto =
                        new Rectangle(
                            e.Bounds.X + 6,
                            e.Bounds.Y,
                            System.Math.Max(
                                0,
                                e.Bounds.Width - 42),
                            e.Bounds.Height);

                    TextRenderer.DrawText(
                        e.Graphics,
                        valor,
                        comboBox.Font,
                        areaTexto,
                        texto,
                        TextFormatFlags.Left |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.EndEllipsis |
                        TextFormatFlags.NoPrefix);

                    if ((e.State &
                         DrawItemState.Focus) ==
                        DrawItemState.Focus)
                    {
                        e.DrawFocusRectangle();
                    }
                };

            // =====================================================
            // FLECHA PERSONALIZADA
            // =====================================================

            Panel arrowPanel =
                new Panel
                {
                    Name =
                        "pnlComboArrow",

                    Size =
                        new Size(
                            34,
                            container.Height - 4),

                    Location =
                        new Point(
                            container.Width - 36,
                            2),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Right |
                        AnchorStyles.Bottom,

                    BackColor =
                        Color.White,

                    Cursor =
                        Cursors.Hand,

                    TabStop =
                        false
                };

            arrowPanel.Paint +=
                (_, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    Color arrowColor =
                        comboBox.Enabled
                            ? AppColors.TextSecondary
                            : SystemColors.GrayText;

                    using Pen pen =
                        new Pen(
                            arrowColor,
                            1.6F);

                    int centerX =
                        arrowPanel.ClientSize.Width / 2;

                    int centerY =
                        arrowPanel.ClientSize.Height / 2;

                    e.Graphics.DrawLine(
                        pen,
                        centerX - 4,
                        centerY - 2,
                        centerX,
                        centerY + 2);

                    e.Graphics.DrawLine(
                        pen,
                        centerX,
                        centerY + 2,
                        centerX + 4,
                        centerY - 2);
                };

            void AbrirCombo()
            {
                if (!comboBox.Enabled)
                {
                    return;
                }

                comboBox.Focus();

                comboBox.DroppedDown =
                    true;
            }

            arrowPanel.Click +=
                (_, _) =>
                {
                    AbrirCombo();
                };

            arrowPanel.MouseEnter +=
                (_, _) =>
                {
                    if (comboBox.Enabled)
                    {
                        arrowPanel.BackColor =
                            AppColors.HoverBackground;
                    }
                };

            arrowPanel.MouseLeave +=
                (_, _) =>
                {
                    arrowPanel.BackColor =
                        Color.White;
                };

            comboBox.EnabledChanged +=
                (_, _) =>
                {
                    arrowPanel.Enabled =
                        comboBox.Enabled;

                    arrowPanel.BackColor =
                        Color.White;

                    arrowPanel.Invalidate();
                    comboBox.Invalidate();
                    container.Invalidate();
                };

            comboBox.SelectedIndexChanged +=
                (_, _) =>
                {
                    comboBox.Invalidate();
                };

            comboBox.DropDown +=
                (_, _) =>
                {
                    container.Invalidate();
                };

            comboBox.DropDownClosed +=
                (_, _) =>
                {
                    container.Invalidate();
                };

            container.Resize +=
                (_, _) =>
                {
                    comboBox.Size =
                        new Size(
                            System.Math.Max(
                                40,
                                container.Width - 16),
                            28);

                    arrowPanel.Location =
                        new Point(
                            container.Width - 36,
                            2);

                    arrowPanel.Size =
                        new Size(
                            34,
                            System.Math.Max(
                                20,
                                container.Height - 4));

                    arrowPanel.BringToFront();
                };

            container.Controls.Add(
                arrowPanel);

            arrowPanel.BringToFront();

            container.Invalidate();
        }
    }
}
