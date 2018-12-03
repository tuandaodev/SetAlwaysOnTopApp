using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SetAlwaysOnTopApp
{
    class Methods
    {
        /******** For GetWindowTitle() *********************/
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        /*********** END ***********************************/

        /********* Setting Windows on Top ******************/

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr handle);


        /*
         * From another lib
         */
        //public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsWindowVisible(IntPtr hWnd);

        //[DllImport("user32.dll", EntryPoint = "GetWindowText",
        //    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        //[DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        //    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction,
        //   IntPtr lParam);



        const Int32 SW_HIDE = 0;
        const Int32 SW_SHOWNORMAL = 1;
        const Int32 SW_NORMAL = 1;
        const Int32 SW_SHOWMINIMIZED = 2;
        const Int32 SW_SHOWMAXIMIZED = 3;
        const Int32 SW_MAXIMIZE = 3;
        const Int32 SW_SHOWNOACTIVATE = 4;
        const Int32 SW_SHOW = 5;
        const Int32 SW_MINIMIZE = 6;
        const Int32 SW_SHOWMINNOACTIVE = 7;
        const Int32 SW_SHOWNA = 8;
        const Int32 SW_RESTORE = 9;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_SHOWWINDOW = 0x0040;
        const int SWP_NOACTIVATE = 0x0010;
        const int HWND_TOPMOST = -1;
        const int HWND_NOTOPMOST = -2;
        /*********** END ***********************************/


        public static String GetWindowTitle()
        {
            int i = 0;
            while (i < 1)
            {
                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                IntPtr handle = GetForegroundWindow();

                if (GetWindowText(handle, Buff, nChars) > 0)
                {
                    return Buff.ToString();
                }
            }
            return null;
        } // GetWindowTitle()

        public static void AoT_on(string title)
        {
            //Console.WriteLine(title);

            Process[] processes = Process.GetProcesses();

            //Process.GetProcesses().Any(p => p.MainWindowTitle.Equals(title));

            foreach (var process in processes)
            {
                string mWinTitle = process.MainWindowTitle.ToString();

                if (!string.IsNullOrEmpty(mWinTitle))
                {
                    Console.WriteLine(mWinTitle);
                }

                if (mWinTitle == title)
                {
                    IntPtr handle = process.MainWindowHandle;
                    if (handle != IntPtr.Zero)
                    {
                        Console.Write(handle);
                        SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                    }
                }

            }

            
        } // AoT_on()

        public static bool IsProcessOpen()
        {
            if (Process.GetProcessesByName("Always On Top - Quick Order").Length > 1)
            {
                // Is running
                return true;
            }
            return false;
        }


        public static void SetView(string title)
        {
            Console.WriteLine(title);

            Process[] processes = Process.GetProcesses(".");

            //Process.GetProcesses().Any(p => p.MainWindowTitle.Equals(title));

            foreach (var process in processes)
            {
                string mWinTitle = process.MainWindowTitle.ToString();
                if (mWinTitle == title)
                {
                    IntPtr handle = process.MainWindowHandle;
                    if (handle != IntPtr.Zero)
                    {

                        Console.Write(handle);

                        if (IsIconic(handle))
                        {
                            ShowWindow(handle, SW_RESTORE);
                        } else
                        {
                            ShowWindow(handle, SW_MINIMIZE);
                        }
                           
     
                    }
                }

            }
        } // AoT_on()


        public static void AoT_off(string title)
        {
            Process[] processes = Process.GetProcesses(".");
            foreach (var process in processes)
            {
                string mWinTitle = process.MainWindowTitle.ToString();
                if (mWinTitle == title)
                {
                    IntPtr handle = process.MainWindowHandle;
                    if (handle != IntPtr.Zero)
                    {
                        Console.Write(handle);
                        SetWindowPos(handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                    }
                }
            }
        } // AoT_off()

        //public static List<DesktopWindow> GetDesktopWindowsByTitle(string popup_name)
        //{
        //    var collection = new List<DesktopWindow>();
        //    EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
        //    {
        //        var result = new StringBuilder(255);
        //        GetWindowText(hWnd, result, result.Capacity + 1);
        //        string title = result.ToString();

        //        var isVisible = !string.IsNullOrEmpty(title) && IsWindowVisible(hWnd);

        //        if (isVisible && title == popup_name)
        //        {
        //            Console.WriteLine(title);
        //            Console.Write(hWnd);
        //            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        //        }

        //        collection.Add(new DesktopWindow { Handle = hWnd, Title = title, IsVisible = isVisible });

        //        return true;
        //    };

        //    EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero);
        //    return collection;
        //}


    }


    //public class DesktopWindow
    //{
    //    public IntPtr Handle { get; set; }
    //    public string Title { get; set; }
    //    public bool IsVisible { get; set; }
    //}

    //public class User32Helper
    //{

    //    public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

    //    [DllImport("user32.dll")]
    //    [return: MarshalAs(UnmanagedType.Bool)]
    //    public static extern bool IsWindowVisible(IntPtr hWnd);

    //    [DllImport("user32.dll", EntryPoint = "GetWindowText",
    //        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    //    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

    //    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
    //        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    //    public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction,
    //       IntPtr lParam);

    //    public static List<DesktopWindow> GetDesktopWindows()
    //    {
    //        var collection = new List<DesktopWindow>();
    //        EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
    //        {
    //            var result = new StringBuilder(255);
    //            GetWindowText(hWnd, result, result.Capacity + 1);
    //            string title = result.ToString();

    //            var isVisible = !string.IsNullOrEmpty(title) && IsWindowVisible(hWnd);

    //            if (isVisible)
    //            {
    //                Console.WriteLine(title);
    //            }

    //            collection.Add(new DesktopWindow { Handle = hWnd, Title = title, IsVisible = isVisible });

    //            return true;
    //        };

    //        EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero);
    //        return collection;
    //    }

    //}
}
