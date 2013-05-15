using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Krile2Eject
{
    public static class Eject
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string lpszCommand, StringBuilder lpszReturnString, uint cchReturn, IntPtr hwndCallback);

        public static bool CanEject()
        {
            return mciSendString("capability cdaudio can eject", null, 0, IntPtr.Zero) == 0;
        }

        public static bool Open()
        {
            return mciSendString("set cdaudio door open", null, 0, IntPtr.Zero) == 0;
        }

        public static bool Close()
        {
            return mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero) == 0;
        }
    }
}
