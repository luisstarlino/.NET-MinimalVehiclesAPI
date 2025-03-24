using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NET_MinimalAPI.test.Tests.Setup
{
    [TestClass]
    public class TestStartup
    {
        public static WebApplication? App { get; private set; }
        public static IServiceProvider? ServiceProvider { get; private set; }

        [AssemblyInitialize] // --- Run a unique time before test
        public static void Initialize(TestContext context)
        {
            var builder = WebApplication.CreateBuilder();

            // --- config and start the services (using the setupTest)
            SetupTest.ConfigureServices(builder);

            App = builder.Build();

            // --- middleware, controllers
            SetupTest.ConfigureApp(App);

            // --- DI using service provideer to tests
            ServiceProvider = App.Services;

            Console.WriteLine("Test environment initialized. ✅");
        }
    }
}
