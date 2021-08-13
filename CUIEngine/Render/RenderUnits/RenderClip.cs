using System;
using System.Drawing;
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
        /// <param name="coord">原点</param>
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
            Size = src.Size;
            Coord = src.Coord;

            int x = Size.X, y = Size.Y;

            units = new RenderUnit[x * y];
            for (int k = 0; k < x * y; k++)
            {
                int i = k % x;
                int j = k / x;
                SetUnit(i, j, src.GetUnit(i, j));
            }
        }
        /// <summary>
        /// 调整片段大小,并进行偏移,片段内容及内容的绝对坐标保持不变
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="offset">片段坐标偏移量,以左上角为原点,x向下为右,y向下为正</param>
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
                        if (nx >= 0 && nx < Size.X && ny >= 0 && ny < Size.Y)
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
                        if (nx >= 0 && nx < Size.X && ny >= 0 && ny < Size.Y)
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
                Size = newSize;
                Coord += offset;
                units = newUnits;
            }
        }

        /// <summary>
        /// 改变渲染片段的大小以及坐标,其内容在屏幕上的绝对坐标不变
        /// </summary>
        /// <param name="newSize">新的渲染片段大小</param>
        /// <param name="newCoord">新的渲染片段坐标</param>
        public void Remap(Vector2Int newSize, Vector2Int newCoord)
        {
            int x = newSize.X, y = newSize.Y;
            Vector2Int offset = newCoord - Coord;
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
                        if (nx >= 0 && nx < Size.X && ny >= 0 && ny < Size.Y)
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
                        if (nx >= 0 && nx < Size.X && ny >= 0 && ny < Size.Y)
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
                Size = newSize;
                Coord += offset;
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
            if (x >= 0 && x < Size.X && y >= 0 && y < Size.Y)
            {
                units[y * Size.X + x] = unit;
            }
        }
        
        /// <summary>
        /// 设置对应位置的单元并将单元颜色进行相应的设置,若要设置的单元为空单元,则设置失败
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool PutUnit(int x, int y, RenderUnit unit)
        {
            if (!unit.IsEmpty)
            {
                if(x >= 0 && x < Size.X && y >= 0 && y < Size.Y)
                {
                    RenderUnit oldUnit = units[y * Size.X + x];

                    ColorPair colorPair = unit.ColorPair;
                    ColorPair oldUnitColorPair = oldUnit.ColorPair;
                    colorPair.ForegroundColor = ParseTransparentColor(colorPair.ForegroundColor, oldUnitColorPair);
                    colorPair.BackgroundColor = ParseTransparentColor(colorPair.BackgroundColor, oldUnitColorPair);

                    unit.ColorPair = colorPair;

                    SetUnit(x, y, unit);
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 将透明颜色转变成对应的颜色
        /// </summary>
        /// <param name="rawColor"></param>
        /// <param name="oldUnitColorPair"></param>
        /// <returns></returns>
        static CUIColor ParseTransparentColor(CUIColor rawColor, ColorPair oldUnitColorPair)
        {
            if (rawColor == CUIColor.NextForegroundColor)
            {
                return oldUnitColorPair.ForegroundColor;
            }
            else if (rawColor == CUIColor.NextBackgroundColor)
            {
                return oldUnitColorPair.BackgroundColor;
            }

            return rawColor;
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
                return units[y * Size.X + x];
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
            Vector2Int offset = other.Coord - this.Coord;
            int x, y;
            if(!shouldClip)
            {
                x = GetMergeResultEdgeLength(this.Size.X, other.Size.X, offset.X);
                y = GetMergeResultEdgeLength(this.Size.Y, other.Size.Y, offset.Y);
                int i = Math.Min(offset.X, 0);
                int j = Math.Min(offset.Y, 0);
                this.Resize(new Vector2Int(x,y), new Vector2Int(i, j));
            }
            
            int ox = offset.X;
            int oy = offset.Y;
            int bx = other.Size.X, by = other.Size.Y;
            {
                for (int i = 0; i < bx; i++)
                {
                    for (int j = 0; j < by; j++)
                    {
                        RenderUnit unit = other.units[j * other.Size.X + i];
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
            int x = Size.X, y = Size.Y;
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

        /// <summary>
        /// 在渲染片段中画一个矩形框
        /// </summary>
        /// <param name="origin">矩形左上方的顶点</param>
        /// <param name="height">高</param>
        /// <param name="width">宽</param>
        /// <param name="style">矩形的样式</param>
        public void DrawBorder(Vector2Int origin, int height, int width, RenderUnit style)
        {
            for (int x = 0; x <= width; x++)
            {
                PutUnit(origin.X + x, origin.Y, style);
                PutUnit(origin.X + x, origin.Y + height - 1, style);
            }

            for (int y = 1; y <= height - 1; y++)
            {
                PutUnit(origin.X, origin.Y + y, style);
                PutUnit(origin.X + width - 1, origin.Y + y, style);
            }
        }

        /// <summary>
        /// 在渲染片段中画一个矩形
        /// </summary>
        /// <param name="origin">矩形左上方的顶点</param>
        /// <param name="height">高</param>
        /// <param name="width">宽</param>
        /// <param name="style">矩形的样式</param>
        public void DrawRectangle(Vector2Int origin, int height, int width, RenderUnit style)
        {
            for (int i = 0; i <= width; i++)
            {
                for (int j = 0; j <= height; j++)
                {
                    PutUnit(origin.X + i, origin.Y + j, style);
                }
            }
        }
    }

    public enum LineStyle
    {
        Horizontal,
        Vertical
    }
}