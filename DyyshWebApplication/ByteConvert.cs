using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DyyshWebApplication
{
    public class ByteConvert
    {
        public static double BytesToKiloBytes(double bytes)
        {
            return bytes / 1024.0;
        }

        public static double BytesToKiloBytes(int bytes)
        {
            return (double)bytes / 1024.0;
        }

        public static double BytesToMegaBytes(double bytes)
        {
            return bytes / 1024.0 / 1024.0;
        }

        public static double BytesToMegaBytes(int bytes)
        {
            return (double)bytes / 1024.0 / 1024.0;
        }

    }
}