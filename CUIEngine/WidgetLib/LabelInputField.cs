using System;
using System.Text;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;
//todo: 控件未完成
namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 文本编辑框
    /// </summary>
    public abstract class LabelEditField : Label, IInteractive
    {
        bool editing = false;

        ColorPair normalColorPair;

        StringBuilder content = new StringBuilder();
        int currentIndex = 0;
        int curMinIndex = 0;

        /// <summary>
        /// 文本输入框的内容
        /// </summary>
        public string Content
        {
            get => content.ToString();
            set
            {
                content = new StringBuilder(value);
                SetCurrentIndex(content.Length - 1);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            AttachKeys();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            DetachKeys();
        }

        public void Select(bool state)
        {
            if (state)
            {
                normalColorPair = TextColorPair;
                TextColorPair = new ColorPair(Settings.ActiveForegroundCUIColor, Settings.ActiveBackgroundCUIColor);
            }
            else
            {
                TextColorPair = normalColorPair;
            }
        }

        public void Interact()
        {
            editing = !editing;
            if (editing)
            {
                SetCurrentIndex(content.Length);
                Cursor.Show();
            }else
            {
                Cursor.Hide();
            }
        }

        void AttachKeys()
        {
            Input.AttachHandler(OnInput);
            Input.AttachHandler(MoveCursor, ConsoleKey.LeftArrow);
            Input.AttachHandler(MoveCursor, ConsoleKey.RightArrow);
        }

        void DetachKeys()
        {
            Input.DetachHandler(OnInput);
            Input.DetachHandler(MoveCursor, ConsoleKey.LeftArrow);
            Input.DetachHandler(MoveCursor, ConsoleKey.RightArrow);
        }

        void OnInput(ConsoleKeyInfo info)
        {
            if (editing)
            {
                char c = info.KeyChar;
                if(c != '\0')
                {
                    //删除
                    if (info.Key == ConsoleKey.Backspace)
                    {
                        if (currentIndex >= 1 && content.Length >= 1)
                        {
                            content.Remove(currentIndex - 1, 1);
                            SetCurrentIndex(currentIndex - 1);
                        }

                        return;
                    }
                    content.Insert(currentIndex, c);
                    SetCurrentIndex(currentIndex + 1);
                }
            }
        }

        /// <summary>
        /// 移动光标
        /// </summary>
        /// <param name="info"></param>
        void MoveCursor(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.RightArrow)
            {
                SetCurrentIndex(currentIndex + 1);
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                SetCurrentIndex(currentIndex - 1);
            }
        }

        /// <summary>
        /// 设置当前光标所在索引
        /// </summary>
        /// <param name="target"></param>
        void SetCurrentIndex(int target)
        {
            int newIndex = Math.Clamp(target, 0, content.Length);
            int cursorX = 0;
            int cursorY = 0;

            //光标右移
            if (newIndex > currentIndex)
            {
                if (newIndex > curMinIndex + Size.X)
                {
                    cursorX = Coord.X + Size.X;
                    cursorY = Coord.Y;
                    curMinIndex = newIndex - Size.X;
                }
                else
                {
                    cursorX = Coord.X + newIndex - curMinIndex;
                    cursorY = Coord.Y;
                }
            }
            //光标左移
            else if (newIndex < currentIndex)
            {
                if (newIndex < curMinIndex)
                {
                    //从左边出格
                    cursorX = Coord.X;
                    cursorY = Coord.Y;
                    curMinIndex = newIndex;
                }
                else
                {
                    cursorX = Coord.X + newIndex - curMinIndex;
                    cursorY = Coord.Y;
                }
            }
            Cursor.SetCursor(new Vector2Int(cursorX, cursorY));
            currentIndex = newIndex;
            Content = Content.Substring(curMinIndex, Math.Min(Content.Length - curMinIndex - 1, Size.X));
            UpdateRenderClip();
        }
    }
}