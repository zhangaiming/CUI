using System;
using System.Collections.Generic;
using System.Linq;
using CUIEngine.Mathf;

namespace CUIEngine
{
    public class RenderClip
    {
        RenderUnit[,] units;
        Vector2Int coord;

        /// <summary>
        /// 渲染片段的大小
        /// </summary>
        public Vector2Int Size => new Vector2Int(units.GetLength(0), units.GetLength(1));
        /// <summary>
        /// 渲染片段的坐标
        /// </summary>
        public Vector2Int Coord
        {
            get => coord;
            set => coord = value;
        }

        /// <summary>
        /// 进行渲染片段的初始化, 默认全为空单元
        /// </summary>
        /// <param name="x">片段宽度</param>
        /// <param name="y">片段高度</param>
        public RenderClip(int x, int y, Vector2Int coord)
        {
            this.coord = coord;
            units = new RenderUnit[Math.Max(0, x), Math.Max(0, y)];
        }
        
        /// <summary>
        /// 进行渲染片段的初始化, 默认全为空单元
        /// </summary>
        /// <param name="Size">片段的大小</param>
        public RenderClip(Vector2Int size, Vector2Int coord) : this(size.X, size.Y, coord){}

        /// <summary>
        /// 对源片段进行复制
        /// </summary>
        /// <param name="src"></param>
        public RenderClip(RenderClip src)
        {
            coord = src.coord;
            int x = src.Size.X, y = src.Size.Y;
            units = new RenderUnit[x, y];
            for (x--; x >= 0; x--)
            {
                for (y--; y >= 0; y--)
                {
                    units[x, y] = src.units[x, y];
                }
            }
        }

        /// <summary>
        /// 调整片段大小, 片段内容及内容坐标保持不变
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="offset">片段坐标偏移, 以左上角为原点, x向下为右, y向下为正</param>
        public void Resize(Vector2Int newSize, Vector2Int offset)
        {
            int x = newSize.X, y = newSize.Y;
            if (x >= 0 && y >= 0)
            {
                RenderClip nClip = new RenderClip(newSize, coord + offset);
                x = Math.Min(x, Math.Max(0, Size.X + offset.X));
                y = Math.Max(y, Math.Max(0, Size.Y + offset.Y));
                int nx, ny;

                List<int> a = new List<int>();
                
            }
        }
        
        /// <summary>
        /// 直接设置对应位置的单元, 只要在范围内就一定会对原单元进行覆盖
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        public void SetUnit(Vector2Int coord, RenderUnit unit)
        {
            if (coord.X < units.GetLength(0) && coord.Y < units.GetLength(1))
            {
                units[coord.X, coord.Y] = unit;
            }
        }

        /// <summary>
        /// 根据单元的权重判断是否对原单元进行覆盖
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="unit"></param>
        public void PutUnit(Vector2Int coord, RenderUnit unit)
        {
            int x = coord.X, y = coord.Y;
            if (x < units.GetLength(0) && y < units.GetLength(1))
            {
                if(units[x,y].Weight <= unit.Weight)
                    units[x, y] = unit;
            }
        }

        /// <summary>
        /// 获得一个单元
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public RenderUnit GetUnit(Vector2Int coord)
        {
            return GetUnit(coord.X, coord.Y);
        }

        /// <summary>
        /// 获得一个单元
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public RenderUnit GetUnit(int x, int y)
        {
            if (x >= 0 && x < Size.X && y >= 0 && y < Size.Y)
            {
                return units[x, y];
            }

            return null;
        }

        /// <summary>
        /// 根据所给参数合并两个渲染片段
        /// </summary>
        /// <param name="a">主片段</param>
        /// <param name="b">副片段</param>
        /// <param name="offset">副片段相对于主片段的位移</param>
        /// <param name="shouldClip">是否对副片段进行裁剪, 裁剪后合并所得片段大小与主片段一致</param>
        /// <returns>合并所得片段</returns>
        public static RenderClip Merge(RenderClip a, RenderClip b, Vector2Int offset, bool shouldClip = true)
        {
            RenderClip res = new RenderClip(a);
            int x, y;
            if (shouldClip)
            {
                x = a.Size.X;
                y = a.Size.Y;
            }
            else
            {
                x = GetMergeResultEdgeLength(a.Size.X, b.Size.X, offset.X);
                y = GetMergeResultEdgeLength(a.Size.Y, b.Size.Y, offset.Y);
                res.Resize(new Vector2Int(x,y), -offset);
            }

            int ox = Math.Max(offset.X, 0);
            int oy = Math.Max(offset.Y, 0);
            int bx = b.Size.X, by = b.Size.Y;
            for (int i = 0; i < bx; i++)
            {
                for (int j = 0; j < by; j++)
                {
                    //res.units[i + ox, j + oy] = b.units[i, j];
                    res.PutUnit(new Vector2Int(i + ox, j + oy), b.units[i, j]);
                }
            }

            return res;
        }
        
        /// <summary>
        /// 根据所给参数合并两个渲染片段
        /// </summary>
        /// <param name="a">主片段</param>
        /// <param name="b">副片段</param>
        /// <param name="shouldClip">是否对副片段进行裁剪, 裁剪后合并所得片段大小与主片段一致</param>
        /// <returns>合并所得片段</returns>
        public static RenderClip Merge(RenderClip a, RenderClip b, bool shouldClip = true)
        {
            return Merge(a, b, Vector2Int.Zero, shouldClip);
        }

        static int GetMergeResultEdgeLength(int a, int b, int offset)
        {
            if (offset > 0)
                return Math.Max(a, b + offset);
            else
                return Math.Max(a - offset, b);
        }
    }
}