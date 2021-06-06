using System;
using System.Collections.Generic;
using System.Text;
using CarmesinaConfig.data_bank;

namespace CarmesinaConfig.classes
{
    class classes_luta
    {
        public class Player : Program
        {
            Random ale = new Random();

            private ulong Id;
            private int Nivel;
            private string Nome;
            private int Vida;
            private int Protecao;
            private int Dano;
            private int Moedas;
            private int Xp;

            public Player(ulong id)
            {
                this.Id = Convert.ToUInt64(Db_luta.GetValue(id.ToString(), "USER_ID"));
                this.Nivel = int.Parse(Db_luta.GetValue(id.ToString(), "LEVEL"));
                this.Vida = int.Parse(Db_luta.GetValue(id.ToString(), "HP"));
                this.Xp = int.Parse(Db_luta.GetValue(id.ToString(), "XP"));
                this.Moedas = int.Parse(Db_luta.GetValue(id.ToString(), "COINS"));
                this.Nome = Db_luta.GetValue(id.ToString(), "USER_NAME");
                this.Protecao = int.Parse(Db_luta.GetValue(id.ToString(), "PROTECTION"));
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
                    return false;
                }
                else return true;
            }
            public int GetMoedas() { return this.Moedas; }
            public int GetDano() { return this.Dano; }
            public int GetXp() { return this.Xp; }
            public int GetProtecao() { return this.Protecao; }
            public string GetName() { return Nome; }
            public int GetLife() { return Vida; }
            public int GetNivel() { return Nivel; }
            public ulong GetId() { return Id; }
        }


        public class Inimigo : Program
        {
            Random ale = new Random();

            string[] inimigos = { "Skeleton=https://cdn.discordapp.com/attachments/841867549427499009/850040450215051334/latest.png",
            "Whiter_skeleton=https://cdn.discordapp.com/attachments/841867549427499009/850040499938787368/latest.png",
            "Slime=https://cdn.discordapp.com/attachments/841867549427499009/850040814813708348/3e82fa3819b2034621b3c29e20f49deb.png",
            "Zombie=https://cdn.discordapp.com/attachments/841867549427499009/850040908052693012/latest.png",
            "Spider=https://cdn.discordapp.com/attachments/841867549427499009/850040962822701076/spider.png",
            "Enderman=https://cdn.discordapp.com/attachments/841867549427499009/850041076353204234/f9d0402d9e293e6c67a02d358ba1a5b7.png"};

            private int Nivel;
            private string Nome;
            private int Vida;
            private bool Vivo = true;
            private int Protecao;
            private int Dano;
            private string ImagemURL;


            public Inimigo(int nivel = 0, int vida = 0, int protecao = 0, int dano = 0)
            {
                string i = inimigos[ale.Next(inimigos.Length)];
                string iname = i.Substring(0, i.IndexOf("="));
                string iimage = i.Substring(i.IndexOf("=") + 1, i.Length - i.Substring(0, i.IndexOf("=")).Length - 1);

                if (nivel <= 1) this.Nivel = 1;
                else this.Nivel = nivel;

                if (vida == 0) this.Vida = (10 * this.Nivel) - ale.Next(5);
                else this.Vida = vida;

                if (protecao == 0) this.Protecao = ale.Next(this.Nivel);
                else this.Protecao = protecao;

                if (dano == 0) this.Dano = 4 * this.Nivel;
                else this.Dano = dano;

                this.Nome = iname;
                this.ImagemURL = iimage;
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
}
