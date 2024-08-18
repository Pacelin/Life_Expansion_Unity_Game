using System.Runtime.InteropServices;

namespace UniOwl.Celestials
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct byte3
    {
        public readonly byte x;
        public readonly byte y;
        public readonly byte z;
        
        public byte3(in byte x, in byte y, in byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}