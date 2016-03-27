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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MKVhardsub
{
    static class NativeMethods
    {
        public const int WM_FONTCHANGE = 29;
        private static IntPtr HWND_BROADCAST = new IntPtr(0xffff);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("gdi32.dll")]
        static extern int AddFontResource(string lpFilename);

        [DllImport("gdi32.dll")]
        static extern bool RemoveFontResource(string lpFileName);

        public static void BroadcastFontChanged()
        {
            SendMessage(HWND_BROADCAST, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
        }

        public static void InstallFontDirectory(string directory)
        {
            var files = System.IO.Directory.GetFiles(directory, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                AddFontResource(file);
            }
            BroadcastFontChanged();
        }

        public static void UninstallFontDirectory(string directory)
        {
            var files = System.IO.Directory.GetFiles(directory, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                RemoveFontResource(file);
                System.IO.File.Delete(file);
            }
            BroadcastFontChanged();
        }
    }
}
