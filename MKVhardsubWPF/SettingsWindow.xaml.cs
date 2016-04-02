using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MKVhardsubWPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        private Dispatcher currentDispatcher = Application.Current.Dispatcher;

        public SettingsWindow()
        {
            InitializeComponent();
            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                var settings = Properties.Settings.Default;
                crfSlider.Value = settings.CrfValue;
                cboPreset.Text = settings.Preset;
            }));
        }

        private void crfSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                lblCrfCount.Text = Convert.ToInt32(e.NewValue).ToString();
            }));
        }

        private void cmdSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.Preset = cboPreset.Text;
            settings.CrfValue = Convert.ToInt32(crfSlider.Value);
            settings.Save();
            this.Close();
        }

        private void cmdDiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
