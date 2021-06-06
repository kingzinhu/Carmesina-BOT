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
        public static int Count(string texto, string valor)
        {
            int c = 0;
            foreach (char l in texto)
            {
                if (l.ToString() == valor) c++;
            }
            return c;
        }
        public static DiscordEmbed EmbedComum(string texto, string cor = null)
        {
            if (cor == null) { cor = "ffaafd"; }
            var builder = new DiscordEmbedBuilder()
                .WithDescription(texto).WithColor(new DiscordColor(cor));
                
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

        public static string PrimeiraInt(string texto)
        {
            string numeros = "0123456789";
            string resultado = "";
            bool achou_int = false;

            foreach (char letra in texto)
            {
                if (numeros.Contains(letra))
                {
                    resultado += letra;
                    achou_int = true;
                }
                else if (achou_int) break;
            }

            return resultado;
        }

        public static string PegarLetras(string texto)
        {
            string letras = "abcdefghijklmnopqrstuvwxyz 0123456789'áéíóúãõâêîôûàèìòùäëïöü";
            string resultado = "";

            foreach (char letra in texto)
            {
                if (letras.Contains(letra.ToString().ToLower()))
                {
                    resultado += letra;
                }
            }
            return resultado;
        }

        public static string TirarEspacos(string texto)
        {
            string resultado = "";
            bool removido_primeiros = false;

            foreach (char letra in texto)
            {
                if (removido_primeiros) resultado += letra;
                else
                {
                    if (letra != ' ')
                    {
                        removido_primeiros = true;
                        resultado += letra;
                    }
                }
            }

            return resultado;
        }

        public static string FormatarAcentosHtml(string texto)
        {
            texto = texto.Replace("&Aacute;", "Á");
            texto = texto.Replace("&aacute;", "á");
            texto = texto.Replace("&Acirc;", "Â");
            texto = texto.Replace("&acirc;", "â");
            texto = texto.Replace("&Agrave;", "À");
            texto = texto.Replace("&agrave;", "à");
            texto = texto.Replace("&Aring;", "Å");
            texto = texto.Replace("&aring;", "å");
            texto = texto.Replace("&Atilde;", "Ã");
            texto = texto.Replace("&atilde;", "ã");
            texto = texto.Replace("&Auml;", "Ä");
            texto = texto.Replace("&auml;", "ä");
            texto = texto.Replace("&Eacute;", "É");
            texto = texto.Replace("&eacute;", "é");
            texto = texto.Replace("&Ecirc;", "Ê");
            texto = texto.Replace("&ecirc;", "ê");
            texto = texto.Replace("&Egrave;", "È");
            texto = texto.Replace("&egrave;", "è");
            texto = texto.Replace("&Euml;", "Ë");
            texto = texto.Replace("&euml;", "ë");
            texto = texto.Replace("&Iacute;", "Í");
            texto = texto.Replace("&iacute;", "í");
            texto = texto.Replace("&Icirc;", "Î");
            texto = texto.Replace("&icirc;", "î");
            texto = texto.Replace("&Igrave;", "Ì");
            texto = texto.Replace("&igrave;", "ì");
            texto = texto.Replace("&Iuml;", "Ï");
            texto = texto.Replace("&iuml;", "ï");
            texto = texto.Replace("&Oacute;", "Ó");
            texto = texto.Replace("&oacute;", "ó");
            texto = texto.Replace("&Ocirc;", "Ô");
            texto = texto.Replace("&ocirc;", "ô");
            texto = texto.Replace("&Ograve;", "Ò");
            texto = texto.Replace("&ograve;", "ò");
            texto = texto.Replace("&Oslash;", "Ø");
            texto = texto.Replace("&oslash;", "ø");
            texto = texto.Replace("&Otilde;", "Õ");
            texto = texto.Replace("&otilde;", "õ");
            texto = texto.Replace("&Ouml;", "Ö");
            texto = texto.Replace("&ouml;", "ö");
            texto = texto.Replace("&Uacute;", "Ú");
            texto = texto.Replace("&uacute;", "ú");
            texto = texto.Replace("&Ucirc;", "Û");
            texto = texto.Replace("&ucirc;", "û");
            texto = texto.Replace("&Ugrave;", "Ù");
            texto = texto.Replace("&ugrave;", "ù");
            texto = texto.Replace("&Uuml;", "Ü");
            texto = texto.Replace("&uuml;", "ü");
            texto = texto.Replace("&Ccedil;", "Ç");
            texto = texto.Replace("&ccedil;", "ç");
            texto = texto.Replace("&Ntilde;", "Ñ");
            texto = texto.Replace("&ntilde;", "ñ");
            texto = texto.Replace("&lt;", "<");
            texto = texto.Replace("&gt;", ">");
            texto = texto.Replace("&amp;", "&");
            texto = texto.Replace("&quot;", "\"");
            texto = texto.Replace("&reg;", "®");
            texto = texto.Replace("&copy;", "©");
            texto = texto.Replace("&Yacute;", "Ý");
            texto = texto.Replace("&yacute;", "ý");
            texto = texto.Replace("&nbsp;", " ");
            texto = texto.Replace("&deg;", "°");

            return texto;
        }
    }
}
