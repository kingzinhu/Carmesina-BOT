using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarmesinaConfig.comandos
{
    class basicos : BaseCommandModule
    {
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.Client.SendMessageAsync(ctx.Channel, "Any test for a while");
        }

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
        public async Task Say(CommandContext ctx, [RemainingText] string texto)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(texto);
            await ctx.Message.DeleteAsync(); 
        }

        [Command("type")]
        [Aliases("typing")]
        [Description("Do Carmesine starts typing")]
        public async Task Type(CommandContext ctx, int tempo = 1000)
        {
            await ctx.TriggerTypingAsync();
            Thread.Sleep(tempo);
        }
    }
}
