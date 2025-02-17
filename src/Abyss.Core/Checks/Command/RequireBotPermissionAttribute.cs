using Abyss.Core.Entities;
using Abyss.Core.Extensions;
using Discord;
using Humanizer;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abyss.Core.Checks.Command
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequireBotPermissionAttribute : CheckAttribute, IAbyssCheck
    {
        public readonly List<ChannelPermission> ChannelPermissions = new List<ChannelPermission>();
        public readonly List<GuildPermission> GuildPermissions = new List<GuildPermission>();

        public RequireBotPermissionAttribute(params ChannelPermission[] permissions)
        {
            ChannelPermissions.AddRange(permissions);
        }

        public RequireBotPermissionAttribute(params GuildPermission[] permissions)
        {
            GuildPermissions.AddRange(permissions);
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext context, IServiceProvider provider)
        {
            var discordContext = context.ToRequestContext();

            var cperms = discordContext.BotUser.GetPermissions(discordContext.Channel);
            foreach (var gperm in GuildPermissions)
            {
                if (!discordContext.BotUser.GuildPermissions.Has(gperm) && !discordContext.BotUser.GuildPermissions.Has(GuildPermission.Administrator))
                {
                    return new CheckResult(
                        $"I need the \"{gperm.Humanize()}\" server-level permission.");
                }
            }

            foreach (var cperm in ChannelPermissions)
            {
                if (!cperms.Has(cperm) && !discordContext.BotUser.GuildPermissions.Has(GuildPermission.Administrator))
                {
                    return new CheckResult(
                        $"I need the \"{cperm.Humanize()}\" channel-level permission.");
                }
            }

            return CheckResult.Successful;
        }

        public string GetDescription(AbyssRequestContext requestContext) => ChannelPermissions.Count > 0
            ? $"I need these channel-level permissions: {string.Join(", ", ChannelPermissions.Select(a => a.Humanize()))}."
            : $"I need these server-level permissions: {string.Join(", ", GuildPermissions.Select(a => a.Humanize()))}.";
    }
}