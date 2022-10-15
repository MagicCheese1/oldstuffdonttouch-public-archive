using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace sbdgdfghbf
{
    public class Client
    {
        public Process process{get; private set;}
        public string Username{get; private set;}

        private int xAddress{get; set;}
        private int yAddress{get; set;}
        private int zAddress{get; set;} 
        private int jumpAddress{get; set;}
        public float x{
        get{
            var buffer = new byte[4];
            var bufferRead = 0;
            Memory.ReadProcessMemory((int)process.Handle, xAddress, buffer, buffer.Length, ref bufferRead);
            return BitConverter.ToSingle(buffer, 0);
        }
        private set{
            this.x = value;
        }}
        public float y{get{
            var buffer = new byte[4];
            var bufferRead = 0;
            Memory.ReadProcessMemory((int)process.Handle, yAddress, buffer, buffer.Length, ref bufferRead);
            return BitConverter.ToSingle(buffer, 0);
        }
        private set{
            this.y = value;
        }}
        public float z{get{
            var buffer = new byte[4];
            var bufferRead = 0;
            Memory.ReadProcessMemory((int)process.Handle, zAddress, buffer, buffer.Length, ref bufferRead);
            return BitConverter.ToSingle(buffer, 0);
        }
        private set{
            this.z = value;
        }}
        public float jump {get{
            var buffer = new byte[4];
            var bufferRead = 0;
            Memory.ReadProcessMemory((int)process.Handle, jumpAddress, buffer, buffer.Length, ref bufferRead);
            return BitConverter.ToSingle(buffer, 0);
        }
        private set{
            this.jump = value;
        }}
        public Client(Process process)
        {
            this.process = process;
            xAddress = Memory.GetAddress(process, 0x00ECEEF0, new int[] {0x0, 0x28, 0xC4, 0x4, 0x60});
            //Console.WriteLine(xAddress);
            yAddress = Memory.GetAddress(process, 0x00ECEEF0, new int[] {0x0, 0x28, 0xC4, 0x4, 0x64});
            //Console.WriteLine(yAddress);
            zAddress = Memory.GetAddress(process, 0x00ECEEF0, new int[] {0x0, 0x28, 0xC4, 0x4, 0x68});
            //Console.WriteLine(zAddress);
            jumpAddress = Memory.GetAddress(process, 0x00ECEEF0, new int[] {0x0, 0x28, 0xC4, 0x4, 0x94});
            this.Username = Program.GetUsername(process);
        }

        public void setPositon(float x, float y, float z, float jump)
        {
            var buffer = new byte[4];
            var bufferRead = 0;
            //Memory.ReadProcessMemory((int)process.Handle, xAddress, buffer, buffer.Length, ref bufferRead);
            buffer = BitConverter.GetBytes(x);
            Memory.WriteProcessMemory((int)process.Handle, xAddress, buffer, buffer.Length, ref bufferRead);
            //Memory.ReadProcessMemory((int)process.Handle, yAddress, buffer, buffer.Length, ref bufferRead);
            buffer = BitConverter.GetBytes(y);
            Memory.WriteProcessMemory((int)process.Handle, yAddress, buffer, buffer.Length, ref bufferRead);
            //Memory.ReadProcessMemory((int)process.Handle, zAddress, buffer, buffer.Length, ref bufferRead);
            buffer = BitConverter.GetBytes(z);
            Memory.WriteProcessMemory((int)process.Handle, zAddress, buffer, buffer.Length, ref bufferRead);
            buffer = BitConverter.GetBytes(jump);
            Memory.WriteProcessMemory((int)process.Handle,jumpAddress, buffer, buffer.Length, ref bufferRead);
            //readPositon();
            //Console.WriteLine($"sx: {this.x}, sy: {this.y}, sz: {this.z}");
        }

        public void setUsername()
        {
            var buffer = new byte[Username.Length+1];
            var bufferRead = 0;
            buffer = Encoding.ASCII.GetBytes(Username + "1");
            Memory.WriteProcessMemory((int)process.Handle, Memory.GetAddress(process, 0x00ECEEF0, new int[] {0x8, 0x28, 0x1A4, 0x3E8, 0x19C, 0x0}), buffer, buffer.Length, ref bufferRead);
        }
    }
}