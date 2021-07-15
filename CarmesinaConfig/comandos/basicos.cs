using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarmesinaConfig;
using DSharpPlus.Interactivity.Extensions;
using CarmesinaConfig.funcoes;
using System.Net;
using HtmlAgilityPack;
using CarmesinaConfig.data_bank;

namespace CarmesinaConfig.comandos
{
    class basicos : BaseCommandModule
    {

        [Command("test")]
        [Description("Test some function")]
        public async Task Test(CommandContext ctx, [RemainingText] string id = "")
        {
            await ctx.TriggerTypingAsync();

            await ctx.RespondAsync("opa");

            DiscordMessage mensagem = await ctx.RespondAsync("cu");

            DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":thumbsup:");

            opars:

            await mensagem.CreateReactionAsync(emoji);

            var reacao = await mensagem.WaitForReactionAsync(ctx.Member, TimeSpan.FromSeconds(8f));

            if (!reacao.TimedOut){
                if (reacao.Result.Emoji == emoji)
                {
                    await ctx.RespondAsync("oi");
                    await mensagem.DeleteAllReactionsAsync();
                    goto opars;
                } 
            }
        }

        [Command("ping")]
        [Description("Shows Carmesina's ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(simples.EmbedComum($":ping_pong: Pong! The current latency is {ctx.Client.Ping}ms"));
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
                .WithImageUrl(user.AvatarUrl)
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
                await ctx.RespondAsync(simples.EmbedComum("<:lamp:816411488356270141> *Aren't you forgetting anything?*"));
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
        [Description("Cleans a quantity of messages in a channel")]
        public async Task Purge(CommandContext ctx, int qnts = 0)
        {
            await ctx.TriggerTypingAsync();
            if (qnts == 0)
            {
                await ctx.RespondAsync(simples.EmbedComum("<:lamp:816411488356270141> You need enter a quantity!"));
            }
            else
            {
                var messages = await ctx.Channel.GetMessagesBeforeAsync(ctx.Message.Id, qnts);
                await ctx.Channel.DeleteMessagesAsync(messages);
                await ctx.Message.DeleteAsync();
                await ctx.RespondAsync(simples.EmbedComum($"<:8bitminus:816411488091766795> `{qnts} deleted messages` ***No one will ever know what happened here... <:shy:816399461675696159>***"));
            }
        }

        [Command("github")]
        [Aliases("git")]
        [Description("Show's Carmesina's repository GitHub page")]
        public async Task GitHub(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var embed = new DiscordEmbedBuilder()
                .WithThumbnail("https://cdn.discordapp.com/attachments/816569715483738112/817164146453774336/githubcarmesina.png")
                .WithAuthor("Kingzinhu", "https://twitter.com/kingzinhur", "https://cdn.discordapp.com/attachments/816569715483738112/845712042682089532/2b104ee7173622c6bd5a8652c45f8bc6.png")
                .WithTitle("<:GitHub:817168795302363177> Take a look on my GitHub repository ^-^")
                .WithDescription("https://github.com/kingzinhu/Carmesina-BOT")
                .WithColor(new DiscordColor("ffaafd"));
            await ctx.RespondAsync(embed.Build());
        }
    }
}
