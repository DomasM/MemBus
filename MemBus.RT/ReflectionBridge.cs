﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MemBus.Subscribing;

namespace MemBus
{
    /// <summary>
    /// Used to be able to use the old API against the type
    /// </summary>
    static class ReflectionBridge
    {
        public static bool IsAssignableFrom(this Type type, Type otherType)
        {
            return type.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());
        }

        public static void Raise(this EventHandler handler, object sender)
        {
            if (handler != null)
                handler(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Ask some class whether it implements a certain interface
        /// </summary>
        /// <typeparam name="T">The interface you are looking for</typeparam>
        /// <param name="type">the inspected type</param>
        /// <returns>true if the interface is imaplemented</returns>
        public static bool ImplementsInterface<T>(this Type type) where T : class
        {
            return type.GetTypeInfo().ImplementedInterfaces.Any(t => t == typeof(T));
        }

        public static IEnumerable<MethodInfo> MethodCandidatesForSubscriptionBuilders(
            this Type reflectedType,
            Func<MethodInfo, bool> methodSelector,
            Func<Type, bool> returntypePredicate)
        {
            var disposeTokenMethod = reflectedType.ImplementsInterface<IAcceptDisposeToken>()
                                         ? (mi =>
                                            mi.Name == "Accept" && mi.GetParameters().Length == 1 &&
                                            mi.GetParameters()[0].ParameterType == typeof(IDisposable))
                                         : new Func<MethodInfo, bool>(mi => false);

            var candidates =
                (from mi in reflectedType.GetTypeInfo().DeclaredMethods.Where(mi => mi.IsPublic && !mi.IsStatic)
                 where
                 !mi.IsGenericMethod &&
                   mi.GetParameters().Length == 1 &&
                   !disposeTokenMethod(mi) &&
                   returntypePredicate(mi.ReturnType) &&
                   methodSelector(mi)
                 select mi);
            return candidates;
        }
    }
}