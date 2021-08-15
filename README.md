# CUI
## 介绍
一个Windows控制台用户交互界面引擎，灵感来源于DOSBOX中TASM的调试界面，程序的结构参考了unity的组件式开发，目前也还没有开发到最终满意的样子。
希望最终能够通过CUI引擎，可以轻松地在控制台中设计字符UI界面，作为应用或者游戏的一部分。

## 功能简述
+ 轻松地使用控件绘制出场景
+ 使用控件树管理UI控件
+ 像在unity里一样，通过脚本系统赋予控件功能
+ 自定义预设场景，在运行过程中快速切换场景（还未实现）

## 基本使用教程
### 创建项目
首先创建一个Windows Console程序并引用引擎项目或链接相关库，然后进行引擎初始化即可使用引擎功能。

```C#
//在使用所有引擎中的功能时，必须先初始化引擎
CUIEngine.Initialize();

//...

//想要关闭引擎时，通过Shutdown()卸载引擎
CUIEngine.Shutdown();
```

### 实例化控件
使用CUI开发程序时，你需要使用控件来构建场景。
在我提供的Solution中，有一个基本的控件库项目，包含有TextBox（文本块）和Panel（面板）两个控件。下面使用其中的Panel控件进行创建控件的示例：

```C#
//在创建控件前先初始化引擎
CUIEngine.Initialize();

//在坐标原点处创建一个宽度3格，高度2格的Panel控件实例
Panel panel = Widget.Create<Panel>(new Vector2Int(3, 2), Vector2Int.Zero, "panel");
//在创建控件之后，就可以对它进行设置等操作
```

### 自定义控件
使用CUI开发程序时，你也可以通过继承Widget或WidgetContainer来实现自定义控件，下面这段代码是创建一个自定义控件所必需的代码：

```C#
public class CustomWidget : Widget
{
  public void MakeRenderClip()
  {
    //在这进行控件的绘制...
  }
}
```

### 自定义脚本
在使用CUI过程中，你很可能需要为控件创建自定义脚本以实现更多功能。这在使用过程中和unity创建脚本的体验相似，使用过unity的人可能对此感到熟悉：

```C#
public class CustomScript : Script
{
  //在自定义脚本类中的Awake()函数会在初始化完毕后被调用
  void Awake(){ //... }

  //在自定义脚本类中的Update()函数会在每一帧画面渲染完成之后被调用
  void Update(){ //... }
  
  //在自定义脚本类中的OnEnabled()函数会在每次脚本被启用时被调用
  void OnEnabled(){ //... }
  
  //这里可以随意添加其他代码...
}
```

### 输入处理
要设计一个应用或者游戏，用户的输入自然必不可少。Input类是CUI中对输入处理进行封装的一个类。
通过Input类，可以在任何地方创建对某一按键或所有按键的绑定，Input类会在接收到用户输入之后调用每个绑定的处理函数。

```C#
public class CustomScript : Script
{
  void Awake()
  {
    //绑定对任意按键输入响应的处理函数
    Input.AttachHandler(AnyKeyInputHandler);
    
    //绑定只对Ctrl+A组合键进行响应的处理函数
    Input.AttachHandler(KeyInputHandler, ConsoleKey.A, true);
  }
  
  void AnyKeyInputHandler(ConsoleKeyInfo keyInfo)
  {
    //对任意按键输入都进行处理...
  }
  
  void KeyInputHandler(ConsoleKeyInfo keyInfo)
  {
    //对某一按键输入组合进行处理...
  }
}
```

## 灵感来源
下图是DOSBOX中TASM界面，通过目前的CUI引擎已经基本可以实现图中的效果，只是还有些别扭……
![](https://s3.bmp.ovh/imgs/2021/08/b553555041af723c.png)

## 一些小背景
绝大部分接触编程的人，第一次编写出的程序就是运行在那个黑色窗口里的，我也不例外。当时我是为了学游戏制作而开始学习编程，于是在掌握了一点点编程技能后，就迫不及待地想在这个黑框框里试着做小游戏。当时就在想为什么在这框框里整个游戏界面这么麻烦呢淦（。
在后来接触到unity、unreal等各种游戏引擎之后，又还是放不下这个小黑框，于是就做了这么一个引擎。
