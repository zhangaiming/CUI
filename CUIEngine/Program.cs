using System;
using System.Threading;
using CUIEngine.Forms;
using CUIEngine.Inputs;
using CUIEngine.Mathf;

namespace CUIEngine
{
    internal class Program
    {
        static bool isRunning = true;
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();

            Input.AttachHandler(Shutdown, ConsoleKey.Escape);
            
            Form form = Form.Create<TestForm>(new Vector2Int(5, 3), "Form");
            
            Thread.Sleep(2000);
            
            form.Size = new Vector2Int(10, 10);

            while (isRunning)
            {
                Thread.Sleep(1);
            }
            CUIEngine.Shutdown();
        }

        static void Shutdown()
        {
            isRunning = false;
        }
    }
}