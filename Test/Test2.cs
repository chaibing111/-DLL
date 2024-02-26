using PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Test2 : IPlugin
    {
        public Guid Guid => new Guid("C2A62F4E-44E8-4349-A0CE-BF562441BC04");

        public string Menu => "测试";

        public string Name => "我的插件";

        public void Execute()
        {
            MessageBox.Show("我的插件执行了", "友情提示");
        }

        public void Load()
        {

        }
        public void Dispose()
        {

        }
    }
}
