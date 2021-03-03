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
        static DiscordEmbed EmbedComum(string texto, string cor = null, string avatarUrl = null)
        {
            if (cor == null) { cor = "ffaafd"; }
            var builder = new DiscordEmbedBuilder()
                .WithDescription(texto)
                .WithColor(new DiscordColor(cor));
            if (!(avatarUrl == null))
            {
                builder.WithAuthor(" ", null, avatarUrl);
            }
            var embed = builder.Build();
            return embed;
        }

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
            await ctx.RespondAsync(EmbedComum($":ping_pong: Pong! The current latency is {ctx.Client.Ping}ms"));
        }

        [Command("avatar")]
        [Aliases("usericon")]
        [Description("Show's user's icon")]
        public async Task Avatar(CommandContext ctx, DiscordUser user = null)
        {
            await ctx.TriggerTypingAsync();
            if (user == null) { user = ctx.User; }
            var builder = new DiscordEmbedBuilder()
                .WithDescription($"{user.Mention}'s avatar:")
                .WithImageUrl($"{user.AvatarUrl}")
                .WithColor(new DiscordColor("ffaafd"));
            var embed = builder.Build();
            await ctx.RespondAsync(embed);
        }

        [Command("say")]
        [Description("Repeat the arguments you put after the command")]
        public async Task Say(CommandContext ctx, [RemainingText] string texto = null)
        {
            await ctx.TriggerTypingAsync();
            if (texto == null)
            {
                await ctx.RespondAsync(EmbedComum("Aren't you forgetting anything?", null , 
                    "https://media.discordapp.net/attachments/816569715483738112/816570567712833556/lamp.png"));
            }
            else
            {
                await ctx.RespondAsync(texto);
                await ctx.Message.DeleteAsync();
            }
        }

        [Command("type")]
        [Aliases("typing")]
        [Description("Do Carmesina starts typing")]
        public async Task Type(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
        }

        [Command("purge")]
        [Aliases("clean", "clear")]
        public async Task Purge(CommandContext ctx, int qnts = 0)
        {
            await ctx.TriggerTypingAsync();
            if (qnts == 0)
            {
                await ctx.RespondAsync(EmbedComum("You need to enter a quantity!", null,
                    "https://media.discordapp.net/attachments/816569715483738112/816570567712833556/lamp.png"));
            }
        }
    }
}
