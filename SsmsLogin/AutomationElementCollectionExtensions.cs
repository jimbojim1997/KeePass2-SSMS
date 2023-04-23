using System.Windows.Automation;

namespace SsmsLogin
{
    internal static class AutomationElementCollectionExtensions
    {
        public static AutomationElement GetControlByAutomationId(this AutomationElementCollection collection, string automationId)
        {
            foreach (AutomationElement element in collection)
            {
                if (element.Current.AutomationId == automationId) return element;
            }
            return null;
        }
    }
}
