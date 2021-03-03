using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarmesinaConfig.comandos
{
    class basicos : BaseCommandModule
    {
        [Command("ping")]
        [Description("Shows Carmesina's ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($":ping_pong: Pong! The current latency is {ctx.Client.Ping.ToString()}ms");
        }

        [Command("avatar")]
        [Aliases("usericon")]
        [Description("Show's user's icon")]
        public async Task Avatar(CommandContext ctx, [RemainingText] DiscordUser user = null)
        {
            await ctx.TriggerTypingAsync();
            if (user == null) { user = ctx.User; };
            await ctx.RespondAsync(user.AvatarUrl);
        }

    }
}
