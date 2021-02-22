using System;
using CUIEngine.Forms;
using CUIEngine.Mathf;

namespace CUIEngine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();

            Form.Create<TestForm>(new Vector2Int(3, 3), "Form");

            Console.ReadKey();
            CUIEngine.Shutdown();
        }
    }
}