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
            await ctx.RespondAsync($":ping_pong: Pong! The current latency is {ctx.Client.Ping}ms");
        }

        [Command("avatar")]
        [Aliases("usericon")]
        [Description("Show's user's icon")]
        public async Task Avatar(CommandContext ctx, [RemainingText] DiscordUser user)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(user.AvatarUrl);
        }
        [Command("avatar")]
        public async Task Avatar(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(ctx.User.AvatarUrl);
        }

        [Command("say")]
        [Description("Repeat the arguments you put after the command")]
        public async Task Say(CommandContext ctx, [RemainingText] string args = null)
        {
            await ctx.TriggerTypingAsync();
            if (args == null)
            {
                await ctx.RespondAsync($"> <:shy:816399461675696159> i think you forgot to say something," +
                    $" {ctx.Member.Mention}... haven't ya?");
            }
            else
            {
                await ctx.RespondAsync(args);
            }
            await ctx.Message.DeleteAsync();
        }
    }
}
