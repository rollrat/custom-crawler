// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawlerDynamics.Utils
{
    /// <summary>
    /// Contains all instance information.
    /// </summary>
    public class InstanceMonitor
    {
        public static Dictionary<string, object> Instances = new Dictionary<string, object>();
    }

    /// <summary>
    /// Class to make lazy instance easier to implement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ILazy<T>
        where T : new()
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() =>
        {
            T instance = new T();
            InstanceMonitor.Instances.Add(instance.GetType().Name.ToLower(), instance);
            return instance;
        });
        public static T Instance => instance.Value;
        public static bool IsValueCreated => instance.IsValueCreated;
    }
}
