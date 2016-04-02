using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MKVhardsubWPF.ViewModel
{
    class NativeMethods
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
            if (!Directory.Exists(directory)) return;
            var files = Directory.GetFiles(directory, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                AddFontResource(file);
            }
            BroadcastFontChanged();
        }

        public static void UninstallFontDirectory(string directory)
        {
            if (!Directory.Exists(directory)) return;
            var files = Directory.GetFiles(directory, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                RemoveFontResource(file);
                System.IO.File.Delete(file);
            }
            BroadcastFontChanged();
        }
    }
}
