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

namespace CarmesinaConfig.comandos
{
    class diversao : BaseCommandModule
    {

        [Command("mostgay")]
        [Aliases("gayest")]
        [Description("Tell who is the most gay in the chat")]
        public async Task MostGay(CommandContext ctx, string arg = "20")
        {
            await ctx.TriggerTypingAsync();
            try { int.Parse(arg); }
            catch { arg = "20"; }
            int qnt = int.Parse(arg);
            if (qnt > 30) qnt = 30;
            var msgs = await ctx.Channel.GetMessagesBeforeAsync(ctx.Message.Id, qnt);
            Random random = new Random();
            int val = random.Next(qnt - 1);
            var sorteado = msgs[val].Author;
            await ctx.RespondAsync(null, false, simples.EmbedComum($"**Hmm... I can see {sorteado.Mention} is the gayest of the chat right now.**"));
        }

        [Command("pp")]
        [Aliases("ppsize", "penissize", "size", "penis")]
        [Description("Show's a user pp size")]
        public async Task ppsize(CommandContext ctx, DiscordUser user = null)
        {
            if (user == null) user = ctx.User;
            Random random = new Random();
            int val = random.Next(15);
            var tamanho = new StringBuilder();
            for (int c = 0; c <= val; c++) tamanho.Append("=");
            var embed = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor("ffaafd"))
                    .WithTitle($"**{user.Username}'s pp size:**")
                    .WithDescription($"8{tamanho}D")
                    .WithFooter($"{val*2} cm");
            await ctx.RespondAsync(null, false, embed.Build());
        }

        [Command("dice")]
        [Aliases("die", "random")]
        [Description("Rolls a dice with custom ammount of faces")]
        public async Task dice(CommandContext ctx, int faces = 6)
        {
            int face = new Random().Next(faces + 1);
            await ctx.RespondAsync(null, false, simples.EmbedComum($"You rolled {faces} and got: `{face}`"));
        }

        [Command("draw")]
        [Aliases("draft")]
        [Description("Draw a user from the chat")]
        public async Task draw(CommandContext ctx)
        {
            var messages = await ctx.Channel.GetMessagesBeforeAsync(ctx.Message.Id);
            var escolhido = messages[new Random().Next(messages.Count)].Author;
            await ctx.RespondAsync("Hmm... I gonna choose...");
            Thread.Sleep(3000);
            await ctx.RespondAsync($"{escolhido.Mention}!!!");
        }

        [Command("syllabes")]
        [Aliases("split", "word")]
        [Description("Split the word in syllabes")]
        public async Task syllabes(CommandContext ctx, string ins)
        {
            string vogais = "";
            string consoantes = "";
            int silabas = 1;
            string contraria = "";
            string japostas = "";

            ins = ins.Split()[0];
            DivisionSilabica_CSharp.Divisor divisor = new DivisionSilabica_CSharp.Divisor(ins);
            divisor.getString();

            foreach (char l in ins)
            {
                if ("aeiou".Contains(l.ToString().ToLower()) && !japostas.Contains(l.ToString().ToLower()))
                {
                    japostas += l.ToString();
                    vogais += l.ToString() + " ";
                }

                if ("bcdfghjklmnpqrstvwxyz".Contains(l.ToString().ToLower()) && !japostas.Contains(l.ToString().ToLower()))
                {
                    japostas += l.ToString();
                    consoantes += l.ToString() + " ";
                }
            }

            string separado = divisor.silabear();

            foreach (char l in separado) if (l == '-')  silabas += 1;

            for (int i = ins.Length - 1; i >= 0; i--) contraria += ins[i];

            var embed = new DiscordEmbedBuilder()
                .WithTitle($"📚 {simples.Capitalize(ins)}")
                .AddField($"✂️ Splited ({silabas})", separado)
                .AddField("🅰 Vowels", vogais)
                .AddField("🅱 Consonants", consoantes)
                .AddField("🔁 Reverse", contraria)
                .WithColor(new DiscordColor("ffaafd"))
                .WithUrl($"https://www.dicio.com.br/" + ins + "/")
                .WithFooter($"Letters: {ins.Length}");
            await ctx.RespondAsync(null, false, embed.Build());
        }
    }
}
