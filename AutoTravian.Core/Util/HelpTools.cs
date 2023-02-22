using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.Core.Util
{
    internal class HelpTools
    {
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int int_4, string string_0, int int_5, int int_6);
        [DllImport("urlmon.dll")]
        [return: MarshalAs(UnmanagedType.Error)]
        private static extern int CoInternetSetFeatureEnabled(int int_4, [MarshalAs(UnmanagedType.U4)] int int_5, bool bool_0);
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetSetOption(IntPtr intptr_0, int int_4, IntPtr intptr_1, int int_5);
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();

        public static void CoInternetSetFeatureEnabled()
        {
            HelpTools.CoInternetSetFeatureEnabled(21, 2, true);
        }

        public static void UrlMkSetSessionOption(string string_0)
        {
            HelpTools.UrlMkSetSessionOption(268435457, string_0, string_0.Length, 0);
        }
    }
}
