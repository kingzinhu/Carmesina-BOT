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
using System;
using System.Collections.Generic;
using System.Text;
using CarmesinaConfig.funcoes;

namespace CarmesinaConfig.comandos
{
    class Exclusivos : BaseCommandModule
    {
        static ulong[] Donos = { 348664615175192577, 380512056413257729, 699992121347801139 };

        [Command("survey")]
        [Aliases("vote", "voting", "poll")]
        [Description("Make a custom survey")]
        public async Task Survey(CommandContext ctx, int qnt = 1)
        {
            if (simples.ContemUlong(ctx.Member.Id, Donos))
            {
                await ctx.TriggerTypingAsync();
                await ctx.Message.DeleteAsync();
                var pergunta = await ctx.RespondAsync(null, false, simples.EmbedComum("<:lamp:816411488356270141> **What's the survey's theme?**"));
                var resposta = await ctx.Message.GetNextMessageAsync(TimeSpan.FromSeconds(15));
                if (!resposta.TimedOut)
                {
                    await pergunta.DeleteAsync();
                    await resposta.Result.DeleteAsync();
                    await ctx.TriggerTypingAsync();
                    var pergunta2 = await ctx.RespondAsync(null, false, simples.EmbedComum("<:lamp:816411488356270141> **How long (in seconds) is this survey timeout?**"));
                    aguardar_resposta:
                    var resposta2 = await ctx.Message.GetNextMessageAsync(TimeSpan.FromSeconds(15));

                    if (!resposta2.TimedOut)
                    {
                        try { int.Parse(resposta2.Result.Content); }
                        catch { await ctx.RespondAsync(null, false, simples.EmbedComum("<:Mcross:816411488872038420> **That's not a value. Try again**", "ff0000")); goto aguardar_resposta; }
                        await resposta2.Result.DeleteAsync();
                        await pergunta2.DeleteAsync();
                        var enquete = new DiscordEmbedBuilder()
                            .WithAuthor(ctx.Member.Username, null, ctx.Member.AvatarUrl)
                            .WithDescription(resposta.Result.Content)
                            .WithColor(new DiscordColor("ffaafd"));
                        var msg = await ctx.RespondAsync(null, false, enquete.Build());
                        if (qnt > 11) qnt = 11;
                        if (qnt < 0) qnt = 0;
                        if (qnt == 0)
                        {
                            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":thumbsup:"));
                            await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":thumbsdown:"));
                        }

                        else if (qnt == 1) { await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":hand_splayed:")); }
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

                        if (qnt == 1) resultados.AppendLine($"{reacoes[0]} = `{reacoes.Count}`");
                        else
                        {
                            foreach (var reactions in reacoes)
                            {
                                resultados.AppendLine($"{reactions.Emoji} = `{reactions.Total}`");
                            }
                        }

                        await msg.DeleteAllReactionsAsync();
                        var final = new DiscordEmbedBuilder()
                            .WithAuthor(ctx.Member.Username, null, ctx.Member.AvatarUrl)
                            .WithTitle("Finished Survey")
                            .AddField("Survey's name", $"`{resposta.Result.Content}`", true)
                            .AddField("Final result", resultados.ToString())
                            .WithFooter($"Belongs: {resposta2.Result.Content}s")
                            .WithColor(new DiscordColor("ffaafd"));

                        await msg.ModifyAsync(null, final.Build());
                        var avisar = new DiscordEmbedBuilder()
                            .WithColor(new DiscordColor("ffaafd"))
                            .WithTitle($"\"{resposta.Result.Content}\"")
                            .WithUrl(msg.JumpLink)
                            .WithDescription("This survey has been finished!");
                        await ctx.RespondAsync(ctx.User.Mention, false, avisar.Build());
                    }
                    else { await ctx.RespondAsync(null, false, simples.EmbedComum("<:offline:816411488385761283> **Time Out**", "ff0000")); }
                }
                else { await ctx.RespondAsync(null, false, simples.EmbedComum("<:offline:816411488385761283> **Time Out**", "ff0000")); }
            }
            else { await ctx.RespondAsync(null, false, simples.EmbedComum("<:Mcross:816411488872038420> **Only Carmesina's owners can use this command**", "ff0000")); }
        }
    }
}
