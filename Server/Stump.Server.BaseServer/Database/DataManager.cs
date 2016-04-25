using System;
using System.Reflection;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.BaseServer.Database
{
    public abstract class DataManager
    {
        public virtual void Initialize()
        {
        }

        public virtual void TearDown()
        {
        }
    }

    public abstract class DataManager<T> : Singleton<T> where T : class
    {
        public virtual void Initialize()
        {
        }

        public virtual void TearDown()
        {
        }
    }

    public static class DataManagerAllocator
    {
        public static Assembly Assembly;

        [Initialization(InitializationPass.First, "Initialize DataManagers")]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsAbstract || !type.IsSubclassOfGeneric(typeof (DataManager<>)) ||
                    type == typeof (DataManager<>)) continue;

                var method = type.GetMethod("Initialize", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                // if the method is already managed we don't call it
                if (method.GetCustomAttribute<InitializationAttribute>(true) != null)
                    continue;

                object instance = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static).
                    GetValue(null, new object[0]);
                method.Invoke(instance, new object[0]);
            }
        }
    }
}