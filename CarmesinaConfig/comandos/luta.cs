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
using DSharpPlus;

namespace CarmesinaConfig.comandos
{
    public static class variaveis
    {
        public static List<string> ini = new List<string>();
    }

    class luta : BaseCommandModule
    {
        [Command("summon")]
        [Aliases("sumonar")]
        public async Task Summon(CommandContext ctx, string nome = null)
        {
            var mensagens = await ctx.Client.GetChannelAsync(841867549427499009).Result.GetMessagesAsync(100);

            Random ale = new Random();

            foreach (var m in mensagens)
            {
                variaveis.ini.Add(m.Attachments[0].Url);
            }

            await ctx.TriggerTypingAsync();

            if (nome == null) nome = "unknown";

            Inimigo inimigo = new Inimigo(nome, ale.Next(1, 15));

            DiscordEmbedBuilder inimigoEmbed = new DiscordEmbedBuilder()
                .AddField(":small_orange_diamond: Protection:", $":shield: | **{inimigo.GetProtecao()}**", true)
                .AddField(":small_orange_diamond: Level:", $":arrow_up: | **{inimigo.GetNivel()}**", true)
                .WithImageUrl(inimigo.GetImageUrl())
                .WithAuthor(inimigo.GetName())
                .WithFooter("HP: " + inimigo.GetLife(), "https://cdn.discordapp.com/attachments/816569715483738112/841874844895412244/heart-56-76703.png")
                .WithColor(new DiscordColor("ffaafd"));

            DiscordMessage mensagem = await ctx.RespondAsync(inimigoEmbed.Build());

            DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":crossed_swords:");

            batalha:

            await mensagem.CreateReactionAsync(emoji);

            var reacao = await mensagem.WaitForReactionAsync(ctx.Member, TimeSpan.FromSeconds(8f));

            if (!reacao.TimedOut)
            {
                if (reacao.Result.Emoji == emoji)
                {
                    int dano = 10 + ale.Next(10);

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

                    if (inimigo.Status()) { await mensagem.ModifyAsync(inimigoResultado.Build()); goto batalha; }
                    else await mensagem.ModifyAsync(inimigoMorto.Build());
                }
            }
        }

    }

    public class Player : Program
    {
        Random ale = new Random();

        private int Nivel;
        private string Nome;
        private int Vida;
        private bool Vivo;
        private int Protecao;
        private int Moedas;
        private int Xp;

        public Player(string nome = "Unknown", int nivel = 1, bool vivo = true)
        {
            if (nivel >= 1) this.Nivel = 1;
            else this.Nivel = nivel;

            this.Nome = nome;
            this.Vivo = vivo;
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
        private bool Vivo;
        private int Protecao;
        private string ImagemURL;
        


        public Inimigo(string nome = "Unknown", int nivel = 1, int vida = 0, int protecao = 0, bool vivo = true)
        {
            if (nivel == 1) this.Nivel = 1;
            else this.Nivel = nivel;

            if (vida == 0) this.Vida = (10 * this.Nivel) - ale.Next(5);
            else this.Vida = vida;

            if (protecao == 0) this.Protecao = ale.Next(12);
            else this.Protecao = protecao;

            this.Nome = nome;
            this.Vivo = vivo;
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

        public string GetImageUrl()
        {
            return ImagemURL;
        }

        public string GetName()
        {
            return Nome;
        }

        public int GetLife()
        {
            return Vida;
        }

        public int GetProtecao()
        {
            return Protecao;
        }

        public int GetNivel()
        {
            return Nivel;
        }
    }
}
