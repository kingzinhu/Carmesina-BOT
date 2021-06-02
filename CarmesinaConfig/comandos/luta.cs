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
using DSharpPlus;

namespace CarmesinaConfig.comandos
{
    class luta : BaseCommandModule
    {
        [Command("registrar")]
        public async Task Registrar(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            if (Db_luta.TemId(ctx.Member.Id.ToString())) await ctx.RespondAsync(simples.EmbedComum("Você já tem um save!"));
            else
            {
                Db_luta.AddData(ctx.Member.Id.ToString(), ctx.Member.Username);
                await ctx.RespondAsync(simples.EmbedComum("Save criado!"));
            }
        }

        [Command("summon")]
        [Aliases("sumonar")]
        public async Task Summon(CommandContext ctx, string nome = null)
        {
            await ctx.TriggerTypingAsync();

            if (nome == null) nome = "unknown";

            if (!Db_luta.TemId(ctx.Member.Id.ToString())) { 
                await ctx.RespondAsync(simples.EmbedComum("Você precisa de um save!!! Ditie `c.registrar` para criar um novo save."));
            }
            else
            {
                Random ale = new Random();

                Inimigo inimigo = new Inimigo(nome, ale.Next(1, 15));

                Player player = new Player(ctx.Member.Username,
                int.Parse(Db_luta.GetValue(ctx.Member.Id.ToString(), "LEVEL")),
                int.Parse(Db_luta.GetValue(ctx.Member.Id.ToString(), "COINS")),
                int.Parse(Db_luta.GetValue(ctx.Member.Id.ToString(), "XP")));

                while (player.Status() && inimigo.Status())
                {
                    DiscordEmbedBuilder inimigoEmbed = new DiscordEmbedBuilder()
                        .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                        .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                        .WithImageUrl(inimigo.GetImageUrl())
                        .WithAuthor(inimigo.GetName())
                        .WithFooter("HP: " + inimigo.GetLife(), "https://cdn.discordapp.com/attachments/816569715483738112/841874844895412244/heart-56-76703.png")
                        .WithColor(new DiscordColor("ffaafd"));

                    DiscordMessage log = await ctx.RespondAsync($"> ```• [Um(a) {inimigo.GetName()} selvagem apareceu!!!]```");
                    DiscordMessage mensagem = await ctx.RespondAsync(inimigoEmbed.Build());

                    DiscordEmoji espadas = DiscordEmoji.FromName(ctx.Client, ":crossed_swords:");
                    DiscordEmoji escudo = DiscordEmoji.FromName(ctx.Client, ":shield:");

                batalha:

                    await mensagem.CreateReactionAsync(espadas);

                    var reacoes = await mensagem.CollectReactionsAsync(TimeSpan.FromSeconds(5));
                    List<string> atacantes = new List<string>();
                    List<string> defensores = new List<string>();
                    foreach (var reacao in reacoes)
                    {
                        foreach (var autor in reacao.Users)
                        {
                            if(autor.Id != 813623531043815444)
                            {
                                if (reacao.Emoji == espadas) atacantes.Add(autor.Id.ToString());
                                if (reacao.Emoji == escudo) defensores.Add(autor.Id.ToString());
                            }
                        }
                    }

                    int dano = 0;
                    foreach (string atacante in atacantes)
                    {
                        if (Db_luta.TemId(atacante))
                        {
                            int d = int.Parse(Db_luta.GetValue(atacante, "DAMAGE"));
                            dano += d;
                            log = await AddLogAsync(log, $"{ctx.Client.GetUserAsync(Convert.ToUInt64(atacante)).Result.Username} ataca {inimigo.GetName()} com {d} de força");
                        }
                    }

                    inimigo.DarDano(dano, out dano);

                    DiscordEmbedBuilder inimigoResultado = new DiscordEmbedBuilder()
                        .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                        .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                        .WithImageUrl(inimigo.GetImageUrl())
                        .WithAuthor(inimigo.GetName())
                        .WithFooter("HP: " + inimigo.GetLife() + $"  (- {dano})", "https://cdn.discordapp.com/attachments/816569715483738112/841874844895412244/heart-56-76703.png")
                        .WithColor(new DiscordColor("ffaafd"));

                    DiscordEmbedBuilder inimigoMorto = new DiscordEmbedBuilder()
                        .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                        .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                        .WithImageUrl("https://cdn.discordapp.com/attachments/841867549427499009/841887792044179456/coffin.png")
                        .WithAuthor(inimigo.GetName())
                        .WithFooter("Dead " + $" (- {dano})", "https://cdn.discordapp.com/attachments/816569715483738112/841888256282591242/skull-147-450412.png")
                        .WithColor(new DiscordColor("ffaafd"));

                    await mensagem.DeleteAllReactionsAsync();

                    if (inimigo.Status())
                    {
                        await mensagem.ModifyAsync(inimigoResultado.Build());

                        goto batalha;
                    }
                    else await mensagem.ModifyAsync(inimigoMorto.Build());
                }
            }
        }
        public static async void Atacando(Inimigo i, Player p, DiscordMessage log)
        {
            while (i.Status() && p.Status())
            {
                int dano = i.GetDano();
                await Task.Delay(3000);
                p.DarDano(dano, out dano);
                await log.ModifyAsync($"{i.GetName()} ataca, dando {dano} de dano em {p.GetName()}");
            }
        }
        public static async Task<DiscordMessage> AddLogAsync(DiscordMessage log, string ato)
        {
            string cont = log.Content;
            if (simples.Count(cont, "]") >= 5)
            {
                cont = cont.Substring(0, cont.LastIndexOf("["));
            } 
            string res = $"> ```• [{ato}]\n> {cont.Substring(cont.IndexOf("["), cont.LastIndexOf("]") - 6)}```";
            var msg = await log.ModifyAsync(res);
            return msg;
        }
    }
    public class Player : Program
    {
        Random ale = new Random();

        private int Nivel;
        private string Nome;
        private int Vida;
        private bool Vivo = true;
        private int Protecao;
        private int Dano;
        private int Moedas;
        private int Xp;
        private string Items;

        public Player(string nome, int nivel, int moedas, int xp, string items = "")
        {
            if (nivel <= 1) this.Nivel = 1;
            else this.Nivel = nivel;

            this.Vida = 10 * this.Nivel;
            this.Items = items;
            this.Xp = xp;
            this.Moedas = moedas;
            this.Nome = nome;
        }
        public void DarDano(int dano)
        {
            int resultado = dano - this.Protecao;

            if (resultado < 0) resultado = 0;

            this.Vida -= resultado;

            if (Vida < 0) Vida = 0;
        }
        public void DarDano(int dano, out int res)
        {
            int resultado = dano - this.Protecao;

            if (resultado < 0) resultado = 0;

            res = resultado;
            this.Vida -= resultado;

            if (Vida < 0) Vida = 0;
        }
        public void DarMoedas(int val)
        {
            this.Moedas += val;
            if (this.Moedas >= 0) this.Moedas = 0;
        }
        public void ReduzirVida(int val)
        {
            Vida -= val;

            if (Vida < 0) Vida = 0;
        }
        public bool Status()
        {
            if (this.Vida <= 0)
            {
                this.Vivo = false;
                return false;
            }
            else return true;
        }
        public int GetMoedas() { return this.Moedas; }
        public int GetDano() {  return this.Dano; }
        public int GetXp() { return this.Xp; }
        public int GetProtecao() { return this.Protecao; }
        public string GetName() { return Nome; }
        public int GetLife() { return Vida; }
        public int GetNivel() { return Nivel; }
    }

    public class Inimigo : Program
    {
        Random ale = new Random();

        string[] inimigos = { "https://cdn.discordapp.com/attachments/841867549427499009/841876876057903104/0c087012bc7afce9.png",
            "https://cdn.discordapp.com/attachments/841867549427499009/841877066224107580/oXiYah.jpg",
            "https://cdn.discordapp.com/attachments/841867549427499009/841877202714361866/WhatsApp_Image_2021-05-02_at_8.42.36_PM_3.jpeg",
            "https://cdn.discordapp.com/attachments/841867549427499009/841877468570451968/image.png",};

        private int Nivel;
        private string Nome;
        private int Vida;
        private bool Vivo = true;
        private int Protecao;
        private int Dano;
        private string ImagemURL;
        


        public Inimigo(string nome = "Unknown", int nivel = 0, int vida = 0, int protecao = 0, int dano = 0)
        {
            if (nivel <= 1) this.Nivel = 1;
            else this.Nivel = nivel;

            if (vida == 0) this.Vida = (10 * this.Nivel) - ale.Next(5);
            else this.Vida = vida;

            if (protecao == 0) this.Protecao = ale.Next(12);
            else this.Protecao = protecao;

            if (dano == 0) this.Dano = 5 * this.Nivel;
            else this.Dano = dano;

            this.Nome = nome;
            this.ImagemURL = inimigos[ale.Next(inimigos.Length)];
        }

        public void DarDano(int dano)
        {
            int resultado = dano - this.Protecao;

            if (resultado < 0) resultado = 0;

            this.Vida -= resultado;

            if (Vida < 0) Vida = 0;
        }
        public void DarDano(int dano, out int res)
        {
            int resultado = dano - this.Protecao;

            if (resultado < 0) resultado = 0;

            res = resultado;
            this.Vida -= resultado;

            if (Vida < 0) Vida = 0;
        }

        public void ReduzirVida(int val)
        {
            Vida -= val;

            if (Vida < 0) Vida = 0;
        }

        public bool Status()
        {
            if (this.Vida <= 0)
            {
                this.Vivo = false;
                return false;
            }
            else return true;
        }
        public int GetDano() { return this.Dano; }
        public string GetImageUrl() { return ImagemURL; }

        public string GetName() { return Nome; }

        public int GetLife() { return Vida; }

        public int GetProtecao() { return Protecao; }

        public int GetNivel() { return Nivel; }
    }
}
