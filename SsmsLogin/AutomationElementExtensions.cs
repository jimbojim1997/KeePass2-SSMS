using System.Windows.Automation;

namespace SsmsLogin
{
    internal static class AutomationElementExtensions
    {
        public static void SetValue(this AutomationElement element, string value)
        {
            if (element.TryGetCurrentPattern(ValuePattern.Pattern, out object pattern)) ((ValuePattern)pattern).SetValue(value);
        }

        public static void Invoke(this AutomationElement element)
        {
            if (element.TryGetCurrentPattern(InvokePattern.Pattern, out object pattern)) ((InvokePattern)pattern).Invoke();
        }
    }
}
