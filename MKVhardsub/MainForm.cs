//   MKVhardsub, create hardsubbed videos.
//   Copyright(C) 2016  Fahmi Noor Fiqri
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//   GNU General Public License for more details.
///
//   You should have received a copy of the GNU General Public License
//   along with this program.If not, see<http://www.gnu.org/licenses/>.

using gMKVToolnix;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MKVhardsub
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Regex timePattern = new Regex(@"time=(\d{2,}:\d{2,}:\d{2,}.\d{2,})");
        Regex durationPattern = new Regex(@"Duration: (\d{2,}:\d{2,}:\d{2,}.\d{2,})");
        TimeSpan _currentMaxDuration;
        double _currentMaxSecond;
        string _currentSubtitleFile = "";
        bool _isSubtitleEmbedded = false;

        private delegate void ChangeStatusTextDelegate(string status, int value);

        #region Form Presenter
        private void ChangeStatusTexts(string status, int value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new ChangeStatusTextDelegate(ChangeStatusTexts), status, value);
            }
            else
            {
                lblStatus.Text = status;
                if (value < 0)
                {
                    prgStatus.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    prgStatus.Style = ProgressBarStyle.Blocks;
                    prgStatus.Value = value;
                }
            }
        }

        private void ResetAll()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ResetAll));
            }
            else
            {
                _currentSubtitleFile = "";
                _currentMaxDuration = TimeSpan.Zero;
                _currentMaxSecond = 0;
                txtInputMkv.Text = "";
                txtSubtitleFile.Text = "";

                cmdStart.Enabled = true;
                cmdStop.Enabled = false;

                prgStatus.Style = ProgressBarStyle.Blocks;
                prgStatus.Value = 0;
                lblStatus.Text = "Ready.";
            }
        }
        #endregion

        #region Command Buttons
        private void cmdStart_Click(object sender, EventArgs e)
        {
            var arg = new MkvTexts()
            {
                MkvInput = txtInputMkv.Text,
                SubtitleFile = txtSubtitleFile.Text,
                UseEmbedded = chkEmbedd.Checked,
            };
            bwFonts.RunWorkerAsync(arg);
            ChangeStatusTexts("Building font cache...", -1);

            cmdStart.Enabled = false;
            cmdStop.Enabled = true;
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            if (!ffmpeg.HasExited)
                ffmpeg.Kill();

            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
        }

        private void brwInputMkv_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ofd.DefaultExt = "*.mkv";
            ofd.Filter = "Matroska Video File (MKV)|*.mkv";
            ofd.Title = "Select mkv video to encode.";

            if (ofd.ShowDialog() != DialogResult.OK) return;
            txtInputMkv.Text = ofd.FileName;
        }

        private void brwSubtitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ofd.DefaultExt = "*.ass;*.saa";
            ofd.Filter = "ASS/SAA Subtitle|*.ass;*.saa";
            ofd.Title = "Select subtitle to add.";

            if (ofd.ShowDialog() != DialogResult.OK) return;
            txtSubtitleFile.Text = ofd.FileName;
        }
        #endregion

        #region Workers
        private void PrepareFonts(MkvTexts args)
        {
            var _mkvMerge = new gMKVMerge(Helpers.BinariesPath);
            var segments = _mkvMerge.GetMKVSegments(args.MkvInput);

            for (int iSegments = 0; iSegments < segments.Count; iSegments++)
            {
                if (segments[iSegments].GetType() == typeof(gMKVAttachment))
                {
                    Helpers.ExtractSegment(args.MkvInput, segments[iSegments], Helpers.FontsCachePath);
                }
                if (segments[iSegments].GetType() == typeof(gMKVTrack) && args.UseEmbedded)
                {
                    if ((segments[iSegments] as gMKVTrack).TrackType == MkvTrackType.subtitles)
                    {
                        Helpers.ExtractSegment(args.MkvInput, segments[iSegments], Path.GetDirectoryName(args.MkvInput));
                        _isSubtitleEmbedded = true;
                    }
                }
            }
        }

        private void bwFonts_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (MkvTexts)e.Argument;
            Directory.CreateDirectory(Helpers.FontsCachePath);
            PrepareFonts(args);
            if (!_isSubtitleEmbedded && args.UseEmbedded)
            {
                MessageBox.Show("No subtitle found!", "No subtitle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ResetAll();
                return;
            }
            NativeMethods.InstallFontDirectory(Helpers.FontsCachePath);

            _currentSubtitleFile = Helpers.GetSubtitleFile(args);

            var sb = new StringBuilder();
            sb.Append("-y -i \"");
            sb.Append(Path.GetFileName(args.MkvInput));
            sb.Append("\" -vf \"subtitles=");
            sb.Append(_currentSubtitleFile);
            sb.Append("\" -crf 18 \"");
            sb.Append(Path.GetFileNameWithoutExtension(args.MkvInput));
            sb.Append(".mp4\"");

            var startInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(Helpers.BinariesPath, "ffmpeg.exe"),
                CreateNoWindow = false,
                Arguments = sb.ToString(),
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(args.MkvInput),
                WindowStyle = ProcessWindowStyle.Normal,
            };
            ffmpeg.StartInfo = startInfo;
            ffmpeg.Start();
            ffmpeg.BeginErrorReadLine();
            ffmpeg.BeginOutputReadLine();
        }

        #region ffmpeg Process
        private void FFMPEG_OuputReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data == null) return;

            Match durationMatch = durationPattern.Match(e.Data);
            if (durationMatch.Success)
            {
                _currentMaxDuration = TimeSpan.Parse(durationMatch.Groups[1].Value);
                _currentMaxSecond = _currentMaxDuration.TotalSeconds;
            }

            Match timeMatch = timePattern.Match(e.Data);
            if (timeMatch.Success)
            {
                TimeSpan currentTime = TimeSpan.Parse(timeMatch.Groups[1].Value);
                double currentSeconds = currentTime.TotalSeconds;
                ChangeStatusTexts("Encoding video...", (int)((currentSeconds / _currentMaxSecond) * 100));
            }
        }

        private void FFMPEG_Exited(object sender, EventArgs e)
        {
            NativeMethods.UninstallFontDirectory(Helpers.FontsCachePath);
            Directory.Delete(Helpers.FontsCachePath, true);
            ChangeStatusTexts("Completed.", 0);
            File.Delete(Path.Combine(Path.GetDirectoryName(txtInputMkv.Text), _currentSubtitleFile));

            ResetAll();
        }
        #endregion
        #endregion

    }
}
