using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SsmsLogin
{
    /// <summary>
    /// Methods for interacting with the user32.dll APIs.
    /// </summary>
    internal static class User32
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lParam);

        public static IEnumerable<IntPtr> EnumWindows()
        {
            List<IntPtr> children = new List<IntPtr>();

            EnumWindows((cptr, lParam) =>
            {
                children.Add(cptr);
                return true;
            }, IntPtr.Zero);

            return children;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsDelegate lpEnumFunc, IntPtr lParam);

        public static IEnumerable<IntPtr> EnumChildWindows(IntPtr hWndParent)
        {
            List<IntPtr> children = new List<IntPtr>();

            EnumChildWindows(hWndParent, (cptr, lParam) =>
            {
                children.Add(cptr);
                return true;
            }, IntPtr.Zero);

            return children;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWind, StringBuilder lpString, int nMaxCount);

        public static string GetWindowTextString(IntPtr hWind)
        {
            int length = GetWindowTextLength(hWind);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWind, sb, sb.Capacity);
            return sb.ToString();
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWind, out IntPtr lpdwProcessId);

        public delegate bool EnumWindowsDelegate(IntPtr hwnd, int lParam);
    }
}
