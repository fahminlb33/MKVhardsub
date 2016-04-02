using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKVhardsubWPF.ViewModel
{
    class MProgressChangedEventArgs : EventArgs
    {
        public int ProgressPercentage { get; set; }
        public string StatusText { get; set; }
    }
}
