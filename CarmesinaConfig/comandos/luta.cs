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
using CarmesinaConfig.funcoes;
using CarmesinaConfig.data_bank;
using CarmesinaConfig.classes;
using DSharpPlus;

namespace CarmesinaConfig.comandos
{
    class luta : BaseCommandModule
    {
        //⚔️🛡️❤️👤🏃🪦

        string linha = "<:linhameio:850650956806619206><:linhameio:850650956806619206><:linhameio:850650956806619206>" +
            "<:linhameio:850650956806619206><:linhameio:850650956806619206><:linhadireita:850650956759826443>";

        string invi = "<:invisible:850656527035793418>";

        string donos = "348664615175192577";

        List<string> log = new List<string>();

        [Command("registrar")]
        public async Task Registrar(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            if (Db_luta.TemId(ctx.Member.Id.ToString())) await ctx.RespondAsync(simples.EmbedComum("Você já tem um save!"));
            else
            {
                string dia = DateTime.Now.Day.ToString();
                string mes = DateTime.Now.Month.ToString();
                string ano = DateTime.Now.Year.ToString();

                if (int.Parse(dia) < 10) dia = "0" + dia;
                if (int.Parse(mes) < 10) mes = "0" + mes;

                Db_luta.AddData(ctx.Member.Id.ToString(), ctx.Member.Username);
                Db_luta.UpdateValue(ctx.Member.Id.ToString(), "SINCE", $"{dia}/{mes}/{ano}");
                await ctx.RespondAsync(simples.EmbedComum("Save criado!"));
            }
        }

        [Command("hospital")]
        public async Task Hospital(CommandContext ctx, DiscordUser user = null)
        {
            DiscordEmoji emoji = await ctx.Client.GetGuildAsync(813625355842617385).Result.GetEmojiAsync(816411490776514560);
            DiscordEmoji cancel = await ctx.Client.GetGuildAsync(813625355842617385).Result.GetEmojiAsync(816411488872038420);

            bool paraSi = true;

            if (user == null) user = ctx.Member as DiscordUser;
            else paraSi = false;

            int hp = int.Parse(Db_luta.GetValue(user.Id.ToString(), "HP"));
            int maxhp = int.Parse(Db_luta.GetValue(user.Id.ToString(), "MAX_HP"));
            string nome = user.Username;

            if (!(maxhp - hp == 0))
            {
                DiscordMessage mensagem;

                if (paraSi)
                {
                    mensagem = await ctx.RespondAsync(simples.EmbedComum($"`{ctx.Member.Username}`, deseja mesmo recuperar toda a sua vida por " +
                        $"**{maxhp - hp}** :coin:?"));
                }
                else
                {
                    mensagem = await ctx.RespondAsync(simples.EmbedComum($"`{ctx.Member.Username}`, deseja mesmo recuperar toda a vida de `{nome}` por " +
                        $"**{maxhp - hp}** :coin:?"));
                }
                

                await mensagem.CreateReactionAsync(emoji);

                var resultado = await mensagem.WaitForReactionAsync(ctx.Member, TimeSpan.FromSeconds(20));

                await mensagem.DeleteAllReactionsAsync();

                if (!resultado.TimedOut)
                {
                    if (resultado.Result.Emoji == emoji)
                    {
                        int moedas = int.Parse(Db_luta.GetValue(ctx.Member.Id.ToString(), "COINS"));
                        try
                        {
                            if (moedas >= maxhp - hp)
                            {
                                if (paraSi) Db_luta.UpdateValue(ctx.Member.Id.ToString(), "HP", maxhp.ToString());
                                else Db_luta.UpdateValue(user.Id.ToString(), "HP", maxhp.ToString());

                                Db_luta.UpdateValue(ctx.Member.Id.ToString(), "COINS", (moedas - (maxhp - hp)).ToString());

                                await mensagem.ModifyAsync(simples.EmbedComum($"Vida de {nome} restaurada com sucesso! <:Mcheck:816411490776514560>", "00ff00"));
                            }
                            else await mensagem.ModifyAsync(simples.EmbedComum("Você não tem moedas o suficiente! <:Mcross:816411488872038420>", "ff0000"));
                        }
                        catch (Exception e)
                        {
                            await ctx.RespondAsync(simples.EmbedComum("ERRO: " + e.Message, "ff0000"));
                        }
                    }
                    else
                    {
                        await mensagem.ModifyAsync(simples.EmbedComum("Compra cancelada. <:Mcross:816411488872038420>", "ff0000"));
                    }
                }
                else await mensagem.ModifyAsync(simples.EmbedComum("<:offline:816411488385761283> Tempo esgotado", "a0a0a0"));
            }
            else
            {
                await ctx.RespondAsync(simples.EmbedComum($"`{nome}`, você já está com a vida cheia!", "ff0000"));
            }

            
        }

        [Command("inventory")]
        [Aliases("inv", "perfil", "inventario")]
        public async Task Inventory(CommandContext ctx, DiscordMember user = null)
        {
            if (user == null) user = ctx.Member;
            try
            {
                await ctx.TriggerTypingAsync();

                if (!Db_luta.TemId(ctx.Member.Id.ToString()))
                {
                    await ctx.RespondAsync(simples.EmbedComum("Você precisa de um save!!! Ditie `c.registrar` para criar um novo save."));
                }
                else
                {
                    Db_luta.UpdateData(user.Username, user.Id.ToString());

                    int xp = int.Parse(Db_luta.GetValue(user.Id.ToString(), "XP"));
                    int hp = int.Parse(Db_luta.GetValue(user.Id.ToString(), "HP"));
                    int maxhp = int.Parse(Db_luta.GetValue(user.Id.ToString(), "MAX_HP"));
                    int lvl = int.Parse(Db_luta.GetValue(user.Id.ToString(), "LEVEL"));
                    int dmg = int.Parse(Db_luta.GetValue(user.Id.ToString(), "DAMAGE"));
                    int prtc = int.Parse(Db_luta.GetValue(user.Id.ToString(), "PROTECTION"));
                    int coins = int.Parse(Db_luta.GetValue(user.Id.ToString(), "COINS"));

                    DiscordEmbedBuilder inventario = new DiscordEmbedBuilder()     
                        .WithAuthor(user.Username, null, user.AvatarUrl)
                        .AddField("Propriedades:", $"\n:arrow_up: | Nível: **{lvl}** ({xp} / {500*((lvl + 1) * (lvl + 1))}) \n\n " +
                        $":heart: | Vida: **{hp}** / {maxhp} \n\n " +
                        $":crossed_swords: | Dano: **{dmg}** \n\n " +
                        $":shield: | Proteção: **{prtc}**\n")
                        .AddField("Itens:", $"\n:coin: |  Moedas: **{coins}** \n")
                        .WithColor(user.Color)
                        .WithFooter($"Jogador desde: {Db_luta.GetValue(user.Id.ToString(), "SINCE")}");

                    await ctx.RespondAsync(inventario.Build());
                }
            }
            catch (Exception e)
            {
                await ctx.RespondAsync(simples.EmbedComum($"ERRO: {e.Message}\n\n{e.StackTrace}\n\n{e.InnerException}", "ff0000"));
            }
        }

        [Command("get")]
        public async Task Get(CommandContext ctx, DiscordUser user, string field)
        {
            await ctx.TriggerTypingAsync();
            if (Db_luta.TemId(user.Id.ToString()))
            {
                try 
                {
                    Db_luta.UpdateData(user.Username, user.Id.ToString());
                    Db_luta.UpdateData(user.Username, user.Id.ToString());
                    Db_luta.UpdateData(user.Username, user.Id.ToString());
                    await ctx.RespondAsync(simples.EmbedComum($"O usuário `{user.Username}` tem o campo `{field.ToUpper()}` com o valor `{Db_luta.GetValue(user.Id.ToString(), field)}`"));
                }
                catch (Exception e)
                {
                    await ctx.RespondAsync(simples.EmbedComum($"ERRO : {e.Message}", "ff0000"));
                }
            }
            else
            {
                await ctx.RespondAsync(simples.EmbedComum($"Este usuário não foi encontrado em meu banco de dados!", "ff0000"));
            }
        }


        [Command("set")]
        public async Task Set(CommandContext ctx, DiscordUser user, string field, string value)
        {
            await ctx.TriggerTypingAsync();
            if (donos.Contains(ctx.Member.Id.ToString())) 
            {
                if (Db_luta.TemId(user.Id.ToString()))
                {
                    Db_luta.UpdateData(user.Username, user.Id.ToString());
                    Db_luta.UpdateValue(user.Id.ToString(), field, value);
                    Db_luta.UpdateData(user.Username, user.Id.ToString());
                    await ctx.RespondAsync(simples.EmbedComum($"O campo `{field.ToUpper()}` do usuário `{user.Username}` foi atualizado para `{value}`"));
                }
                else
                {
                    await ctx.RespondAsync(simples.EmbedComum($"Este usuário não foi encontrado em meu banco de dados!", "ff0000"));
                }
            }
            else
            {
                await ctx.RespondAsync(simples.EmbedComum($"Apenas meus donos podem usar este comando!", "ff0000"));
            }
        }


        [Command("summon")]
        [Aliases("sumonar")]
        public async Task Summon(CommandContext ctx, int lvl = 0)
        {
            DiscordMember king = await ctx.Client.GetGuildAsync(813625355842617385).Result.GetMemberAsync(348664615175192577);

            try
            {
                await ctx.TriggerTypingAsync();

                if (!Db_luta.TemId(ctx.Member.Id.ToString()))
                {
                    await ctx.RespondAsync(simples.EmbedComum("Você precisa de um save!!! Digite `c.registrar` para criar um novo save."));
                }
                else if (int.Parse(Db_luta.GetValue(ctx.Member.Id.ToString(), "HP")) <= 0)
                {
                    await ctx.RespondAsync(simples.EmbedComum("Você ta morto, você não pode sumonar nenhum monstro!", "ff0000"));
                }
                else
                {
                    List<string> blackList = new List<string>();
                    blackList.Add("813623531043815444");

                    Random ale = new Random();

                    Db_luta.UpdateData(ctx.User.Username, ctx.User.Id.ToString());

                    List<classes_luta.Player> players = new List<classes_luta.Player>();
                    players.Add(new classes_luta.Player(ctx.Member.Id));

                    if (lvl == 0) lvl = players[0].GetNivel() + ale.Next(-2, 2);

                    classes_luta.Inimigo inimigo = new classes_luta.Inimigo(lvl);

                    DiscordEmbedBuilder inimigoEmbed = new DiscordEmbedBuilder()
                        .WithDescription(linha)
                        .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                        .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                        .WithImageUrl(inimigo.GetImageUrl())
                        .WithAuthor(inimigo.GetName())
                        .WithFooter("HP: " + inimigo.GetLife(), "https://cdn.discordapp.com/attachments/816569715483738112/841874844895412244/heart-56-76703.png")
                        .WithColor(new DiscordColor("ffaafd"));

                    DiscordMessage mensagem = await ctx.RespondAsync(inimigoEmbed.Build());

                    DiscordEmoji espadas = DiscordEmoji.FromName(ctx.Client, ":crossed_swords:");
                    DiscordEmoji escudo = DiscordEmoji.FromName(ctx.Client, ":shield:");
                    DiscordEmoji correr = DiscordEmoji.FromName(ctx.Client, ":person_running:");

                    List<string> log = new List<string>();

                    bool inimigoAtaca = false;

                    while (inimigo.Status() && players.Count > 0)
                    {
                        await mensagem.CreateReactionAsync(espadas);
                        await mensagem.CreateReactionAsync(escudo);
                        await mensagem.CreateReactionAsync(correr);

                        var reacoes = await mensagem.CollectReactionsAsync(TimeSpan.FromSeconds(2));

                        List<classes_luta.Player> atacantes = new List<classes_luta.Player>();
                        List<classes_luta.Player> defensores = new List<classes_luta.Player>();
                        List<classes_luta.Player> fugitivos = new List<classes_luta.Player>();

                        List<classes_luta.Player> pl = new List<classes_luta.Player>();

                        foreach (classes_luta.Player p in players) pl.Add(p);

                        foreach (classes_luta.Player player in players)
                        {
                            if (player.GetLife() <= 0)
                            {
                                pl.Remove(player);
                                blackList.Add(player.GetId().ToString());
                            }
                        }
                        players.Clear(); foreach (classes_luta.Player p in pl) players.Add(p);

                        foreach (var reacao in reacoes)
                        {
                            foreach (var autor in reacao.Users)
                            {
                                string a = autor.Id.ToString();

                                if (Db_luta.TemId(a) && !blackList.Contains(a))
                                {
                                    if (!ContemPlayer(players, Convert.ToUInt64(a))) players.Add(new classes_luta.Player(Convert.ToUInt64(a)));

                                    if (reacao.Emoji == correr)
                                    {
                                        fugitivos.Add(GetPlayerById(players, Convert.ToUInt64(a)));
                                        blackList.Add(a);
                                        players.Remove(GetPlayerById(players, Convert.ToUInt64(a)));
                                        
                                    }

                                    else if (reacao.Emoji == espadas)
                                    {
                                        atacantes.Add(GetPlayerById(players, Convert.ToUInt64(a)));
                                    }

                                    else if (reacao.Emoji == escudo
                                        && !ContemPlayer(atacantes, Convert.ToUInt64(a)))
                                    {
                                        defensores.Add(GetPlayerById(players, Convert.ToUInt64(a)));
                                    }
                                }
                            }
                        }

                        if (fugitivos.Count > 0)
                        {
                            if (fugitivos.Count == 1) AddLog(log, $"🏃 `{fugitivos[0].GetName()}` ❤️ **{fugitivos[0].GetLife()}**", out log);
                            else AddLog(log, $"🏃 **{fugitivos.Count}** {invi} 👤 **{players.Count}** ", out log);
                        }

                        // ⚔️🛡️❤️👤🏃👹💀

                        int dano = 0;
                        foreach (classes_luta.Player atacante in atacantes)
                        {
                            if (Db_luta.TemId(atacante.GetId().ToString()))
                            {
                                int d = int.Parse(Db_luta.GetValue(atacante.GetId().ToString(), "DAMAGE"));
                                dano += d;
                            }
                        }

                        int danoR = 0;

                        inimigo.DarDano(dano, out danoR);

                        if (atacantes.Count > 1)
                        {
                            if (inimigo.Status()) AddLog(log, $"(👤 **{atacantes.Count}**) **{dano}** ⚔️ {invi} 👹 ❤️ **{inimigo.GetLife()}** (-**{danoR}**)", out log);
                            else AddLog(log, $"(👤 **{atacantes.Count}**) **{dano}** ⚔️ {invi} 👹 💀 **{inimigo.GetLife()}** (-**{danoR}**)", out log);
                        }

                        else if (atacantes.Count > 0) 
                        {
                            if (inimigo.Status()) AddLog(log, $"`{await GetUsernameAsync(ctx, atacantes[0].GetId())}` **{dano}** ⚔️ {invi} 👹 ❤️ **{inimigo.GetLife()}** (-**{danoR}**)", out log);
                            else AddLog(log, $"`{await GetUsernameAsync(ctx, atacantes[0].GetId())}` **{dano}** ⚔️ {invi} 👹 💀 **{inimigo.GetLife()}** (-**{danoR}**)", out log);
                        } 

                        if (inimigo.Status())
                        {
                            if (inimigoAtaca && players.Count > 0)
                            {

                                classes_luta.Player p = players[ale.Next(0, players.Count)];

                                int danoDado = inimigo.GetDano();
                                int danoRecebido = 0;

                                if (ContemPlayer(defensores, p.GetId()))
                                {
                                    int d = 0;

                                    if (inimigo.GetDano() > p.GetLife() + p.GetProtecao()) d = inimigo.GetDano() - (p.GetLife() + p.GetProtecao());

                                    p.DarDano(d, out d);

                                    if (p.Status())
                                    {
                                        AddLog(log, $"👹 **{danoDado}** ⚔️ {invi} 🛡 `{p.GetName()}` ❤️ **{p.GetLife()}** (-**{danoRecebido}**)", out log);
                                    }
                                    else
                                    {
                                        AddLog(log, $"👹 **{danoDado}** ⚔️ {invi} 🛡 `{p.GetName()}` 💀 **{p.GetLife()}** (-**{danoRecebido}**)", out log);
                                    }
                                }
                                else
                                {
                                    p.DarDano(danoDado, out danoRecebido);
                                    if (p.Status())
                                    {
                                        AddLog(log, $"👹 **{danoDado}** ⚔️ {invi} `{p.GetName()}` ❤️ **{p.GetLife()}** (-**{danoRecebido}**)", out log);
                                    }
                                    else
                                    {
                                        AddLog(log, $"👹 **{danoDado}** ⚔️ {invi} `{p.GetName()}` 💀 **{p.GetLife()}** (-**{danoRecebido}**)", out log);
                                    }
                                }       // ⚔️🛡️❤️👤🏃👹💀
                            }
                        }

                        DiscordEmbedBuilder inimigoResultado = new DiscordEmbedBuilder()
                            .WithDescription($"{linha}\n{GetLog(log)}")
                            .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                            .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                            .WithImageUrl(inimigo.GetImageUrl())
                            .WithAuthor(inimigo.GetName())
                            .WithFooter("HP: " + inimigo.GetLife() + $"  (- {dano})", "https://cdn.discordapp.com/attachments/816569715483738112/841874844895412244/heart-56-76703.png")
                            .WithColor(new DiscordColor("ffaafd"));

                        DiscordEmbedBuilder inimigoMorto = new DiscordEmbedBuilder()
                            .WithDescription(":linhaesquerda::linhameio::linhameio::linhameio::linhadireita:\n" + GetLog(log))
                            .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                            .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                            .WithImageUrl("https://cdn.discordapp.com/attachments/841867549427499009/850076189065478144/tombstone.png")
                            .WithAuthor(inimigo.GetName())
                            .WithFooter("Dead " + $" (- {dano})", "https://cdn.discordapp.com/attachments/816569715483738112/841888256282591242/skull-147-450412.png")
                            .WithColor(new DiscordColor("ffaafd"));

                        if (!inimigo.Status()) await mensagem.ModifyAsync(inimigoMorto.Build());
                        else await mensagem.ModifyAsync(inimigoResultado.Build());
                        inimigoAtaca = true;

                        foreach (classes_luta.Player p in players) Db_luta.UpdateData(p);

                        await mensagem.DeleteAllReactionsAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await ctx.RespondAsync(simples.EmbedComum($"ERRO: {e.Message}\n\n{e.StackTrace}\n\n{e.InnerException}", "ff0000"));
            }
        }


        public static bool ContemPlayer(List<classes_luta.Player> players, ulong id)
        {
            foreach (classes_luta.Player p in players)
            {
                if (p.GetId() == id) return true;
            }
            return false;
        }


        public static classes_luta.Player GetPlayerById(List<classes_luta.Player> players, ulong id)
        {
            Exception e = new ArgumentException("No player Found in GetPlayerId()"); 

            foreach (classes_luta.Player p in players)
            {
                if (p.GetId() == id) return p;
            }

            throw e;
        }


        public static string GetLog(List<string> log)
        {
            string res = "";

            foreach (string l in log)
            {
                res += l + "\n" +
                    "<:linhameio:850650956806619206><:linhameio:850650956806619206><:linhameio:850650956806619206>" +
                    "<:linhameio:850650956806619206><:linhameio:850650956806619206><:linhadireita:850650956759826443>\n";
            }
            return res;
        }


        public static async Task<string> GetUsernameAsync(CommandContext ctx, ulong id)
        {
            DiscordUser user = await ctx.Client.GetUserAsync(id);

            return user.Username;
        }


        public static void AddLog(List<string> lista, string item, out List<string> res)
        {
            if (lista.Count >= 5)
            {
                lista.RemoveAt(0);
                lista.Add(item);
            }
            else 
            {
                lista.Add(item); 
            }

            res = lista;
        }
    }
}
