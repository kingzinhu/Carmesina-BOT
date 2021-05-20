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

        public static string Capitalize(string texto)
        {
            string l = texto.Substring(0, 1).ToUpper();
            return l + texto.Substring(1);
        }

        public static string Join(string texto, string item)
        {
            string resultado = "";
            int pos = 0;

            foreach (char letra in texto)
            {
                if (letra != ' ') resultado += letra;
                else if ((pos > 0) && (resultado[ pos - 1].ToString() != item)) resultado += item;
                pos++;
            }

            return resultado;
        }
    }
}
