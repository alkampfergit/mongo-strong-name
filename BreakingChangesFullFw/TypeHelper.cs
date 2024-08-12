using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BreakingChangesFullFw
{
    public static class TypeHelper
    {
        private static Dictionary<string, Assembly> _mongodbAssembly = new Dictionary<string, Assembly>();

        static TypeHelper()
        {
            var driver = Assembly.Load("MongoDB.Driver");
            _mongodbAssembly.Add("MongoDB.Driver", driver);

            var bson = Assembly.Load("MongoDB.Bson");
            _mongodbAssembly.Add("MongoDB.Bson", bson);

            var core = Assembly.Load("MongoDB.Driver.Core");
            _mongodbAssembly.Add("MongoDB.Driver.Core", core);
        }

        internal static Assembly ResolveEventHandler(object sender, ResolveEventArgs args)
        {
            var asmName = new AssemblyName(args.Name);
            if (_mongodbAssembly.TryGetValue(asmName.Name, out var assembly))
            {
                return assembly;
            }
            return null;
        }
    }
}
