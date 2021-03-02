using System;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;
using CUIEngine.Widgets;

namespace CUIEngine.Forms
{
    public class TestForm : Form
    {
        static TestForm()
        {
            DefaultSize = new Vector2Int(20, 14);
        }
        
        protected override void OnInitializeForm()
        {
            base.OnInitializeForm();
            Title = "A Form";
            
            ColorBlock w1 =
                Widget.CreateWidget<ColorBlock>(new Vector2Int(5, 5), new Vector2Int(5, 5), "w1", this);
            w1.Color = CUIColor.Blue;
            Widget.CreateWidget<ColorBlock>(new Vector2Int(5, 5), new Vector2Int(17, 11), "w2", this);
            ColorBlock? w2 = Widget.Find<ColorBlock>("w2");
            w2!.Color = CUIColor.Green;
            
            Input.AttachHandler(MoveFormDown, ConsoleKey.S);
        }

        void MoveFormDown(ConsoleKeyInfo info)
        {
            this.Coord += new Vector2Int(0, 1);
        }
    }
}