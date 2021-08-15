using BasicWidgetLib.Widgets;
using CUIEngine.Render;
using CUIEngine.Scripts;
using CUIEngine.Scripts.ScriptAttribute;

namespace BasicWidgetLib.Scripts
{
    [TargetWidget(typeof(TextBox))]
    public class SelectableText : Script
    {
        ColorPair normalColor = new ColorPair(CUIColor.DarkGray, CUIColor.Black);
        ColorPair highlightedColor = new ColorPair(CUIColor.Black, CUIColor.DarkCyan);

        bool selected = false;

        TextBox textBox = null!;

        /// <summary>
        /// 文本未被选中时的颜色
        /// </summary>
        public ColorPair NormalColor
        {
            get => normalColor;
            set => normalColor = value;
        }

        /// <summary>
        /// 文本被选中时的颜色
        /// </summary>
        public ColorPair SelectedColor
        {
            get => highlightedColor;
            set => highlightedColor = value;
        }

        void Awake()
        {
            textBox = (TextBox)Owner;
            SetSelect(false);
        }

        public void SetSelect(bool select)
        {
            selected = select;
            if (select)
            {
                textBox.TextColor = SelectedColor.ForegroundColor;
                textBox.BackgroundColor = SelectedColor.BackgroundColor;
            }
            else
            {
                textBox.TextColor = normalColor.ForegroundColor;
                textBox.BackgroundColor = normalColor.BackgroundColor;
            }
        }
    }
}