using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CarmesinaConfig.comandos;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace CarmesinaConfig
{
    public class Program
    {
        private DiscordClient _client;

        static void Main(string[] args)
        {
            new Program().RodarBotAsync().GetAwaiter().GetResult(); 
            
        }
        

        public async Task RodarBotAsync()
        {
            DiscordConfiguration cfg = new DiscordConfiguration
            {
                Token = Environment.GetEnvironmentVariable("CARMESINA_TOKEN"),
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };
            _client = new DiscordClient(cfg);
            _client.Ready += Client_Ready;
            _client.ClientErrored += Client_ClientError;

            string[] prefix = new string[5];
            prefix[0] = "c.";

            CommandsNextExtension cnt = _client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = prefix,
                EnableDms = true,
                CaseSensitive = false,
                EnableDefaultHelp = true,
                EnableMentionPrefix = false,
                IgnoreExtraArguments = true,

            });
            cnt.CommandExecuted += Cnt_CommandExecuted;

            cnt.RegisterCommands<basicos>();

            await _client.ConnectAsync();
            await Task.Delay(-1);

        }

        private Task Client_Ready(DiscordClient client, ReadyEventArgs e)
        {
            return Task.Run(async () =>
            {
                DiscordChannel canal_status = await client.GetChannelAsync(814009178393542678);
                await client.SendMessageAsync(canal_status, "> <:online:816411487794102293> Online, rs");
                await client.UpdateStatusAsync(new DiscordActivity("c.help", ActivityType.Playing), UserStatus.Online);
            }); 
        }

        private Task Client_ClientError(DiscordClient client, ClientErrorEventArgs e)
        {
            return Task.Run(async () =>
            {
                DiscordChannel canal_errors = await client.GetChannelAsync(816382897237393408);
                await client.SendMessageAsync(canal_errors, $"> <:alert:816396654727004231> Um erro aconteceu em:\n" + $"```{e.Exception.GetType()}``` ```{e.Exception.Message}```");
            });
        }

        private Task Cnt_CommandExecuted(CommandsNextExtension commands, CommandExecutionEventArgs e)
        {
            return Task.Run(async () =>
            {
               DiscordChannel canal_executedcommands = await _client.GetChannelAsync(816391639681597469);
               await _client.SendMessageAsync(canal_executedcommands, $"> <:8bitplus:816411488105005105> Used command: `{e.Command.Name}`" +
                   $"\n> Execution author: `{e.Context.Member.DisplayName}`" +
                   $"\n> Guild: `{e.Context.Guild.Name}`" +
                   $"\n> Channel: `{e.Context.Channel.Name}`" +
                   $"\n```{e.Context.Message.Content}```");
            });
        }
    }
}
