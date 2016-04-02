using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gMKVToolnix;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MKVhardsubWPF.ViewModel
{
    class Helpers
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", "source");

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
                return default(T);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static bool HasEmbeddedSubtitle(string mkvFilePath)
        {
            try
            {
                var mkvCheck = new gMKVMerge(GetBinariesPath());
                var segments = mkvCheck.GetMKVSegments(mkvFilePath);

                for (int iSegments = 0; iSegments < segments.Count; iSegments++)
                {
                    if (segments[iSegments].GetType() == typeof(gMKVTrack))
                    {
                        if ((segments[iSegments] as gMKVTrack).TrackType == MkvTrackType.subtitles)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string ExtractSubtitle(string mkvInput, string outPath)
        {
            var mkvCheck = new gMKVMerge(GetBinariesPath());
            var segments = mkvCheck.GetMKVSegments(mkvInput);
            gMKVTrack segExport = (gMKVTrack)segments.Where(x => typeof(gMKVTrack) == x.GetType() && (x as gMKVTrack).TrackType == MkvTrackType.subtitles).First();

            ExtractMkvSegment(mkvInput, segExport, outPath);
            var oName = string.Format("{0}_track{1}_{2}.{3}", Path.GetFileNameWithoutExtension(mkvInput),
                   segExport.TrackNumber, segExport.Language, GetSubtitleExt(segExport));
            try
            {
                File.Delete(Path.Combine(outPath, "subtitle." + GetSubtitleExt(segExport)));
            }
            catch (Exception ex)
            {
            }
            
            File.Move(Path.Combine(outPath, oName), Path.Combine(outPath, "subtitle." + GetSubtitleExt(segExport)));
            return "subtitle." + GetSubtitleExt(segExport);
        }

        public static bool ExtractFonts(string mkvInput, string outPath)
        {
            try
            {
                var mkvCheck = new gMKVMerge(GetBinariesPath());
                var segments = mkvCheck.GetMKVSegments(mkvInput);
                var segExport = segments.Where(x => x.GetType() == typeof(gMKVAttachment)).ToList();

                ExtractMkvSegment(mkvInput, segExport, outPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string GetBinariesPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
        }

        #region MKVToolNix
        public static void ExtractMkvSegment(string mkvInput, gMKVSegment segment, string outPath)
        {
            ExtractMkvSegment(mkvInput, new List<gMKVSegment>() { segment }, outPath);
        }

        public static void ExtractMkvSegment(string mkvInput, List<gMKVSegment> segments, string outPath)
        {
            var mkvExtract = new gMKVExtract(GetBinariesPath());
            Directory.CreateDirectory(outPath);
            mkvExtract.ExtractMKVSegments(mkvInput, segments, outPath, MkvChapterTypes.XML, TimecodesExtractionMode.NoTimecodes);
        }

        private static string GetSubtitleExt(gMKVTrack argTrack)
        {
            string result = string.Empty;
            string codecID = argTrack.CodecID.ToUpper();
            if (codecID.Contains("S_TEXT/UTF8"))
            {
                result = "srt";
            }
            else if (codecID.Contains("S_TEXT/SSA"))
            {
                result = "ass";
            }
            else if (codecID.Contains("S_TEXT/ASS"))
            {
                result = "ass";
            }
            else if (codecID.Contains("S_TEXT/USF"))
            {
                result = "usf";
            }
            else if (codecID.Contains("S_IMAGE/BMP"))
            {
                result = "sub";
            }
            else if (codecID.Contains("S_VOBSUB"))
            {
                result = "sub";
            }
            else if (codecID.Contains("S_HDMV/PGS"))
            {
                result = "sup";
            }
            else if (codecID.Contains("S_KATE"))
            {
                result = "ogg";
            }
            else
            {
                result = "sub";
            }

            return result;
        }
        #endregion
    }
}
