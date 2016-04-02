using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using MKVhardsubWPF.Model;
using MKVhardsubWPF.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MKVhardsubWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ConvertTaskDataSource _localDb = null;
        private Dispatcher currentDispatcher = Application.Current.Dispatcher;
        private HardsubWorker ffmpegWorker = null;
        private int currentIndex = 0;
        private bool _isCancelled = false;

        private void ResetAll()
        {
            _localDb.Clear();
            currentIndex = 0;

            if (ffmpegWorker != null)
            {
                ffmpegWorker.ProgressChanged -= FfmpegWorker_ProgressChanged;
                ffmpegWorker.ActionCompleted -= FfmpegWorker_ActionCompleted;
            }

            ffmpegWorker = new HardsubWorker();
            ffmpegWorker.ProgressChanged += FfmpegWorker_ProgressChanged;
            ffmpegWorker.ActionCompleted += FfmpegWorker_ActionCompleted;

            cmdAddFiles.IsEnabled = true;
            cmdStartConvert.IsEnabled = true;
            cmdStopConvert.IsEnabled = false;
            cmdSettings.IsEnabled = true;
        }

        private void FfmpegWorker_ActionCompleted(object sender, EventArgs e)
        {
            if (_isCancelled)
            {
                if (currentIndex < _localDb.Count)
                {
                    currentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        _localDb[currentIndex].Progress = 100;
                        _localDb[currentIndex].Status = "Cancelled.";
                    }));
                }
                return;
            }

            if (currentIndex < _localDb.Count)
            {
                currentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    _localDb[currentIndex].Progress = 100;
                 _localDb[currentIndex].Status = "Completed.";
                }));
            }
            currentIndex++;
            if (currentIndex < _localDb.Count)
            {
                ffmpegWorker.StartAction(Helpers.Clone(_localDb[currentIndex]));
            }
            else
            {
                currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    cmdAddFiles.IsEnabled = false;
                    cmdClearAll.IsEnabled = true;
                    cmdStartConvert.IsEnabled = false;
                    cmdStopConvert.IsEnabled = false;
                    cmdSettings.IsEnabled = true;
                }));
            }
        }

        private void FfmpegWorker_ProgressChanged(object sender, MProgressChangedEventArgs e)
        {
            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                if (currentIndex <= _localDb.Count)
                {
                    _localDb[currentIndex].Progress = e.ProgressPercentage;
                    _localDb[currentIndex].Status = e.StatusText;
                }
            }));
        }

        public MainWindow()
        {
            InitializeComponent();
            _localDb = new ConvertTaskDataSource();
            dataGrid.DataContext = _localDb;
            dataGrid.ItemsSource = _localDb;
            currentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => ResetAll()));
        }
         
        private async void AddFilesGrid(string[] inputFiles)
        {
            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                cmdAddFiles.IsEnabled = false;
                cmdClearAll.IsEnabled = false;
            }));

            var hasDouble = false;
            await Task.Factory.StartNew(() =>
            {
                foreach (string fname in inputFiles)
                {
                    if (_localDb.HasFilepath(fname))
                    {
                        hasDouble = true;
                        continue;
                    }
                    currentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => _localDb.AddTask(fname)));
                }
            });

            if (hasDouble)
                await this.ShowMessageAsync("Duplicate file(s)", "Same file(s) are not added to queue.");

            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                cmdAddFiles.IsEnabled = true;
                cmdClearAll.IsEnabled = true;
            }));
        }

        private void cmdAddFiles_Click(object sender, RoutedEventArgs e)
        {
            var vwOpen = new OpenFileDialog()
            {
                Title = "Select input video.",
                DefaultExt = "*.mkv",
                Filter = "Matroska Video File|*.mkv",
                Multiselect = true,
                FileName = "",
            };
            var result = vwOpen.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                AddFilesGrid(vwOpen.FileNames);
            }
        }

        private void cmdSettings_Click(object sender, RoutedEventArgs e)
        {
            var vwSettings = new SettingsWindow();
            vwSettings.ShowDialog();
        }

        private async void cmdStartConvert_Click(object sender, RoutedEventArgs e)
        {
            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                cmdAddFiles.IsEnabled = false;
                cmdClearAll.IsEnabled = false;
                cmdStartConvert.IsEnabled = false;
                cmdStopConvert.IsEnabled = true;
                cmdSettings.IsEnabled = false;
            }));
            if (_localDb.Count == 0)
            {
                await this.ShowMessageAsync("Nothing to do", "There is no items in queue.");
                return;
            }

            currentIndex = 0;
            ffmpegWorker.StartAction(Helpers.Clone(_localDb[currentIndex]));
        }

        private void cmdStopConvert_Click(object sender, RoutedEventArgs e)
        {
            currentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                cmdAddFiles.IsEnabled = false;
                cmdClearAll.IsEnabled = true;
                cmdStartConvert.IsEnabled = false;
                cmdStopConvert.IsEnabled = false;
                cmdSettings.IsEnabled = true;
            }));
            ffmpegWorker.StopAction();
        }

        private void cmdAbout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmdClearAll_Click(object sender, RoutedEventArgs e)
        {
            ResetAll();
        }

        #region Data Grid
        private void dataGrid_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                e.Handled = true;
        }

        private void mnuRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;
            _localDb.Remove((ConvertTaskEntry)dataGrid.SelectedItem);
        }

        private void mnuSubtitleFile_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;
            var openSubDialog = new OpenFileDialog()
            {
                Title = "Select subtitle file.",
                DefaultExt = "*.ass;*.saa",
                Filter = "SubStation Alpha (ASS, SAA)|*.saa;*.ass|SubRip (SRT)|*.srt",
                Multiselect = false,
                FileName = "",
            };
            var result = openSubDialog.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                _localDb[dataGrid.SelectedIndex].SubtitleFilepath = openSubDialog.FileName;
            }
        }

        private void mnuRemoveSub_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;
            _localDb[dataGrid.SelectedIndex].SubtitleFilepath = "Unassigned";
        }

        private async void mnuUseEmbedded_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;
            if (Helpers.HasEmbeddedSubtitle(_localDb[dataGrid.SelectedIndex].InputFilepath))
            {
                _localDb[dataGrid.SelectedIndex].SubtitleFilepath = "Embedded";
            }
            else
            {
                await this.ShowMessageAsync("No subtitle", "No embedded subtitle file associated with this file.");
            }
        }
        #endregion


    }
}
