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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MKVhardsub
{
    public class MkvTexts
    {
        public string MkvInput { get; set; }
        public string SubtitleFile { get; set; }
        public bool UseEmbedded { get; set; }
    }

    static class Helpers
    {
        private static string SubtitleFileName;

        public static string BinariesPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            }
        }

        public static string FontsCachePath
        {
            get
            {
                return @"C:\MKVhardsub-cache\Fonts";
            }
        }

        public static void ExtractSegment(string inputMkv, gMKVSegment segment, string outPath)
        {
            var _mkvExtract = new gMKVExtract(BinariesPath);
            var tempData = new List<gMKVSegment>();

            if (typeof(gMKVTrack) == segment.GetType() && (segment as gMKVTrack).TrackType== MkvTrackType.subtitles)
            {
                var gtrack = (gMKVTrack)segment;
                SubtitleFileName = string.Format("{0}_track{1}_{2}.{3}", Path.GetFileNameWithoutExtension(inputMkv), 
                                   gtrack.TrackNumber, gtrack.Language, GetSubtitleExt(gtrack));
            }

            tempData.Add(segment);
            _mkvExtract.ExtractMKVSegments(inputMkv, tempData, outPath, MkvChapterTypes.XML, TimecodesExtractionMode.NoTimecodes);
            tempData.Clear();
        }

        public static string GetSubtitleFile(MkvTexts args)
        {
            if (args.UseEmbedded)
            {
                var newFilename = Path.GetFileName(SubtitleFileName).Replace(Path.GetFileNameWithoutExtension(SubtitleFileName), "subtitle");
                var outPath = Path.Combine(Path.GetDirectoryName(args.MkvInput), newFilename);
                var inPath = Path.Combine(Path.GetDirectoryName(args.MkvInput), SubtitleFileName);
                File.Copy(inPath, outPath, true);
                File.Delete(inPath);
                return newFilename;
            }
            else
            {
                string ext = Path.GetExtension(args.SubtitleFile);
                File.Copy(args.SubtitleFile, Path.Combine(Path.GetDirectoryName(args.MkvInput), "subtitle." + ext), true);
                return "subtitle." + ext;
            }
        }

        private static string GetSubtitleExt(gMKVTrack argTrack)
        {
            string result = string.Empty;
            if (argTrack.CodecID.ToUpper().Contains("S_TEXT/UTF8"))
            {
                result = "srt";
            }
            else
            {
                if (argTrack.CodecID.ToUpper().Contains("S_TEXT/SSA"))
                {
                    result = "ass";
                }
                else
                {
                    if (argTrack.CodecID.ToUpper().Contains("S_TEXT/ASS"))
                    {
                        result = "ass";
                    }
                    else
                    {
                        if (argTrack.CodecID.ToUpper().Contains("S_TEXT/USF"))
                        {
                            result = "usf";
                        }
                        else
                        {
                            if (argTrack.CodecID.ToUpper().Contains("S_IMAGE/BMP"))
                            {
                                result = "sub";
                            }
                            else
                            {
                                if (argTrack.CodecID.ToUpper().Contains("S_VOBSUB"))
                                {
                                    result = "sub";
                                }
                                else
                                {
                                    if (argTrack.CodecID.ToUpper().Contains("S_HDMV/PGS"))
                                    {
                                        result = "sup";
                                    }
                                    else
                                    {
                                        if (argTrack.CodecID.ToUpper().Contains("S_KATE"))
                                        {
                                            result = "ogg";
                                        }
                                        else
                                        {
                                            result = "sub";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
