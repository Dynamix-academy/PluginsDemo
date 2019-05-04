using System;
using Microsoft.Xrm.Sdk;
namespace PluginsDemo
{
    public class SecureAndUnsecure : IPlugin
    {
        public SecureAndUnsecure(string unsecure, string secure)
        {

            var unsec = unsecure;
            var sec = secure;
        }

        public void Execute(IServiceProvider serviceProvider)
        {

        }
    }
}
