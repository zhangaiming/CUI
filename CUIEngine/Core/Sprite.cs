using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CUIEngine.Mathf;

namespace CUIEngine
{
    public abstract class Sprite
    {
        static Dictionary<string, List<Sprite>> spritesPool = new Dictionary<string, List<Sprite>>();   //精灵池
        string tag = "";
        string name = "";
        bool isDestroyed = false;
        
        /// <summary>
        /// 精灵的坐标
        /// </summary>
        public virtual Vector2Int Coord { get; protected set; } = Vector2Int.Zero;
        
        /// <summary>
        /// 精灵的大小
        /// </summary>
        public virtual Vector2Int Size { get; protected set; } = Vector2Int.Zero;

        /// <summary>
        /// 精灵的标签,用于做寻找精灵的标识
        /// </summary>
        public string Tag
        {
            get => tag;
            protected set => tag = value;
        }

        /// <summary>
        /// 精灵的名称,用于做寻找精灵的标识
        /// </summary>
        public string Name
        {
            get => name;
            protected set => name = value;
        }

        /// <summary>
        /// 精灵是否已被销毁,若已被销毁,则不应该再对相关调用进行回应
        /// </summary>
        public bool IsDestroyed => isDestroyed;

        public Sprite(string name, string tag = "")
        {
            Name = name;
            Tag = tag;
            
            AddToPool(this);
        }

        /// <summary>
        /// 销毁精灵,从精灵池中移除
        /// </summary>
        public static void DestroySprite(Sprite sprite)
        {
            RemoveFromPool(sprite);
            sprite.isDestroyed = true;
        }
        
        /// <summary>
        /// 向精灵池中添加指定精灵
        /// </summary>
        /// <param name="sprite"></param>
        static void AddToPool(Sprite sprite)
        {
            string name = sprite.name;
            if (spritesPool.ContainsKey(name))
            {
                spritesPool[name].Add(sprite);
            }
            else
            {
                spritesPool.Add(name, new List<Sprite>() {sprite});
            }
        }
        
        /// <summary>
        /// 从精灵池中移除指定精灵
        /// </summary>
        /// <param name="sprite"></param>
        static void RemoveFromPool(Sprite sprite)
        {
            string name = sprite.name;
            if (spritesPool.ContainsKey(name))
            {
                List<Sprite> temp = spritesPool[name];
                temp.Remove(sprite);
                if (temp.Count == 0)
                {
                    spritesPool.Remove(name);
                }
            }
        }
        
        /// <summary>
        /// 寻找并返回第一个指定类型的并且拥有指定名字的精灵,如果不能找到匹配的精灵,则返回null
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TSprite"></typeparam>
        /// <returns></returns>
        [return: MaybeNull]
        public static TSprite Find<TSprite>(string name) where TSprite : Sprite
        {
            if (spritesPool.ContainsKey(name))
            {
                List<Sprite> widgets = spritesPool[name];
                if (widgets.Count >= 1)
                {
                    return (TSprite)widgets[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 寻找并返回第一个指定类型的并且拥有指定标签的精灵,如果不能找到,则返回null
        /// </summary>
        /// <param name="tag"></param>
        /// <typeparam name="TSprite"></typeparam>
        /// <returns></returns>
        [return: MaybeNull]
        public static TSprite FindWithTag<TSprite>(string tag) where TSprite : Sprite
        {
            foreach (var keyValuePair in spritesPool)
            {
                List<Sprite> sprites = keyValuePair.Value;
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.Tag == tag && sprite is TSprite)
                    {
                        return (TSprite)sprite;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 寻找并返回所有指定名称的精灵,如果不能找到,则返回一个空表
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TSprite"></typeparam>
        /// <returns></returns>
        [return: MaybeNull]
        public static List<TSprite> FindAll<TSprite>(string name) where TSprite : Sprite
        {
            List<TSprite> res = new List<TSprite>();
            if (spritesPool.ContainsKey(name))
            {
                List<Sprite> sprites = spritesPool[name];
                foreach (Sprite sprite in sprites)
                {
                    if(sprite is TSprite)
                        res.Add((TSprite)sprite);
                }
            }

            return res;
        }
        
        /// <summary>
        /// 寻找并返回所有指定标签的精灵,如果不能找到,则返回一个空表
        /// </summary>
        /// <param name="tag"></param>
        /// <typeparam name="TSprite"></typeparam>
        /// <returns></returns>
        [return: MaybeNull]
        public static List<TSprite> FindAllWithTag<TSprite>(string tag) where TSprite : Sprite
        {
            List<TSprite> res = new List<TSprite>();
            foreach (var keyValuePair in spritesPool)
            {
                List<Sprite> sprites = keyValuePair.Value;
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.Tag == tag && sprite is TSprite)
                    {
                        res.Add((TSprite)sprite);
                    }
                }
            }

            return res;
        }
    }
}