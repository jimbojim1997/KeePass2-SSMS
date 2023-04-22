using KeePass.Plugins;

namespace SsmsLogin
{
    /// <summary>
    /// A KeePass2 extension for logging into Microsoft SQL Server Management Studio.
    /// <para />
    /// KeePass2 documentation: https://keepass.info/help/v2_dev/plg_index.html
    /// </summary>
    public sealed class SsmsLoginExt : Plugin
    {
        private IPluginHost _host;

        public override bool Initialize(IPluginHost host)
        {
            if (host is null) return false;
            base.Initialize(host);
            _host = host;
            return true;
        }
    }
}
