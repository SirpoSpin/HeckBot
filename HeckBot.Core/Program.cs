using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HeckBot.Core.Modules;
using HeckBot.Modules;

namespace HeckBot.Core
{
    class Program
    {
        DateTime lastDateStart; 
        static void Main(string[] args)
        {
            Program prg = new Program();
            prg.lastDateStart = DateTime.Now;
            prg.SetTimer();
            prg.RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient Client;
        private CommandService Commands;
        private List<IGuild> BotGuildList = new List<IGuild>();
        private System.Timers.Timer aTimer;

        public async Task RunBotAsync()
        {
            DiscordSocketConfig config = new DiscordSocketConfig();
            config.MessageCacheSize = 100;
            config.AlwaysDownloadUsers = true;
            Client = new DiscordSocketClient(config);
            Commands = new CommandService();

            string token = Secrets.BotToken;
            Client.Log += Client_Log;
            Commands.Log += Commands_log;
            await RegisterCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Commands_log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            Client.GuildAvailable += Client_GuildAvailable;
            await Commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        private Task Client_GuildAvailable(SocketGuild arg)
        {
            try
            {
                BotGuildList.Add(arg);
            }
            catch (Exception ex)
            {

            }
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Don't process the command if it was a system message
            var message = arg as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('&', ref argPos) ||
                message.Author.IsBot))
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(Client, message);
            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
                await Commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: null);
        }

        public void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Int32 dd = (lastDateStart.Date - DateTime.Now).Days;
            if(dd != 0)
            {
                IHeckUtils utils = new LocalHeckUtil();
                utils.ResetAvailableHecks();
                lastDateStart = DateTime.Now;
            }
        }
    }
}
