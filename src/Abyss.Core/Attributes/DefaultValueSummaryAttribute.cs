﻿using System;

namespace Abyss.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DefaultValueDescriptionAttribute : Attribute
    {
        public DefaultValueDescriptionAttribute(string defaultValueDescription)
        {
            DefaultValueDescription = defaultValueDescription;
        }

        public string DefaultValueDescription { get; }
    }
}