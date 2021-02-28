using System;
using CUIEngine.Mathf;
using CUIEngine.Render;
using NUnit.Framework;

namespace TestProject1
{
    public class TestRenderClip
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_CreateEmpty()
        {
            RenderClip clip = new RenderClip();
            Assert.AreEqual(new Vector2Int(0, 0), clip.Size, "空渲染片段创建失败");
        }

        [TestCase(-1, 2, 0, 0)]
        [TestCase(2, -1, -2, -1)]
        [TestCase(3, 3, 0, -2)]
        [TestCase(0, 0, 2, 2)]
        public void Test_Create(int sizeX, int sizeY, int coordX, int coordY)
        {
            RenderClip clip = new RenderClip(sizeX, sizeY, coordX, coordY);
            //渲染片段大小必须为非负数
            Assert.AreEqual(new Vector2Int(Math.Max(0, sizeX), Math.Max(0, sizeY)), clip.Size, "渲染片段大小错误");
            //渲染片段位置应与参数相同
            Assert.AreEqual(new Vector2Int(coordX, coordY), clip.Coord, "渲染片段位置错误");
        }

        [TestCase(0, 0)]
        [TestCase(5, 5)]
        [TestCase(-2, 5)]
        [TestCase(0, -1)]
        [TestCase(-5, -5)]
        [TestCase(10, -5)]
        [TestCase(10, 10)]
        public void Test_SetAndGetUnit(int coordX, int coordY)
        {
            //创建目标单元
            RenderUnit unit = new RenderUnit(Color.DefaultColor);

            //空渲染片段测试
            RenderClip clip = new RenderClip();
            clip.SetUnit(coordX, coordY, unit);
            JudgeUnit(unit, clip, coordX, coordY);
            
            //非空渲染片段测试
            clip = new RenderClip(6, 6);
            clip.SetUnit(coordX, coordY, unit);
            JudgeUnit(unit, clip, coordX, coordY);
        }

        void JudgeUnit(RenderUnit unit, RenderClip clip, int coordX, int coordY)
        {
            if(coordX >= 0 && coordY >= 0 && coordX < clip.Size.X && coordY < clip.Size.Y)
            {
                Assert.AreEqual(clip.GetUnit(coordX, coordY), unit, "在应该成功设置单元时,取出的单元与放入的单元不相同");
            }
            else
            {
                Assert.AreNotEqual(clip.GetUnit(coordX, coordY), unit, "在不应该成功设置单元时成功地设置了单元, 并且获取出的单元与原单元相同");
            }
        }
    }
}