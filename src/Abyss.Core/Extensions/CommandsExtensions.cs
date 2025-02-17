using Qmmands;
using System;
using System.Linq;

namespace Abyss.Core.Extensions
{
    public static class CommandsExtensions
    {
        public static bool HasAttribute<T>(this Command info, out T attr) where T : Attribute
        {
            if (info.Attributes.FirstOrDefault(b => b is T) is T t)
            {
                attr = t;
                return true;
            }

            attr = default;
            return false;
        }

        public static bool HasAttribute<T>(this Command info) where T : Attribute
        {
            return HasAttribute<T>(info, out _);
        }

        public static bool HasAttribute<T>(this Parameter info, out T attr) where T : Attribute
        {
            if (info.Attributes.FirstOrDefault(b => b is T) is T t)
            {
                attr = t;
                return true;
            }

            attr = default;
            return false;
        }

        public static bool HasAttribute<T>(this Parameter info) where T : Attribute
        {
            return HasAttribute<T>(info, out _);
        }

        public static bool HasAttribute<T>(this Module info, out T attr) where T : Attribute
        {
            if (info.Attributes.FirstOrDefault(b => b is T) is T t)
            {
                attr = t;
                return true;
            }

            attr = default;
            return false;
        }

        public static bool HasAttribute<T>(this Module info) where T : Attribute
        {
            return HasAttribute<T>(info, out _);
        }

        public static string CreateCommandString(this Command command)
        {
            return $"{command.FullAliases.First()} {string.Join(" ", command.Parameters.Select(a => a.Name))}";
        }
    }
}