using PluginBase;
using System.Reflection;

namespace 热拔插DLL
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 插件管理器
        /// </summary>
        private readonly PluginManager _pluginManager;
        public FrmMain()
        {
            InitializeComponent();

            //实例化插件管理器,设置插件文件夹
            _pluginManager = new PluginManager(
                Path.Combine(Application.StartupPath, "Plugins"));
            //实时更新插件
            _pluginManager.PluginsUpdated += UpdatePluginList;

            CmdClick += _pluginManager.ExecuteFunc;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _pluginManager.LoadPlugins();
            UpdatePluginList();
        }

        protected override void OnClosed(EventArgs e)
        {
            _pluginManager.UnloadPlugins();

            base.OnClosed(e);
        }

        #region 旧版本
        /// <summary>
        /// 刷新插件菜单
        /// </summary>
        //private void UpdatePluginList()
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            this.Invoke(UpdatePluginList);
        //            return;
        //        }

        //        pluginsMenu.DropDownItems.Clear();

        //        // 存储每个菜单项所对应的Guid
        //        Dictionary<string, Guid> menuGuids = new();

        //        foreach (var plugin in _pluginManager.Plugins)
        //        {
        //            if (!string.IsNullOrEmpty(plugin.Menu)
        //                && !string.IsNullOrEmpty(plugin.Name)
        //                && plugin.Guid != default(Guid))
        //            {
        //                var menuItemName = plugin.Menu;
        //                if (menuGuids.ContainsKey(menuItemName)
        //                    && menuGuids[menuItemName] == plugin.Guid)
        //                {
        //                    var existingMenuItem = pluginsMenu.DropDownItems
        //                        .OfType<ToolStripMenuItem>()
        //                        .FirstOrDefault(menuItem => menuItem.Text.Equals(menuItemName));
        //                    if (existingMenuItem != null)
        //                    {
        //                        existingMenuItem.DropDownItems.Add(new ToolStripMenuItem(
        //                            plugin.Name, null, (sender, e) => plugin.Execute()));
        //                    }
        //                }
        //                else
        //                {
        //                    // 创建一个新的菜单项并添加到Plugins菜单中
        //                    var newFirstLevelMenuItem = new ToolStripMenuItem(plugin.Menu);
        //                    var newSecondLevelMenuItem = new ToolStripMenuItem(
        //                        plugin.Name, null, (sender, e) => plugin.Execute());
        //                    newFirstLevelMenuItem.DropDownItems.Add(newSecondLevelMenuItem);
        //                    pluginsMenu.DropDownItems.Add(newFirstLevelMenuItem);

        //                    // 保存菜单项名称和ID的映射关系
        //                    menuGuids[menuItemName] = plugin.Guid;
        //                }
        //            }
        //        }
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        UpdatePluginList(); //如果期间集合变化, 重新刷新显示
        //        return;
        //    }
        //    catch
        //    {

        //    }
        //} 
        #endregion

        private void UpdatePluginList()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(UpdatePluginList);
                    return;
                }

                pluginsMenu.DropDownItems.Clear();

                // 存储每个菜单项所对应的Guid
                Dictionary<string, Guid> menuGuids = new();

                foreach (var plugin in _pluginManager.Plugins)
                {
                    if (!string.IsNullOrEmpty(plugin.Menu)
                        && !string.IsNullOrEmpty(plugin.Name)
                        && plugin.Guid != default(Guid))
                    {
                        var menuItemName = plugin.Menu;
                        if (menuGuids.ContainsKey(menuItemName)
                            && menuGuids[menuItemName] == plugin.Guid)
                        {
                            var existingMenuItem = pluginsMenu.DropDownItems
                                .OfType<ToolStripMenuItem>()
                                .FirstOrDefault(menuItem => menuItem.Text.Equals(menuItemName));
                            if (existingMenuItem != null)
                            {
                                existingMenuItem.DropDownItems.Add(CreatePluginMenuItem(plugin));
                            }
                        }
                        else
                        {
                            // 创建一个新的菜单项并添加到Plugins菜单中
                            var newFirstLevelMenuItem = new ToolStripMenuItem(plugin.Menu);
                            var newSecondLevelMenuItem = CreatePluginMenuItem(plugin);
                            newFirstLevelMenuItem.DropDownItems.Add(newSecondLevelMenuItem);
                            pluginsMenu.DropDownItems.Add(newFirstLevelMenuItem);

                            // 保存菜单项名称和ID的映射关系
                            menuGuids[menuItemName] = plugin.Guid;
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                UpdatePluginList(); //如果期间集合变化, 重新刷新显示
                return;
            }
            catch
            {
                // 异常处理逻辑
            }
        }


        event Action<(Guid,string, string)> CmdClick;
        private ToolStripMenuItem CreatePluginMenuItem(IPlugin plugin)
        {
            var menuItem = new ToolStripMenuItem(plugin.Name);
            menuItem.Click += (sender,e) => {
                CmdClick?.Invoke((plugin.Guid, plugin.Menu, plugin.Name));
            }; 
            return menuItem;
        }












        private void FrmMain_Click(object sender, EventArgs e)
        {
            label1.Visible = !label1.Visible;
        }
    }
}