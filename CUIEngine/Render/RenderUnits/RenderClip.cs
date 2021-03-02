//todo: 权重或许可以去掉, 但是为了防止可能有考虑不周全的地方, 以后再确定要不要删掉

using System;
using System.Threading.Tasks;
using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    public class RenderClip
    {
        RenderUnit[] units;
        Vector2Int size;
        Vector2Int coord;

        /// <summary>
        /// 渲染片段的大小
        /// </summary>
        public Vector2Int Size
        {
            get => size;
            set => size = value;
        }
        /// <summary>
        /// 渲染片段的坐标
        /// </summary>
        public Vector2Int Coord
        {
            get => coord;
            set => coord = value;
        }

        /// <summary>
        /// 进行渲染片段的初始化,默认为空片段,即一个单元都没有
        /// </summary>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="coordX"></param>
        /// <param name="coordY"></param>
        public RenderClip(int sizeX = 0, int sizeY = 0, int coordX = 0, int coordY = 0)
        {
            sizeX = Math.Max(sizeX, 0);
            sizeY = Math.Max(sizeY, 0);
            size.X = sizeX;
            size.Y = sizeY;
            coord.X = coordX;
            coord.Y = coordY;
            units = new RenderUnit[sizeX * sizeY];
            Clear();
        }
        
        /// <summary>
        /// 进行渲染片段的初始化, 默认全为空单元
        /// </summary>
        /// <param name="x">片段宽度</param>
        /// <param name="y">片段高度</param>
        /// <param name="coord"></param>
        public RenderClip(int x, int y, Vector2Int coord) : this(x, y, coord.X, coord.Y) { }

        /// <summary>
        /// 进行渲染片段的初始化, 默认全为空单元
        /// </summary>
        /// <param name="size"></param>
        /// <param name="coord"></param>
        public RenderClip(Vector2Int size, Vector2Int coord) : this(size.X, size.Y, coord){}

        /// <summary>
        /// 对源片段进行复制
        /// </summary>
        /// <param name="src"></param>
        public RenderClip(RenderClip src)
        {
            size = src.size;
            coord = src.coord;

            int x = size.X, y = size.Y;

            units = new RenderUnit[x * y];
            for (int k = 0; k < x * y; k++)
            {
                int i = k % x;
                int j = k / x;
                SetUnit(i, j, src.GetUnit(i, j));
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
                RenderUnit[] newUnits = new RenderUnit[newSize.X * newSize.Y];
                RenderUnit emptyUnit = new RenderUnit(true);
                
                //根据片段大小选择处理流程
                if(x * y > 100)
                //并行处理
                {
                    Parallel.For(0, x * y, k =>
                    {
                        int i = k % x;
                        int j = k / x;
                        int nx = i + offset.X;
                        int ny = j + offset.Y;
                        if (nx >= 0 && nx < size.X && ny >= 0 && ny < size.Y)
                        {
                            RenderUnit unit = GetUnit(nx, ny);
                            newUnits[j * newSize.X + i] = unit;
                        }
                        else
                        {
                            newUnits[j * newSize.X + i] = emptyUnit;
                        }
                    });
                }
                else
                //串行处理
                {
                    for (int k = 0; k < x * y; k++)
                    {
                        int i = k % x;
                        int j = k / x;
                        int nx = i + offset.X;
                        int ny = j + offset.Y;
                        if (nx >= 0 && nx < size.X && ny >= 0 && ny < size.Y)
                        {
                            RenderUnit unit = GetUnit(nx, ny);
                            newUnits[j * newSize.X + i] = unit;
                        }
                        else
                        {
                            newUnits[j * newSize.X + i] = emptyUnit;
                        }
                    }
                }
                size = newSize;
                coord += offset;
                units = newUnits;
            }
        }

        /// <summary>
        /// 设置目标单元是否为空
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="isEmpty"></param>
        public void SetEmpty(Vector2Int coord, bool isEmpty)
        {
            SetEmpty(coord.X, coord.Y, isEmpty);
        }
        /// <summary>
        /// 设置目标单元是否为空
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isEmpty"></param>
        public void SetEmpty(int x, int y, bool isEmpty)
        {
            RenderUnit unit = GetUnit(x, y);
            unit.IsEmpty = true;
            SetUnit(x, y, unit);
        }
        /// <summary>
        /// 直接设置对应位置的单元, 只要在范围内就一定会对原单元进行覆盖
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="unit"></param>
        public void SetUnit(Vector2Int coord, RenderUnit unit)
        {
            SetUnit(coord.X, coord.Y, unit);
        }
        /// <summary>
        /// 直接设置对应位置的单元, 只要在范围内就一定会对原单元进行覆盖
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        public void SetUnit(int x, int y, RenderUnit unit)
        {
            if (x >= 0 && x < size.X && y >= 0 && y < size.Y)
            {
                units[y * size.X + x] = unit;
            }
        }
        /// <summary>
        /// 尝试设置对应位置的单元,若要设置的单元为空单元,则设置失败
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool PutUnit(int x, int y, RenderUnit unit)
        {
            if (!unit.IsEmpty)
            {
                RenderUnit oldUnit = units[y * Size.X + x];

                Color color = unit.Color;
                Color oldUnitColor = oldUnit.Color;
                if (color.IsForegroundTransparent)
                {
                    color.ForegroundColor = oldUnitColor.ForegroundColor;
                    color.IsForegroundTransparent = oldUnitColor.IsForegroundTransparent;
                }

                if (color.IsBackgroundTransparent)
                {
                    color.BackgroundColor = oldUnitColor.BackgroundColor;
                    color.IsBackgroundTransparent = oldUnitColor.IsBackgroundTransparent;
                }
                unit.Color = color;
                
                
                SetUnit(x, y, unit);
                return true;
            }

            return false;
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
                return units[y * size.X + x];
            }

            return new RenderUnit(true);
        }
        /// <summary>
        /// 根据所给参数合并两个渲染片段
        /// </summary>
        /// <param name="a">主片段</param>
        /// <param name="b">副片段</param>
        /// <param name="unitCoveredHandler">用于处理单元覆盖事件的回调函数</param>
        /// <param name="shouldClip">是否对副片段进行裁剪, 裁剪后合并所得片段大小与主片段一致</param>
        /// <returns>合并所得片段</returns>
        public static RenderClip Merge(RenderClip a, RenderClip b, 
            Action<int, int, RenderUnit> unitCoveredHandler, bool shouldClip = true)
        {
            RenderClip res = new RenderClip(a);
            res.MergeWith(b, unitCoveredHandler, shouldClip);
            return res;
        }

        /// <summary>
        /// 将片段与另一片段进行合并, 会改变原片段
        /// </summary>
        /// <param name="other"></param>
        /// <param name="unitCoveredHandler"></param>
        /// <param name="shouldClip"></param>
        /// <param name="forceCover"></param>
        public void MergeWith(RenderClip other, Action<int, int, RenderUnit>? unitCoveredHandler, bool shouldClip, bool forceCover = false)
        {
            Vector2Int offset = other.coord - this.coord;
            int x, y;
            if(!shouldClip)
            {
                x = GetMergeResultEdgeLength(this.Size.X, other.Size.X, offset.X);
                y = GetMergeResultEdgeLength(this.Size.Y, other.Size.Y, offset.Y);
                int i = Math.Min(offset.X, 0);
                int j = Math.Min(offset.Y, 0);
                this.Resize(new Vector2Int(x,y), new Vector2Int(i, j));
            }

            int ox = Math.Max(offset.X, 0);
            int oy = Math.Max(offset.Y, 0);
            int bx = other.Size.X, by = other.Size.Y;
            {
                for (int i = 0; i < bx; i++)
                {
                    for (int j = 0; j < by; j++)
                    {
                        RenderUnit unit = other.units[j * other.size.X + i];
                        if (forceCover)
                        {
                            this.SetUnit(i + ox, j + oy, unit);
                            unitCoveredHandler?.Invoke(i, j, unit);
                        }
                        else
                        {
                            bool covered = this.PutUnit(i + ox, j + oy, unit);
                            if (covered)
                            {
                                unitCoveredHandler?.Invoke(i, j, unit);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清空渲染片段中的所有单元
        /// </summary>
        public void Clear()
        {
            int x = size.X, y = size.Y;
            RenderUnit emptyUnit = new RenderUnit(true);
            for (int k = 0; k < x * y; k++)
            {
                int i = k % x;
                int j = k / x;
                SetUnit(i, j, emptyUnit);
            }
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