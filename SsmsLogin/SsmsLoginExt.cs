using System;
using System.Resources;
using System.Windows.Forms;

using KeePass.Plugins;

using SsmsLogin.Properties;

namespace SsmsLogin
{
    /// <summary>
    /// A KeePass2 extension for logging into Microsoft SQL Server Management Studio.
    /// <para />
    /// KeePass2 documentation: https://keepass.info/help/v2_dev/plg_index.html
    /// </summary>
    public sealed class SsmsLoginExt : Plugin
    {
        private const string ENTRY_FIELD_SSMS_SERVER = "ssms_server";
        private const string ENTRY_FIELD_USERNAME = "UserName";
        private const string ENTRY_FIELD_PASSWORD = "Password";

        private IPluginHost _host;

        public override bool Initialize(IPluginHost host)
        {
            if (host is null) return false;
            base.Initialize(host);
            _host = host;

            return true;
        }

        public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
        {
            switch (t)
            {
                case PluginMenuType.Entry:
                    {
                        var menuItem = new ToolStripMenuItem()
                        {
                            Text = Resources.UiOpenSsms
                        };
                        menuItem.Click += OpenInSsmsMenuItemClick;
                        return menuItem;
                    }
            }

            return null;
        }

        private async void OpenInSsmsMenuItemClick(object sender, EventArgs e)
        {
            var entry = _host.MainWindow.GetSelectedEntry(true);
            if (entry is null) return;

            string username = entry.Strings.ReadSafe(ENTRY_FIELD_USERNAME);
            string password = entry.Strings.ReadSafe(ENTRY_FIELD_PASSWORD);
            string servername = entry.Strings.ReadSafe(ENTRY_FIELD_SSMS_SERVER);

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(Resources.UiErrorEntryNoUsername, Resources.UiOpenSsms, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(Resources.UiErrorEntryNoUsername, Resources.UiOpenSsms, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(servername))
            {
                MessageBox.Show(Resources.UiErrorEntryNoServer, Resources.UiOpenSsms, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            await SsmsInteractions.LoginAsync(servername, username, password);
        }
    }
}
