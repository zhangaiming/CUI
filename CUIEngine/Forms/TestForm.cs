using System;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;

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
            
            Panel w1 = new Panel(new Vector2Int(5, 5), new Vector2Int(5, 5), this, "w1")
            {
                FillColor = new ColorPair(CUIColor.Blue),
                DrawType = PanelDrawType.FillOnly
            };
            Panel w2 = new Panel(new Vector2Int(5, 5), new Vector2Int(17, 11), this, "w2")
            {
                FillColor = new ColorPair(CUIColor.Green),
                DrawType = PanelDrawType.FillOnly
            };

            Input.AttachHandler(MoveFormDown, ConsoleKey.S);
        }

        void MoveFormDown(ConsoleKeyInfo info)
        {
            this.Coord += new Vector2Int(0, 1);
        }

        public TestForm(Vector2Int coord, string name, string tag = "") : base(coord, name, tag)
        {
        }
    }
}