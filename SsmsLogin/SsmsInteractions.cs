using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

using SsmsLogin.Properties;

namespace SsmsLogin
{
    internal static class SsmsInteractions
    {
        public static async Task LoginAsync(string server, string username, string password)
        {
            Process ssmsProcess;
            IntPtr loginWindow;
            if (false && TryGetExistingSsmsInstance(out ssmsProcess))
            {
                loginWindow = await OpenLoginWindow(ssmsProcess);
            }
            else
            {
                ssmsProcess = StartSsmsProcess();
                loginWindow = await GetProcessWindowWithTitle(ssmsProcess, Resources.ProcessSsmsLoginWindowTitle);
            }

            if (loginWindow == IntPtr.Zero) return;

            PopulateLoginForm(loginWindow, server, username, password);
            LoginFormLogin(loginWindow);
        }

        private static bool TryGetExistingSsmsInstance(out Process process)
        {
            var ssmsProcess = Process.GetProcessesByName(Resources.ProcessSsmsExeName).FirstOrDefault();
            if (ssmsProcess is null)
            {
                process = null;
                return false;
            }
            else
            {
                process = ssmsProcess;
                return true;
            }
        }

        private static Process StartSsmsProcess()
        {
            return Process.Start(Resources.ProcessSsmsExePath);
        }

        private static Task<IntPtr> OpenLoginWindow(Process ssms)
        {
            throw new NotImplementedException();
        }

        private static async Task<IntPtr> GetProcessWindowWithTitle(Process ssmsProcess, string windowTitle, CancellationToken cancellationToken = default)
        {
            IntPtr loginWindow = IntPtr.Zero;
            while (!cancellationToken.IsCancellationRequested)
            {
                loginWindow = User32.EnumWindows().Where(window =>
                {
                    IntPtr thread = User32.GetWindowThreadProcessId(window, out IntPtr proccessId);
                    string title = User32.GetWindowTextString(window);
                    return ssmsProcess.Id == (int)proccessId && windowTitle.Equals(title, StringComparison.InvariantCultureIgnoreCase);
                }).FirstOrDefault();
                if (loginWindow != IntPtr.Zero) break;
                await Task.Delay(100, cancellationToken);
            }

            return loginWindow;
        }

        private static void PopulateLoginForm(IntPtr loginWindow, string server, string username, string password)
        {
            AutomationElement window = AutomationElement.FromHandle(loginWindow);
            AutomationElementCollection controls = window.FindAll(TreeScope.Descendants, Condition.TrueCondition);
            
            var comboBoxServerTypeElement = controls.GetControlByAutomationId("comboBoxServerType");
            var serverInstanceElement = controls.GetControlByAutomationId("serverInstance");
            var comboBoxAuthenticationElement = controls.GetControlByAutomationId("comboBoxAuthentication");
            var userNameElement = controls.GetControlByAutomationId("userName");
            var passwordElement = controls.GetControlByAutomationId("password");
            var savePasswordElement = controls.GetControlByAutomationId("savePassword");
            var connectElement = controls.GetControlByAutomationId("connect");

            comboBoxServerTypeElement.SetValue("Database Engine");
            serverInstanceElement.SetValue(server);
            comboBoxAuthenticationElement.SetValue("SQL Server Authentication");
            userNameElement.SetValue(username);
            passwordElement.SetValue(password);
            savePasswordElement.SetValue("false");
            connectElement.Invoke();
        }

        private static void LoginFormLogin(IntPtr loginWindow)
        {

        }
    }
}
