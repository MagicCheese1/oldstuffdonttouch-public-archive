using System.Linq;
using System.Numerics;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

namespace sbdgdfghbf
{
    public class Memory
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
        int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
        byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
        
        public static int GetAddress(Process trove, int Pointer, int[] offsets)
        {
            var Base = (int)trove.MainModule.BaseAddress;
            var buffer = new Byte[4];
            var bufferRead = 0;
            var y = 0;
            
            var PointerBase = Base + Pointer;

            ReadProcessMemory((int)trove.Handle, PointerBase, buffer, buffer.Length, ref bufferRead);
            for (int x = 0; x < offsets.Length-1; x++)
            {
                y = BitConverter.ToInt32(buffer, 0) +  offsets[x];
                ReadProcessMemory((int)trove.Handle, (int)y, buffer, buffer.Length, ref bufferRead );
            }
            return BitConverter.ToInt32(buffer, 0) + offsets.Last();
        }
    }
}