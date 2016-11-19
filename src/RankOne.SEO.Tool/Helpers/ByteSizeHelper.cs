using System;
using System.Text;

namespace RankOne.Helpers
{
    public class ByteSizeHelper
    {
        private readonly string[] _sizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public int GetByteSize(string text)
        {
            if (text == null)
            {
                return 0;
            }
            return Encoding.ASCII.GetByteCount(text);
        }

        public string GetSizeSuffix(int value)
        {
            if (value < 0) { return "-" + GetSizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, _sizeSuffixes[mag]);
        }
    }
}
