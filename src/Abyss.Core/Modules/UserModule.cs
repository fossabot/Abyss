﻿using Abyss.Core.Attributes;
using Abyss.Core.Entities;
using Abyss.Core.Extensions;
using Abyss.Core.Results;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Qmmands;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Core.Modules
{
    [Name("User Information")]
    [Description("Commands that help you interact with other Discord users.")]
    public class UserModule : AbyssModuleBase
    {
        private readonly AbyssConfig _config;

        public UserModule(AbyssConfig config)
        {
            _config = config;
        }

        [Command("Avatar")]
        [Description("Grabs the avatar for a user.")]
        [Example("avatar", "avatar @OtherUser")]
        [ResponseFormatOptions(ResponseFormatOptions.DontAttachFooter | ResponseFormatOptions.DontAttachTimestamp)]
        public Task<ActionResult> Command_GetAvatarAsync(
            [Name("User")]
            [Description("The user who you wish to get the avatar for.")]
            [DefaultValueDescription("The user who invoked this command.")]
            SocketGuildUser target = null,
            [Name("Image_Size")] [Description("The size of the resulting image, in pixels.")]
            int size = 1024)
        {
            target = target ?? Context.Invoker;

            return Ok(a =>
            {
                a.WithAuthor(target.ToEmbedAuthorBuilder());
                a.ImageUrl = target.GetEffectiveAvatarUrl(Convert.ToUInt16(size));
            });
        }

        [Command("User", "UserInfo")]
        [Example("user", "user @OtherUser")]
        [Description("Grabs information around a member.")]
        public Task<ActionResult> Command_GetUserInfoAsync(
            [Name("Member")]
            [Description("The user to get information for.")]
            [DefaultValueDescription("The user who invoked this command.")]
            SocketGuildUser member = null)
        {
            member = member ?? Context.Invoker;

            var embed = new EmbedBuilder
            {
                ThumbnailUrl = member.GetEffectiveAvatarUrl(256),
                Color = member.GetHighestRoleColourOrDefault(),
                Author = member.ToEmbedAuthorBuilder()
            };

            var desc = new StringBuilder();

            string FormatActivity(IActivity activity)
            {
                if (activity is SpotifyGame spotify)
                {
                    return $"Listening to {spotify.TrackTitle} by {spotify.Artists.Humanize()}";
                }
                return $"{activity.Type.Humanize()} {activity.Name}";
            }

            var effectiveColor = member.GetHighestRoleColour();

            desc.AppendLine($"**- ID:** {member.Id}");
            desc.AppendLine($"**- Created:** {FormatOffset(member.CreatedAt)}");
            if (member.JoinedAt != null) desc.AppendLine($"**- Joined:** {FormatOffset(member.JoinedAt.Value)}");
            desc.AppendLine(
                $"**- Position:** {(member.Hierarchy == int.MaxValue ? "Server Owner" : member.Hierarchy.Ordinalize())}");
            desc.AppendLine($"**- Deafened:** {(member.IsDeafened ? "Yes" : "No")}");
            desc.AppendLine($"**- Muted:** {(member.IsMuted ? "Yes" : "No")}");
            desc.AppendLine($"**- Nickname:** {member.Nickname ?? "None"}");
            desc.AppendLine($"**- Voice Status:** {GetVoiceChannelStatus(member)}");
            if (member.Activity != null)
                desc.AppendLine($"**- Activity:** {FormatActivity(member.Activity)}");
            desc.AppendLine($"**- Status:** {_config.Emotes.GetEmoteFromActivity(member.Status)} {member.Status.Humanize()}");
            desc.AppendLine($"**- Mutual servers:** {member.MutualGuilds.Count}");
            if (member.PremiumSince != null) desc.AppendLine($"**- Nitro membership since: {FormatOffset(member.PremiumSince.Value)}");
            if (member.ActiveClients != null) desc.AppendLine($"**- Active on:** {string.Join(", ", member.ActiveClients)}");
            if (effectiveColor != null) desc.AppendLine($"**- Colour:** {effectiveColor.Value.ToString()} (R {effectiveColor.Value.R}, G {effectiveColor.Value.G}, B {effectiveColor.Value.B})");

            embed.Description = desc.ToString();

            var roles = member.Roles.Where(r => !r.IsEveryone);
            var socketRoles = roles as SocketRole[] ?? roles.ToArray();

            embed.AddField($"{(socketRoles.Length > 0 ? socketRoles.Length.ToString() : "No")} role{(socketRoles.Length == 1 ? "" : "s")}", socketRoles.Length > 0 ? string.Join(", ", socketRoles.Select(r => r.Name)) : BotService.ZeroWidthSpace);

            return Ok(embed);
        }

        [Command("Hug")]
        [Example("hug @OtherUser", "hug @me")]
        [Description("Gives them all your hugging potential.")]
        [ResponseFormatOptions(ResponseFormatOptions.DontAttachFooter | ResponseFormatOptions.DontAttachTimestamp)]
        public Task<ActionResult> Command_HugUserAsync([Name("Member")] [Description("The user to hug.")]
            SocketGuildUser hugee)
        {
            if (hugee.Id == Context.Invoker.Id) return Ok("You hug yourself.");

            return Ok(e =>
            {
                e.Description = $"**{Context.Invoker.GetActualName()}** hugs **{hugee.GetActualName()}**!";
                e.ImageUrl = "http://i.imgur.com/6bJxUOb.gif";
            });
        }

        public static string FormatOffset(DateTimeOffset offset)
        {
            return offset.DateTime.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture);
        }

        private static string GetVoiceChannelStatus(SocketGuildUser user)
        {
            return user.VoiceState == null ? "Not in a voice channel" : $"In {user.VoiceChannel.Name}";
        }
    }
}