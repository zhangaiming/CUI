using System;
using System.Text;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;
namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 文本编辑框
    /// </summary>
    public class LabelEditField : Label, IInteractive
    {
        bool editing = false;

        ColorPair normalColorPair;

        StringBuilder content = new StringBuilder();
        int currentIndex = 0;
        int textBeginIndex = 0;

        /// <summary>
        /// 文本输入框的内容
        /// </summary>
        public string Content
        {
            get => content.ToString();
            set
            {
                content = new StringBuilder(value);
                SetTextBeginIndex(content.Length - Size.X + 1);
            }
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
                normalColorPair = TextColor;
                TextColor = new ColorPair(Settings.ActiveForegroundCUIColor, Settings.ActiveBackgroundCUIColor);
            }
            else
            {
                TextColor = normalColorPair;
            }
        }

        public void Interact()
        {
            editing = !editing;
            if (editing)
            {
                SetTextBeginIndex(content.Length - Size.X + 1);
                UpdateCursorCoord();
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
                            SetTextBeginIndex(textBeginIndex - 1);
                        }
                        return;
                    }
                    content.Insert(currentIndex, c);
                    MoveCursor(true);
                }
            }
        }

        void MoveCursor(bool moveRight)
        {
            if (moveRight)
            {
                //右移光标
                if (currentIndex < content.Length)
                {
                    currentIndex += 1;
                    if (currentIndex > Size.X + textBeginIndex)
                    {
                        //从右边出格
                        SetTextBeginIndex(textBeginIndex + 1);
                    }
                }
            }
            else
            {
                //左移光标
                if (currentIndex > 0)
                {
                    currentIndex -= 1;
                    if (currentIndex < textBeginIndex)
                    {
                        //从左边出格
                        SetTextBeginIndex(textBeginIndex - 1);
                    }
                }
            }
            
            UpdateCursorCoord();
        }
        
        /// <summary>
        /// 移动光标
        /// </summary>
        /// <param name="info"></param>
        void MoveCursor(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.RightArrow)
            {
                MoveCursor(true);
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                MoveCursor(false);
            }
        }

        void UpdateCursorCoord()
        {
            //设置光标
            Cursor.SetCursor(new Vector2Int(currentIndex - textBeginIndex + Coord.X, Coord.Y));
        }

        /*/// <summary>
        /// 设置当前光标所在索引
        /// </summary>
        /// <param name="targetIndex"></param>
        void SetCurrentIndex(int targetIndex)
        {
            int newIndex = Math.Clamp(targetIndex, 0, content.Length);
            int cursorY = Coord.Y;

            //光标右移
            if (newIndex > currentIndex)
            {
                if (newIndex > textBeginIndex + Size.X)
                {
                    //从右边出格
                    cursorX = Coord.X + Size.X;
                    textBeginIndex = newIndex - Size.X;
                }
                else
                {
                    cursorX = Coord.X + newIndex - textBeginIndex;
                }
            }
            //光标左移
            else if (newIndex < currentIndex)
            {
                if (newIndex < textBeginIndex)
                {
                    //从左边出格
                    cursorX = Coord.X;
                }
                else
                {
                    cursorX = Coord.X + newIndex - textBeginIndex;
                }
            }
            Cursor.SetCursor(new Vector2Int(newIndex - textBeginIndex, cursorY));
            currentIndex = newIndex;
            UpdateRenderClip();
        }*/

        public LabelEditField(Vector2Int size, Vector2Int coord, IWidgetOwner parent, string name, string tag = "") : base(size, coord, parent, name, tag)
        {
            AttachKeys();
        }

        void SetTextBeginIndex(int index)
        {
            index = Math.Clamp(index, 0, content.Length);
            textBeginIndex = index;
            Text = Content.Substring(index, Math.Min(content.Length - textBeginIndex, Size.X));
        }
    }
}