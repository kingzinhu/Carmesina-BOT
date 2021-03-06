﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarmesinaConfig;
using DSharpPlus.Interactivity.Extensions;
using CarmesinaConfig.funcoes;
using HtmlAgilityPack;

namespace CarmesinaConfig.comandos
{
    class receitas : BaseCommandModule
    {
        [Command("revenue")]
        [Aliases("receita")]

        public async Task Revenue(CommandContext ctx, [RemainingText] string comida = null)
        {
            if (comida != null)
            {
                try
                {
                    await ctx.TriggerTypingAsync();
                    comida = simples.Join(comida, "+");

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc = new HtmlWeb().Load($"https://www.tudogostoso.com.br/busca?q={comida}");

                    HtmlNode resultados = doc.DocumentNode.SelectSingleNode("/html/body/div[5]/div[1]/div[2]/div[2]/div");
                    HtmlNode primeiro = resultados.ChildNodes[1];
                    primeiro = primeiro.ChildNodes[primeiro.ChildNodes.Count - 1];

                    HtmlAgilityPack.HtmlDocument pagina = new HtmlAgilityPack.HtmlDocument();
                    pagina = new HtmlWeb().Load("https://www.tudogostoso.com.br" + primeiro.OuterHtml.Substring(9, primeiro.OuterHtml.IndexOf("class") - 11));

                    HtmlNode infos = pagina.DocumentNode.SelectSingleNode("/html/body/div[5]/div[1]/div/div[3]/div[1]/div/div[1]/div[2]").ChildNodes[3];

                    string tempo_de_preparo = simples.PrimeiraInt(infos.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText);
                    string rendimento = simples.PrimeiraInt(infos.ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[5].InnerText);
                    string autor = simples.PegarLetras(infos.ChildNodes[1].ChildNodes[1].ChildNodes[9].ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerText);
                    string nome = pagina.DocumentNode.SelectSingleNode("/html/body/div[5]/div[1]/div/div[3]/div[1]/div/div[1]/div[1]/div[1]/h1").InnerText;

                    HtmlNode row = pagina.DocumentNode.SelectSingleNode("/html/body/div[5]/div[1]/div/div[3]/div[4]/div/div[1]");
                    HtmlNode row2 = pagina.DocumentNode.SelectSingleNode("/html/body/div[5]/div[1]/div/div[3]/div[5]/div/div[1]/div");

                    List<string> ingredientes = new List<string>();
                    List<string> ingredientesTop = new List<string>();

                    List<string> preparo = new List<string>();
                    List<string> preparoTop = new List<string>();

                    if (row.ChildNodes.Count > 5)
                    {
                        int pos = 0;
                        int intervalo = 0;

                        foreach (HtmlNode node in row.ChildNodes)
                        {
                            pos++;

                            if (pos == 5) intervalo++;
                            if (intervalo == 1)
                            {
                                ingredientesTop.Add(simples.PegarLetras(node.InnerText));
                                intervalo++;
                            }
                            else if (intervalo == 2)
                            {
                                string res = "";
                                foreach (HtmlNode n in node.ChildNodes)
                                {
                                    res += "• " + simples.FormatarAcentosHtml(n.InnerText) + "\n\n";
                                    intervalo = 1;
                                }
                                ingredientes.Add(res);
                            }
                        }
                    }
                    else
                    {
                        ingredientesTop.Add("");

                        string res = "";
                        foreach (HtmlNode n in row.ChildNodes[3].ChildNodes)
                        {
                            res += "• " + simples.FormatarAcentosHtml(n.InnerText) + "\n\n";
                        }
                        ingredientes.Add(res);
                    }

                    if (row2.ChildNodes.Count > 3)
                    {
                        int pos = 0;
                        int intervalo = 0;

                        foreach (HtmlNode node in row2.ChildNodes)
                        {
                            pos++;

                            if (pos == 3) intervalo++;
                            if (intervalo == 1)
                            {
                                preparoTop.Add(simples.PegarLetras(node.InnerText));
                                intervalo++;
                            }
                            else if (intervalo == 2)
                            {
                                int c = 0;
                                string res = "";
                                foreach (HtmlNode n in node.ChildNodes)
                                {
                                    c++;
                                    res += c + " - " + simples.FormatarAcentosHtml(n.InnerText) + "\n\n";
                                    intervalo = 1;
                                }
                                preparo.Add(res);
                            }
                        }
                    }
                    else
                    {
                        preparoTop.Add("");

                        int c = 0;
                        string res = "";
                        foreach (HtmlNode n in row2.ChildNodes[1].ChildNodes)
                        {
                            c++;
                            res += c + " - " + simples.FormatarAcentosHtml(n.InnerText) + "\n\n";
                        }
                        preparo.Add(res);
                    }

                    DiscordEmbedBuilder embedP = new DiscordEmbedBuilder()
                        .WithAuthor(nome, "https://www.tudogostoso.com.br" + primeiro.OuterHtml.Substring(9, primeiro.OuterHtml.IndexOf("class") - 11))
                        .AddField(":clock1: Tempo:", tempo_de_preparo + " min", true)
                        .AddField(":rice: Rendimento:", rendimento + " porções", true)
                        .WithDescription(":bread: - Ingredientes \n\n :pencil: - Modo de preparo")
                        .WithFooter("Feito por " + autor)
                        .WithColor(new DiscordColor("ffaafd"));

                    int cenario = 0;

                    var mensagem = await ctx.RespondAsync(embedP.Build());

                    DiscordEmoji direita = DiscordEmoji.FromName(ctx.Client, ":arrow_forward:");
                    DiscordEmoji esquerda = DiscordEmoji.FromName(ctx.Client, ":arrow_backward:");
                    DiscordEmoji pao = DiscordEmoji.FromName(ctx.Client, ":bread:");
                    DiscordEmoji pencil = DiscordEmoji.FromName(ctx.Client, ":pencil:");
                    DiscordEmoji casa = DiscordEmoji.FromName(ctx.Client, ":house:");

                    DiscordColor rosinha = new DiscordColor("ffaafd");

                    int indiceIngredientes = 0;
                    int indicePreparo = 0;

                    DiscordEmbedBuilder embedIn = new DiscordEmbedBuilder();
                    DiscordEmbedBuilder embedPre = new DiscordEmbedBuilder();

                atualizar:

                    await mensagem.DeleteAllReactionsAsync();

                    if (ingredientesTop.Count > 1)
                    {
                        embedIn
                            .WithAuthor(ingredientesTop[indiceIngredientes])
                            .WithDescription(ingredientes[indiceIngredientes])
                            .WithFooter($"{indiceIngredientes + 1} / {ingredientesTop.Count - 1}")
                            .WithColor(rosinha);
                    }
                    else
                    {
                        embedIn
                            .WithAuthor("Ingredientes")
                            .WithDescription(ingredientes[0])
                            .WithColor(rosinha);
                    }

                    if (preparoTop.Count > 1)
                    {
                        embedPre
                            .WithAuthor(preparoTop[indicePreparo])
                            .WithDescription(preparo[indicePreparo])
                            .WithFooter($"{indicePreparo + 1} / {preparoTop.Count - 1}")
                            .WithColor(rosinha);
                    }
                    else
                    {
                        embedPre
                            .WithAuthor("Modo de preparo")
                            .WithDescription(preparo[0])
                            .WithColor(rosinha);
                    }

                    if (cenario == 0)
                    {
                        await mensagem.ModifyAsync(embedP.Build());

                        await mensagem.CreateReactionAsync(pao);
                        await mensagem.CreateReactionAsync(pencil);

                        var reacao = await mensagem.WaitForReactionAsync(ctx.Member, TimeSpan.FromSeconds(30f));

                        if (!reacao.TimedOut)
                        {

                            if (reacao.Result.Emoji == pao)
                            {
                                await mensagem.ModifyAsync(embedIn.Build());
                                cenario = 1;
                                goto atualizar;
                            }

                            if (reacao.Result.Emoji == pencil)
                            {
                                await mensagem.ModifyAsync(embedPre.Build());
                                cenario = 2;
                                goto atualizar;
                            }
                        }
                    }

                    if (cenario == 1)
                    {
                        await mensagem.ModifyAsync(embedIn.Build());

                        await mensagem.CreateReactionAsync(casa);

                        if (ingredientesTop.Count > 1)
                        {
                            await mensagem.CreateReactionAsync(esquerda);
                            await mensagem.CreateReactionAsync(direita);

                        }

                        var reacao = await mensagem.WaitForReactionAsync(ctx.Member);

                        if (!reacao.TimedOut)
                        {
                            if (reacao.Result.Emoji == direita)
                            {
                                if (!(indiceIngredientes >= ingredientesTop.Count - 2)) indiceIngredientes++;
                                goto atualizar;
                            }

                            if (reacao.Result.Emoji == esquerda)
                            {
                                if (!(indiceIngredientes <= 0)) indiceIngredientes--;
                                goto atualizar;
                            }

                            if (reacao.Result.Emoji == casa) cenario = 0;
                            goto atualizar;
                        }
                    }

                    if (cenario == 2)
                    {
                        await mensagem.ModifyAsync(embedPre.Build());

                        await mensagem.CreateReactionAsync(casa);

                        if (preparoTop.Count > 1)
                        {
                            await mensagem.CreateReactionAsync(esquerda);
                            await mensagem.CreateReactionAsync(direita);

                        }

                        var reacao = await mensagem.WaitForReactionAsync(ctx.Member);

                        if (!reacao.TimedOut)
                        {
                            if (reacao.Result.Emoji == direita)
                            {
                                if (!(indicePreparo >= preparoTop.Count - 2)) indicePreparo++;
                                goto atualizar;
                            }

                            if (reacao.Result.Emoji == esquerda)
                            {
                                if (!(indicePreparo <= 0)) indicePreparo--;
                                goto atualizar;
                            }

                            if (reacao.Result.Emoji == casa) cenario = 0;
                            goto atualizar;
                        }
                    }


                }
                catch
                {
                    DiscordEmbedBuilder erro = new DiscordEmbedBuilder()
                        .WithAuthor("Erro!!!", null, "https://media.discordapp.net/attachments/816569715483738112/845787591751106600/8bitcross.png")
                        .AddField("Isso pode ter acontecido por 4 motivos:", "1 - Incompetência do King_ " +
                        "\n\n 2 - Você escreveu algo errado " +
                        "\n\n 3 - Essa receita não deve existir no site em que busco informações" +
                        "\n\n4 - Não faço ideia")
                        .WithColor(new DiscordColor("ff0000"));

                    await ctx.RespondAsync(erro.Build());
                }
            }
            else
            {
                await ctx.RespondAsync(simples.EmbedComum("<:lamp:816411488356270141> *Aren't you forgetting anything?*"));
            }
            
        }
    }
}
