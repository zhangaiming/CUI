namespace CUIEngine.Scene
{
    /// <summary>
    /// 包含控件树等信息的场景.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// 加载此场景,在重写此方法时,必须调用基方法以初始化控件树
        /// </summary>
        public virtual void Load()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 卸载此场景,在重写此方法时,必须调用基方法以卸载控件树
        /// </summary>
        public virtual void Unload()
        {
            UnloadComponent();
        }
        
        /// <summary>
        /// 初始化控件树
        /// </summary>
        void InitializeComponent()
        {
            
        }

        /// <summary>
        /// 卸载控件树
        /// </summary>
        void UnloadComponent()
        {
            
        }
    }
}