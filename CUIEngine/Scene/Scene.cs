using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.Scene
{
    /// <summary>
    /// 场景类,未实装
    /// </summary>
    public abstract class Scene
    {
        static Scene? loadedScene = null;

        /// <summary>
        /// 场景的根控件
        /// </summary>
        public abstract Widget RootWidget { get; set; }

        /// <summary>
        /// 在加载此场景时调用，初始化场景
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// 在卸载此场景时调用
        /// </summary>
        protected virtual void OnUnload()
        {
        }

        /// <summary>
        /// 加载目标场景
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        public static void Load<TScene>() where TScene : Scene, new()
        {
            if (loadedScene != null)
            {
                //卸载当前场景
                loadedScene.OnUnload();
                Root.Instance.RemoveWidget(loadedScene.RootWidget);
            }

            TScene scene = new TScene();
            loadedScene = scene;
            loadedScene.Initialize();
            Root.Instance.AddWidget(loadedScene.RootWidget);
        }
    }
}