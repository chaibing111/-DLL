using PluginBase;

namespace Test
{
    public class Test : IPlugin
    {
        public Guid Guid => new Guid("C2A62F4E-44E8-4349-A0CE-BF562441BC04");
        public string Menu => "测试";

        public string Name => "Test";

        public void Execute()
        {
            MessageBox.Show("插件方法执行了","友情提示");
        }

        public void Load()
        {
            //为每个插件提供自定义初始化方法。
            //有些插件需要创建实例后初始化方法才能正常工作
        }
        public void Dispose()
        {
            // 空方法,因为没有非托管资源需要释放
            // 如果基类有使用非托管资源,也需处理
        }
    }

}
