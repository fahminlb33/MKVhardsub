using MKVhardsubWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKVhardsubWPF.Model
{
    public class ConvertTaskDataSource : ObservableCollectionEx<ConvertTaskEntry>
    {
        private object LockObject = new object();

        public void AddTask(string mkvInput)
        {
            lock (LockObject)
            {
                var newData = new ConvertTaskEntry()
                {
                    InputFilepath = mkvInput,
                    Progress = 0,
                    Status = "Queued",
                };

                //Check subtitle
                if (Helpers.HasEmbeddedSubtitle(mkvInput))
                    newData.SubtitleFilepath = "Embedded";
                else
                    newData.SubtitleFilepath = "Unassigned";

                base.Add(newData);
            }   
        }

        public bool HasFilepath(string inputFile)
        {
            return this.Where(x => x.InputFilepath.ToLowerInvariant() == inputFile.ToLowerInvariant()).Count() != 0;
        }
    }
}
