using NUnit.Framework;
using System.Runtime.CompilerServices;
namespace AffinityWatcher.Tests
{
    /// <summary>
    /// Tests utilites.
    /// </summary>
    internal static class Utils
    {
        internal static void MethodStart([CallerMemberName] string memberName = "")
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{memberName} start.");
        }

        internal static void MethodComplete([CallerMemberName] string memberName = "")
        {
            TestContext.WriteLine($@"{memberName} complete.");
        }
    }
}
