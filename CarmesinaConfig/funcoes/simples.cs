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

namespace CarmesinaConfig.funcoes
{
    class simples
    {
        public static DiscordEmbed EmbedComum(string texto, string cor = null)
        {
            if (cor == null) { cor = "ffaafd"; }
            var builder = new DiscordEmbedBuilder()
                .WithDescription(texto)
                .WithColor(new DiscordColor(cor));
            var embed = builder.Build();
            return embed;
        }

        public static bool ContemUlong(ulong item, ulong[] local)
        {
            foreach(ulong coisa in local)
            {
                if (coisa == item) return true;
            }
            return false;
        }
    }
}
