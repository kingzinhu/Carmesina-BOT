using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarmesinaConfig;
using DSharpPlus.Interactivity.Extensions;

namespace CarmesinaConfig.comandos
{
    class basicos : BaseCommandModule
    {
        static DiscordEmbed EmbedComum(string texto, string cor = null)
        {
            if (cor == null) { cor = "ffaafd"; }
            var builder = new DiscordEmbedBuilder()
                .WithDescription(texto)
                .WithColor(new DiscordColor(cor));
            var embed = builder.Build();
            return embed;
        }

        [Command("test")]
        [Description("Test some function")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
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
                await ctx.RespondAsync(EmbedComum("<:lamp:816411488356270141> *Aren't you forgetting anything?*"));
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
                await ctx.RespondAsync(EmbedComum("<:lamp:816411488356270141> You need enter a quantity!"));
            }
            else
            {
                var messages = await ctx.Channel.GetMessagesBeforeAsync(ctx.Message.Id, qnts);
                await ctx.Channel.DeleteMessagesAsync(messages);
                await ctx.Message.DeleteAsync();
                await ctx.RespondAsync(EmbedComum($"<:8bitminus:816411488091766795> `{qnts} deleted messages` ***No one will ever know what happened here... <:shy:816399461675696159>***"));
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
                .WithAuthor("Kingzinhu", "https://twitter.com/kingzinhur", ctx.User.AvatarUrl)
                .WithTitle("<:GitHub:817168795302363177> Take a look on my GitHub repository ^-^")
                .WithDescription("https://github.com/kingzinhu/Carmesina-BOT")
                .WithColor(new DiscordColor("ffaafd"));
            await ctx.RespondAsync(embed.Build());
        }

        [Command("survey")]
        [Aliases("vote", "voting", "poll")]
        [Description("Make a custom survey")]
        public async Task Survey(CommandContext ctx, int qnt = 1)
        {
            await ctx.TriggerTypingAsync();
            await ctx.Message.DeleteAsync();
            var pergunta = await ctx.RespondAsync(EmbedComum("<:lamp:816411488356270141> **What's the survey's theme?**"));
            var resposta = await ctx.Message.GetNextMessageAsync(TimeSpan.FromSeconds(15));
            if (!resposta.TimedOut)
            {
                await pergunta.DeleteAsync();
                await resposta.Result.DeleteAsync();
                await ctx.TriggerTypingAsync();
                var pergunta2 = await ctx.RespondAsync(EmbedComum("<:lamp:816411488356270141> **How long (in seconds) this survey gonna belong?**"));
                aguardar_resposta:
                var resposta2 = await ctx.Message.GetNextMessageAsync(TimeSpan.FromSeconds(15));
                
                if (!resposta2.TimedOut)
                {
                    try { int.Parse(resposta2.Result.Content); }
                    catch { await ctx.RespondAsync(EmbedComum("<:Mcross:816411488872038420> **That's not a value. Try again**", "ff0000")); goto aguardar_resposta; }
                    await resposta2.Result.DeleteAsync();
                    await pergunta2.DeleteAsync();
                    var enquete = new DiscordEmbedBuilder()
                        .WithAuthor(ctx.Member.Username, null, ctx.Member.AvatarUrl)
                        .WithDescription(resposta.Result.Content)
                        .WithColor(new DiscordColor("ffaafd"));
                    var msg = await ctx.RespondAsync(enquete.Build());
                    if (qnt > 11) qnt = 11;
                    if (qnt < 0) qnt = 0;
                    if (qnt < 2) { await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":hand_splayed:")); }
                    else
                    {
                        string[] numeros = { ":zero:", ":one:", ":two:", ":three:", ":four:", ":five:",
                            ":six:", ":seven:", ":eight:", ":nine:", ":keycap_ten:" };

                        int adicoes = 0;

                        while (!(adicoes == qnt))
                        {
                            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, numeros[adicoes]));
                            adicoes++;
                        }
                    }
                    
                    var reacoes = await msg.CollectReactionsAsync(TimeSpan.FromSeconds(int.Parse(resposta2.Result.Content)));
                    var resultados = new StringBuilder();
                    await ctx.Client.SendMessageAsync(await ctx.Client.GetChannelAsync(813631033819922452), reacoes.ToString());
                    foreach (var reactions in reacoes)
                    {
                        resultados.AppendLine($"{reactions.Emoji} = `{reactions.Total}`");
                    }

                    await msg.DeleteAllReactionsAsync();
                    var final = new DiscordEmbedBuilder()
                        .WithAuthor(ctx.Member.Username, null, ctx.Member.AvatarUrl)
                        .WithTitle("Finished Survey")
                        .AddField("Survey's name", $"`{resposta.Result.Content}`", true)
                        .AddField("Final result", resultados.ToString())
                        .WithFooter($"Belongs: {resposta2.Result.Content}s")
                        .WithColor(new DiscordColor("ffaafd"));

                    await msg.ModifyAsync(final.Build());
                    var avisar = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor("ffaafd"))
                        .WithTitle($"\"{resposta.Result.Content}\"")
                        .WithUrl(msg.JumpLink)
                        .WithDescription("This survey has been finished!");
                    await ctx.RespondAsync(ctx.User.Mention, avisar.Build());
                } else { await ctx.RespondAsync(EmbedComum("<:offline:816411488385761283> **Time Out**", "ff0000")); }
            } else { await ctx.RespondAsync(EmbedComum("<:offline:816411488385761283> **Time Out**", "ff0000")) ; }
        }
    }
}
