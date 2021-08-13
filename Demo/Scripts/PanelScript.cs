using System;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Scripts;

namespace CUITest.Scripts
{
    public class PanelScript : Script
    {
        void Awake()
        {
            Input.AttachHandler(Move);
        }

        void Move(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.A)
            {
                Owner.Coord += new Vector2Int(-1, 0);
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                Owner.Coord += new Vector2Int(1, 0);
            }
        }
    }
}