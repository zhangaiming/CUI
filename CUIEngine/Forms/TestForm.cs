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
            base.OnInitialize();
            Title = "A Form";
            
            TestWidget w1 =
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), new Vector2Int(5, 5), "w1", this);
            w1.BackColor = ConsoleColor.Blue;
            Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), new Vector2Int(7, 7), "w2", this);
            TestWidget? w2 = Widget.Find<TestWidget>("w2");
            w2!.BackColor = ConsoleColor.Green;
            
            Input.AttachHandler(MoveFormDown, ConsoleKey.S);
        }

        void MoveFormDown()
        {
            this.Coord += new Vector2Int(0, 1);
        }
    }
}