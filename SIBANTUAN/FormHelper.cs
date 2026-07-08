using System;
using System.Windows.Forms;

namespace SIBANTUAN
{
    /// <summary>
    /// Helper class untuk mengatur fullscreen dan responsive layout di semua form
    /// </summary>
    public static class FormHelper
    {
        /// <summary>
        /// Mengatur form untuk fit fullscreen dengan proper scaling
        /// </summary>
        public static void SetFullscreenMode(Form form)
        {
            try
            {
                // Set window state ke maximized
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.Sizable;
                
                // Auto scale untuk responsive design
                form.AutoScaleMode = AutoScaleMode.Font;
                
                // Set main controls untuk responsive
                SetControlsResponsive(form);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting fullscreen: " + ex.Message);
            }
        }

        /// <summary>
        /// Mengatur semua controls dalam form untuk responsive design
        /// </summary>
        private static void SetControlsResponsive(Form form)
        {
            try
            {
                foreach (Control ctrl in form.Controls)
                {
                    // Set top panel ke dock top
                    if (ctrl.Name == "panel1" || ctrl.Name == "pnlHeader")
                    {
                        ctrl.Dock = DockStyle.Top;
                    }
                    // Set footer panel ke dock bottom
                    else if (ctrl.Name == "pnlFooter" || ctrl.Name == "panel2" && ctrl.Height < 10)
                    {
                        ctrl.Dock = DockStyle.Bottom;
                    }
                    // Set main content panels untuk fill
                    else if (ctrl is Panel || ctrl is DataGridView)
                    {
                        ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | 
                                     AnchorStyles.Right | AnchorStyles.Bottom;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting responsive controls: " + ex.Message);
            }
        }

        /// <summary>
        /// Mengatur DockStyle untuk panel-panel utama form
        /// </summary>
        public static void SetPanelDocking(Panel headerPanel, Panel contentPanel, Panel footerPanel)
        {
            try
            {
                if (headerPanel != null)
                {
                    headerPanel.Dock = DockStyle.Top;
                }

                if (contentPanel != null)
                {
                    contentPanel.Dock = DockStyle.Fill;
                    contentPanel.AutoScroll = true;
                }

                if (footerPanel != null)
                {
                    footerPanel.Dock = DockStyle.Bottom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting panel docking: " + ex.Message);
            }
        }

        /// <summary>
        /// Mengatur DataGridView untuk fit container dengan baik
        /// </summary>
        public static void SetDataGridViewResponsive(DataGridView dgv)
        {
            try
            {
                if (dgv != null)
                {
                    dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | 
                                AnchorStyles.Right | AnchorStyles.Bottom;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgv.RowTemplate.Height = 25;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting DataGridView: " + ex.Message);
            }
        }
    }
}
