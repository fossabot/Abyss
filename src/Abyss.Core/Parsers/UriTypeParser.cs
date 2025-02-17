﻿using Abyss.Core.Parsers;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Abyss.Core.Parsers
{
    [DiscoverableTypeParser]
    public class UriTypeParser : TypeParser<Uri>, IAbyssTypeParser
    {
        public override ValueTask<TypeParserResult<Uri>> ParseAsync(Parameter parameter, string value, CommandContext context,
            IServiceProvider provider)
        {
            return Uri.IsWellFormedUriString(value, UriKind.Absolute)
                ? TypeParserResult<Uri>.Successful(new Uri(value))
                : TypeParserResult<Uri>.Unsuccessful("Unknown URL.");
        }

        public (string, string, string) FriendlyName => ("A URL of a webpage or image.", "A list of URLs.", null);
    }
}