
namespace PluginBase
{
    public interface IPlugin:IDisposable
    {
        Guid Guid { get; }
        string Menu { get; }
        string Name { get; }
        void Execute();
        void Load();
    }
}

#region 为什么要有Load方法
//将一部分初始化逻辑放到 Load 方法中，
//可以帮助保持构造方法的简洁性、
//提供更灵活的初始化方式(可选择是否Load)，
//并允许处理异步操作或按需加载的场景。 
#endregion

#region 为什么要有Dispose方法
//非托管资源（如文件句柄、数据库连接、网络连接等）需要手动释放
//关闭文件、取消订阅事件、释放定时器等。Dispose 方法提供了统一的位置
//实现 IDisposable 接口, 可利用 using 语句或手动调用 Dispose 方法
#endregion

