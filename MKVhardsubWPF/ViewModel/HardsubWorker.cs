using MKVhardsubWPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MKVhardsubWPF.ViewModel
{
    class HardsubWorker
    {
        Regex _timePattern = new Regex(@"time=(\d{2,}:\d{2,}:\d{2,}.\d{2,})", RegexOptions.Compiled);
        Regex _durationPattern = new Regex(@"Duration: (\d{2,}:\d{2,}:\d{2,}.\d{2,})", RegexOptions.Compiled);

        Process _internalProcess = null;
        ConvertTaskEntry _currentEntry = null;
        private string _workingDirectory = "";
        private int _currentItemIndex = 0;
        private bool _isCancelled = false;

        TimeSpan _currentMaxDuration;
        double _currentMaxSecond;

        public event EventHandler<MProgressChangedEventArgs> ProgressChanged;
        public event EventHandler ActionCompleted;

        public HardsubWorker()
        {
            
        }

        private void FFMPEG_Exited(object sender, EventArgs e)
        {
            var subExt = Path.GetExtension(_currentEntry.SubtitleFilepath);
            var outPath = Path.Combine(_workingDirectory, "subtitleTemp." + subExt);
            File.Delete(outPath);
            NativeMethods.UninstallFontDirectory("C:\\fontTemp");

            if (Directory.Exists("C:\\fontTemp"))
                Directory.Delete("C:\\fontTemp", true);

            _internalProcess.OutputDataReceived -= FFMPEG_DataReceived;
            _internalProcess.ErrorDataReceived -= FFMPEG_DataReceived;
            _internalProcess.Exited -= FFMPEG_Exited;

            if (ActionCompleted == null) return;
            ActionCompleted(this, EventArgs.Empty);
        }

        private void FFMPEG_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;

            Match durationMatch = _durationPattern.Match(e.Data);
            if (durationMatch.Success)
            {
                _currentMaxDuration = TimeSpan.Parse(durationMatch.Groups[1].Value);
                _currentMaxSecond = _currentMaxDuration.TotalSeconds;
            }

            Match timeMatch = _timePattern.Match(e.Data);
            if (timeMatch.Success)
            {
                TimeSpan currentTime = TimeSpan.Parse(timeMatch.Groups[1].Value);
                double currentSeconds = currentTime.TotalSeconds;
                RaiseProgressChanged("Encoding video...", (int)((currentSeconds / _currentMaxSecond) * 100));
            }
        }

        private void RaiseProgressChanged(string status, int progress)
        {
            if (ProgressChanged == null) return;
            var args = new MProgressChangedEventArgs();

            if (!_isCancelled)
            {
                args.ProgressPercentage = progress;
                if (progress == 100)
                    args.StatusText = "Completed.";
                else
                    args.StatusText = status;
            }
            else
            {
                args.ProgressPercentage = 0;
                args.StatusText = "Cancelled";
            }
            ProgressChanged(this, args);
        }

        public void StartAction(ConvertTaskEntry input)
        {
            _currentEntry = input;

            RaiseProgressChanged("Extracting fonts...", 0);
            //Task.Factory.StartNew(() =>
            //{
                if (Helpers.ExtractFonts(_currentEntry.InputFilepath, "C:\\fontTemp"))
                    NativeMethods.InstallFontDirectory("C:\\fontTemp");
            //});

            RaiseProgressChanged("Getting ready to encode video...", 0);
            _workingDirectory = Path.GetDirectoryName(input.InputFilepath);

            //Configure FFMPEG process
            _internalProcess = new Process();
            var startArgs = new ProcessStartInfo()
            {
                FileName = Path.Combine(Helpers.GetBinariesPath(), "ffmpeg.exe"),
                Arguments = BuildArgumentString(),

                CreateNoWindow = true,
                ErrorDialog = false,

                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,

                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = _workingDirectory,
            };
            _internalProcess.StartInfo = startArgs;

            //Configure Redirection
            _internalProcess.OutputDataReceived += FFMPEG_DataReceived;
            _internalProcess.ErrorDataReceived += FFMPEG_DataReceived;
            _internalProcess.Exited += FFMPEG_Exited;
            _internalProcess.EnableRaisingEvents = true;

            //Start
            _internalProcess.Start();
            _internalProcess.BeginErrorReadLine();
            _internalProcess.BeginOutputReadLine();
        }

        public void StopAction()
        {
            _isCancelled = true;
            if (!_internalProcess.HasExited)
            {
                _internalProcess.StandardInput.Write("q");
                Thread.Sleep(500);
                _internalProcess.Kill();
            }
            RaiseProgressChanged("Cancelled.", 100);
        }

        private string BuildArgumentString()
        {
            var settings = Properties.Settings.Default;
            var fname = Path.GetFileNameWithoutExtension(_currentEntry.InputFilepath);
            var strFormat = "";

            if (_currentEntry.SubtitleFilepath == "Unassigned")
            {
                strFormat = "-y -i \"{0}\" -preset {1} -crf {2} \"{3}.mp4\"";
                return string.Format(strFormat, _currentEntry.Filename, settings.Preset, settings.CrfValue, fname);
            }
            else
            {
                var subFilename = "";
                if (_currentEntry.SubtitleFilepath == "Embedded")
                {
                    subFilename = Helpers.ExtractSubtitle(_currentEntry.InputFilepath, _workingDirectory);
                }
                else
                {
                    var subExt = Path.GetExtension(_currentEntry.SubtitleFilepath);
                    var outPath = Path.Combine(_workingDirectory, "subtitleTemp." + subExt);
                    File.Copy(_currentEntry.SubtitleFilepath, outPath, true);
                    subFilename = Path.GetFileName(outPath);
                }

                strFormat = "-y -i \"{0}\" -vf \"subtitles={1}\" -preset {2} -crf {3} \"{4}.mp4\"";
                return string.Format(strFormat, _currentEntry.Filename, subFilename, settings.Preset, settings.CrfValue, fname);
            }
        }


    }
}
