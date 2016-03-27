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
