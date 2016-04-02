using System;
using System.ComponentModel;
using System.IO;

namespace MKVhardsubWPF.Model
{ 
    [Serializable()]
    public class ConvertTaskEntry : INotifyPropertyChanged
    {
        private int _progress;
        private string _status, _subPath;

        public string Filename
        {
            get { return Path.GetFileName(InputFilepath); }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                NotifyPropertyChanged("Progress");
            }
        }

        public string SubtitleFilename
        {
            get
            {
                if (SubtitleFilepath.Trim().Length > 1)
                    return Path.GetFileName(SubtitleFilepath);
                else
                    return "Embedded";
            }
        }

        public string InputFilepath { get; set; }

        public string SubtitleFilepath {
            get { return _subPath; }
            set
            {
                _subPath = value;
                NotifyPropertyChanged("SubtitleFilepath");
            }
        }

        #region INotifyChanged Implementation
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
