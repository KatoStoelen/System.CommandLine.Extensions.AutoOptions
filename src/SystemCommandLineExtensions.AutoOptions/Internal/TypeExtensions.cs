﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SystemCommandLineExtensions.AutoOptions.Internal
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Option> GetOptions(
            this Type optionsType, Func<PropertyInfo, string?> defaultOptionNamer)
        {
            var optionProperties = optionsType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(property => property.GetCustomAttribute<NotAnOptionAttribute>() == null);

            foreach (var optionProperty in optionProperties)
            {
                var defaultOptionName = defaultOptionNamer.Invoke(optionProperty);

                var aliases = new[] { defaultOptionName }
                    .Concat(optionProperty
                        .GetCustomAttributes<AliasAttribute>()
                        .Select(attribute => attribute.Alias))
                    .Where(alias => !string.IsNullOrEmpty(alias))
                    .ToArray();

                if (!aliases.Any())
                {
                    throw new InvalidOperationException(
                        $"Option property '{optionProperty.Name}' on options type '{optionsType.FullName}' has no aliases");
                }

                var description = optionProperty.GetCustomAttribute<DescriptionAttribute>()?.Description;
                var defaultValue = optionProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;

                var closedGenericOptionType = typeof(Option<>).MakeGenericType(optionProperty.PropertyType);

                yield return defaultValue == null
                    ? (Option)Activator.CreateInstance(closedGenericOptionType, aliases, description)!
                    : (Option)Activator.CreateInstance(
                        closedGenericOptionType,
                        aliases,
                        Expression.Lambda(Expression.Constant(defaultValue)).Compile(),
                        description)!;
            }
        }

        public static MethodInfo GetDeclaredMethod(this Type type, string methodName) =>
            type.GetMethod(
                methodName,
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly);
    }
}