using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace sbdgdfghbf
{
    class Program
    {
        

        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        static void Main(string[] args)
        {
            Process[] processlist = Process.GetProcesses();
            List<Client> trovelist = new List<Client>();
            Client Primary;
            List<Client> Secondary = new List<Client>();

            foreach (Process process in processlist)
            {
                if (process.ProcessName.ToLower().Trim() == ("trove"))
                {

                    //Console.WriteLine("Process: {0} ID: {1}", process.ProcessName, process.Id);

                    trovelist.Add(new Client(process));

                }
            }
            int x = 0;
            foreach (Client trove in trovelist)
            {               
                x++;
                Console.WriteLine("{3}.process: {0}-{1}.exe, Username: {2}",trove.process.Id, trove.process.ProcessName, trove.Username, x);
            }
            while(true){
            Console.Write("Choose Primary:");
            var input = Console.ReadLine();
            uint i;
                if(uint.TryParse(input.Trim(), out i) && i <= trovelist.Count) // TODO: Debug
                {
                    Primary = trovelist[(int)i-1];
                    foreach( var trove in trovelist)
                    {
                        if(trove != Primary){
                        Secondary.Add(trove);
                        }
                    }
                    break;
                }
            }
            while(true){
                //Console.WriteLine($"px: {Primary.x}, py: {Primary.y}, pz: {Primary.z}");
                /*KeyInput.ControlSendMessage(Secondary[0].process,
                    KeyInput.VirtualKeyStates.VK_1,
                    false
                );               */

                //KeyInput.SendChar(Secondary[0].process, 'W');
                foreach(var Client in Secondary)
                { 
                    Client.setPositon(Primary.x, Primary.y, Primary.z, Primary.jump);   
                }
                Thread.Sleep(1000);
                     
            }
        } 

        public static string GetUsername(Process process)
        {
            var buffer = new byte[32];
            var bufferRead = 0;
            Memory.ReadProcessMemory((int)process.Handle, Memory.GetAddress(process, 0x00ECEEF0, new int[] {0x8, 0x28, 0x1A4, 0x3E8, 0x19C, 0x0}), buffer, buffer.Length, ref bufferRead);
            string value = Encoding.ASCII.GetString(buffer);
            
            string[] value1 = value.Split('\0');
            value = value1[0];
            return value;
        }
    }


}
