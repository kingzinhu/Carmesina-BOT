using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CarmesinaConfig.comandos;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.Enums;

namespace CarmesinaConfig
{
    public class Program
    {
        public DiscordClient _client;
 
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

            InteractivityExtension icf = _client.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(10)
            });


            cnt.CommandExecuted += Cnt_CommandExecuted;

            cnt.RegisterCommands<basicos>();
            cnt.RegisterCommands<Exclusivos>();
            cnt.RegisterCommands<diversao>();

            await _client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task Client_Ready(DiscordClient client, ReadyEventArgs e)
        {
            return Task.Run(async () =>
            {
                DiscordChannel canal_status = await client.GetChannelAsync(814009178393542678);
                var build = new DiscordEmbedBuilder()
                    .WithDescription("<:online:816411487794102293> I'm online!")
                    .WithColor(new DiscordColor("3cdb35"));
                var embed = build.Build();
                await client.SendMessageAsync(canal_status, embed);
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
                string args = e.Context.Message.Content.Substring(e.Context.Message.Content.Split()[0].Length);
                if (args == "") args = " ";
                var build = new DiscordEmbedBuilder()
                    .WithTitle("<:8bitplus:816411488105005105> **Command used**")
                    .AddField("Command", $"`{e.Command.Name}`")
                    .AddField("Author", $"`{e.Context.Member.Username}`")
                    .AddField("Guild", $"`{e.Context.Guild.Name}`")
                    .AddField("Channel", $"`{e.Context.Channel.Name}`")
                    .AddField("Arguments", $"```{args.Replace('`', '©')}```")
                    .WithFooter(e.Context.User.Username + $" ({e.Context.User.Id})", e.Context.User.AvatarUrl)
                    .WithColor(new DiscordColor("324f79"));
                DiscordChannel canal_executedcommands = await _client.GetChannelAsync(816391639681597469);
                var embed = build.Build();
                await _client.SendMessageAsync(canal_executedcommands, embed);
            });
        }
    }
}
