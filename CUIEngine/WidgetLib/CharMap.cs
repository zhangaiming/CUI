using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 字符图
    /// </summary>
    public class CharMap : Widget
    {
        string content = "";

        /// <summary>
        /// 字符图的文本内容,使用'\r'换行
        /// </summary>
        public string Content
        {
            get => content;
            set => content = value;
        }

        //todo: 没写完
        protected override void MakeRenderClip()
        {
            CurrentClip?.Clear();

            int x, y;
            
            string temp = content;
            for (var i = 0; i < temp.Length; i++)
            {
                char c = temp[i];
                //换行符
                if (c == 'r')
                {
                    
                }
            }
        }
    }
}