using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace PluginBase
{
    public class PluginManager
    {
        private readonly string _pluginPath;
        private FileSystemWatcher? _watcher;

        private readonly List<IPlugin> _plugins = new();
        public List<IPlugin> Plugins { get => _plugins; }

        public event Action? PluginsUpdated;

        public PluginManager(string pluginPath)
        {
            //设置插件文件夹
            _pluginPath = pluginPath;
            //开启文件夹监控
            StartWatching();
        }

        ~PluginManager()
        {
            //停止监控文件夹
            StopWatching();
        }

        /// <summary>
        /// 加载文件夹所有插件
        /// </summary>
        public void LoadPlugins()
        {
            if (!Directory.Exists(_pluginPath))
            {
                return;
            }

            _plugins.Clear();

            Array.ForEach(Directory.GetFiles(_pluginPath, "*.dll",
                SearchOption.AllDirectories), file => LoadPlugin(file));
        }

        /// <summary>
        /// 加载单个插件
        /// </summary>
        /// <param name="pluginPath"></param>
        public void LoadPlugin(string pluginPath)
        {
            Assembly pluginAssembly;

            try
            {
                pluginAssembly = Assembly.LoadFrom(pluginPath);
            }
            catch (Exception ex)
            {
                LogExceptionDetails(ex, $"加载插件assembley失败,路径:{pluginPath}");
                return;
            }

            // 获取所有实现了IPlugin接口的类
            var pluginTypes = pluginAssembly.GetTypes()
                .Where(type => typeof(IPlugin).IsAssignableFrom(type));
            ////下面代码更宽泛,继承来的实现也算,上面的不算
            //var pluginTypes = pluginAssembly.GetTypes()
            //    .Where(type => type.GetInterfaces().Contains(typeof(IPlugin)));

            if (pluginTypes is null) return;

            foreach (var pluginType in pluginTypes)
            {
                try
                {
                    // 创建插件实例
                    var plugin = Activator.CreateInstance(pluginType) as IPlugin;

                    // 执行插件特定初始化操作
                    plugin?.Load();

                    // 添加到插件列表
                    if (plugin != null)
                        _plugins.Add(plugin);
                }
                catch (Exception ex)
                {
                    LogExceptionDetails(ex, $"插件创建失败!");
                }
            }
        }

        public void UnloadPlugins()
        {
            StopWatching();

            _plugins.ForEach(plugin => plugin.Dispose());

            _plugins.Clear();

            PluginsUpdated?.Invoke();
        }

        public void UnloadPlugin(string pluginPath)
        {
            Assembly? pluginAssembly;
            try
            {
                //使用Location获取dll地址,与参数比较得到内存中对应程序集
                pluginAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(asm => asm.Location == pluginPath);
            }
            catch (Exception ex)
            {
                LogExceptionDetails(ex, $"卸载插件assembley失败,路径:{pluginPath}");
                return;
            }

            // 获取所有实现了IPlugin接口的类
            var pluginTypes = pluginAssembly?.GetTypes()
                .Where(type => typeof(IPlugin).IsAssignableFrom(type));

            if (pluginTypes is null) return;

            foreach (Type pluginType in pluginTypes)
            {
                foreach (var plugin in _plugins)
                {
                    if (plugin.GetType() == pluginType)
                        plugin.Dispose();
                }
                _plugins.RemoveAll(r => r.GetType() == pluginType);
            }
        }

        /// <summary>
        /// 监听目录变化
        /// </summary>
        private void StartWatching()
        {
            if (!Directory.Exists(_pluginPath))
            {
                return;
            }
            _watcher = new FileSystemWatcher(_pluginPath, "*.dll");
            _watcher.IncludeSubdirectories = true;
            _watcher.Created += Watcher_Created;
            _watcher.Deleted += Watcher_Deleted;
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 停止目录监控
        /// </summary>
        private void StopWatching()
        {
            if (_watcher != null)
            {
                _watcher.Created -= Watcher_Created;
                _watcher.Deleted -= Watcher_Deleted;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            LoadPlugin(e.FullPath);
            PluginsUpdated?.Invoke();
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            UnloadPlugin(e.FullPath);
            PluginsUpdated?.Invoke();
        }

        private void LogExceptionDetails(Exception ex, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"记录时间:{DateTime.Now:g}");
            sb.AppendLine($"错误信息:{message}");
            sb.AppendLine(ex.ToString());
            sb.AppendLine(ex.StackTrace);
            File.AppendAllText("ExceptionDetails.log", sb.ToString());
        }

        public void ExecuteFunc((Guid,string,string) cmd)
        {
            var Item = Plugins.Where(r => 
                r.Guid == cmd.Item1 &&
                r.Menu == cmd.Item2 &&
                r.Name == cmd.Item3
            ).FirstOrDefault();
            Item?.Execute();
        }
    }
}
