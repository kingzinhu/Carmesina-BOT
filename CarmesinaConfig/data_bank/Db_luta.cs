using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace CarmesinaConfig.data_bank
{
    class Db_luta
    {
        private static SQLiteConnection cnt;

        private static SQLiteConnection ConnectDataBase()
        {
            cnt = new SQLiteConnection(@"Data Source=C:\Users\King\Documents\Repositorios\CarmesinaBOT\Config\CarmesinaConfig\data_bank\db_luta.db");
            cnt.Open();
            return cnt;
        }

        private static DataTable SelectDataTable(string c)
        {
            var cmd = ConnectDataBase().CreateCommand();

            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();

            cmd.CommandText = c;

            da = new SQLiteDataAdapter(cmd.CommandText, ConnectDataBase());
            da.Fill(dt);
            ConnectDataBase().Close();
            return dt;
        }

        public static string GetValue(string id, string key)
        {
            var dt = SelectDataTable($"SELECT * FROM users_data WHERE USER_ID='{id}'");
            return dt.Rows[0].Field<string>(key);
        }

        public static void ExecutarComando(string comando)
        {
            var cmd = ConnectDataBase().CreateCommand();
            cmd.CommandText = comando;
            cmd.ExecuteNonQuery();
        }

        public static bool TemId(string id)
        {
            var dt = SelectDataTable($"SELECT * FROM users_data WHERE USER_ID='{id}'");
            if (dt.Rows.Count == 0) return false;
            else return true;
        }

        public static DataTable GetData()
        {
            var dt = SelectDataTable("SELECT * FROM users_data");
            return dt;
        }

        public static DataTable Consult(string sql)
        {
            var dt = SelectDataTable(sql);
            return dt;
        }

        public static void AddData(string id, string nome)
        {
            if (!TemId(id))
            {
                ExecutarComando($"INSERT INTO users_data (USER_NAME, USER_ID, LEVEL, HP, DAMAGE, COINS, PROTECTION, XP) VALUES ('{nome}', '{id}', '1', '50', '10', '0', '0', '0')");
            }
        }
    }
}
