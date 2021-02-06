using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Heck.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeckBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        IHeckUtils utils = new LocalHeckUtil();
        [Command("heck")]
        public async Task Heck(string userm = null, float? Value = 1, [Remainder] string Name = null)
        {
            if (Context.Message.Author.IsBot)
                return;
            IUser dMUser = Context.Message.MentionedUsers.FirstOrDefault();
            if (dMUser == null)
            {
                await ReplyAsync("Go heck yourself! You must specify a user.");
                return;
            }
            if (Name == null)
            {
                await ReplyAsync("Get heck'd buddy! You must specify a reason to heck this person.");
                return;
            }
            if(dMUser.IsBot)
            {
                await ReplyAsync("Oh heck...That's a bot, bots are immune to hecks.");
                return;
            }
            HeckGuild guild = GetContextGuild();
            HeckUser usr = utils.GetHeckUser(Context.Message.Author.Id.ToString(), guild.ID, true);
            HeckUser mUsr = utils.GetHeckUser(dMUser.Id.ToString(), guild.ID, true);

            if ((Value ?? 0) == 0)
                Value = 1;
            if ((usr.AvailableHecks - Value.Value) <  0)
            {
                await ReplyAsync("Oh heck! You have don't have enough hecks left to give for that amount of heck.");
                return;
            }
            if(Value.Value < -100)
            {
                await ReplyAsync("Hecking rough dude, you can't heck that low.");
                return;
            }

            float nnValue = Value.Value;

            if (utils.CreateHeck(usr.ID, mUsr.ID, Name, nnValue))
                await ReplyAsync($"Heck u {dMUser.Mention} \"" + Name + "\". You've received " + nnValue.ToString() + " heck(s).");
            else
                await ReplyAsync("Oh heck that's horrible, something went wrong.");

        }

        [Command("heck")]
        public async Task Heck(string userm = null, [Remainder] string Name = null)
        {
            if (Context.Message.Author.IsBot)
                return;
            float? Value = 1;
            IUser dMUser = Context.Message.MentionedUsers.FirstOrDefault();
            if (dMUser == null)
            {
                await ReplyAsync("Go heck yourself! You must specify a user.");
                return;
            }
            if (Name == null)
            {
                await ReplyAsync("Get heck'd buddy! You must specify a reason to heck this person.");
                return;
            }
            if (dMUser.IsBot)
            {
                await ReplyAsync("Oh heck...That's a bot, bots are immune to hecks.");
                return;
            }
            HeckGuild guild = GetContextGuild();
            HeckUser usr = utils.GetHeckUser(Context.Message.Author.Id.ToString(), guild.ID, true);
            HeckUser mUsr = utils.GetHeckUser(dMUser.Id.ToString(), guild.ID, true);

            if ((Value ?? 0) == 0)
                Value = 1;
            if ((usr.AvailableHecks - Value.Value) < 0)
            {
                await ReplyAsync("Oh heck! You have don't have enough hecks left to give for that amount of heck.");
                return;
            }
            if (Value.Value < -100)
            {
                await ReplyAsync("Hecking rough dude, you can't heck that low.");
                return;
            }

            float nnValue = Value.Value;

            if (utils.CreateHeck(usr.ID, mUsr.ID, Name, nnValue))
                await ReplyAsync($"Heck u {dMUser.Mention} \"" + Name + "\". You've received " + nnValue.ToString() + " heck(s).");
            else
                await ReplyAsync("Oh heck that's horrible, something went wrong.");

        }

        [Command("info")]
        public async Task Info(string userm = null)
        {
            if (Context.Message.Author.IsBot)
                return;
            HeckGuild guild = GetContextGuild();
            IUser dUser = null;
            if (string.IsNullOrWhiteSpace(userm))
                dUser = Context.Message.Author;
            else
                dUser = Context.Message.MentionedUsers.FirstOrDefault();
            if (dUser == null)
            {
                await ReplyAsync("Go heck yourself! You must specify a user.");
                return;
            }
            if (dUser.IsBot)
            {
                await ReplyAsync("Oh heck...That's a bot, bots are immune to hecks.");
                return;
            }
            HeckUser usr = utils.GetHeckUser(dUser.Id.ToString(), guild.ID, true);
            UserHeckTotal userHeckTotal = utils.GetUserHeckTotal(usr.ID);

            if (userHeckTotal != null)
                await ReplyAsync($"Your current heck total weight with buffs is: " + userHeckTotal.HecksWeighted.ToString() +
                                    "\r\nYour current heck weight total is: " + userHeckTotal.WeightTotal.ToString() + 
                                    "\r\nYour current heck(s) unweighted is: " + userHeckTotal.HeckTotal.ToString() +
                                    "\r\nYour available heck(s) to heck with is: " + userHeckTotal.AvailableHecks.ToString() +
                                    $"\r\n\r\nGo heck yourself, {dUser.Mention}!");
            else
                await ReplyAsync($"Could not locate info for user.");
        }

        private HeckGuild GetContextGuild()
        {
            IGuild dGuild = Context.Guild;
            return utils.GetHeckGuild(dGuild.Id.ToString(), true);
        }

        [Command("leaderboard")]
        public async Task Leaderboard()
        {
            if (Context.Message.Author.IsBot)
                return;
            List<UserHeckTotal> lst = utils.GetLeaderboard(Context);
            if(lst != null && lst.Count > 0)
            {
                var builder = new EmbedBuilder();
                int index = 1;
                builder.WithTitle(Context.Guild.Name + " Heck Leaderboard"); 
                foreach (UserHeckTotal ttl in lst)
                {
                    builder.AddField(index.ToString() + ". " + ttl.User.Nickname, ttl.HecksWeighted + " Heck(s) Weighted");
                    index++;
                }

                builder.WithColor(Color.Red);
                Embed embed = builder.Build();
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
                await ReplyAsync($"Could not retrieve leaderboard.");
        }

        [Command("help")]
        public async Task Help()
        {
            await ReplyAsync($"Hello! I am friend heck bot. My friends call me heck bot. \r\n"
               + "You may speak to me using the following commands. \r\n\r\n\r\n" 
               + "&heck: Use this command, then specify a user, a value (Not required. Defaults to 1. User must have hecks available to heck with.), and a reason for their hecking.\r\n\r\n"
               + "&info: Use this command to see your heck total and available hecks. You may also mention a user specifically to see their heck total.\r\n\r\n"
               + "&leaderboard: Allows you to see the current users in the server ranked by heck-ness.\r\n\r\n"
               + "&version: Display current heck version.\r\n\r\n"
               + "&roadmap: See what is planned for the FUTURE. @.@ \r\n\r\n"
               + "&who: I guess I can tell you what's up. Use this command to find out what the heck is going on. @.@ \r\n\r\n"
               + "&help: Displays this message. Heck you.");
        }

        [Command("who")]
        public async Task Who()
        {
            await ReplyAsync($"So the whole point of this bot is so you can heck people. What is a heck? Well...we'll find out one day. " +
                $"Hecks refill to a specific value once a day. Once you are at 0 hecks, you can't heck anymore. I will also keep track of all the hecks everyone has and display them in a leaderboard. Have fun and go to heck.");
        }

        [Command("roadmap")]
        public async Task RoadMap()
        {
            await ReplyAsync($"Here is what is planned, you heckuva bench. \r\n"
               + "A hecks detail command that outputs the heck value, reason and who hecked you for your last 5 hecks.\r\n\r\n\r\n"
               + "The patent pending Heck Buff system.\r\n\r\n"
               + "Heck expirations. Keeping things competitive!\r\n\r\n"
               + "A command that will send your parents to heck.\r\n\r\n"
               + "Occasional input in conversation from the heck master itself, heck bot!\r\n\r\n"
               + "Have a hecking day!");
        }

        [Command("version")]
        public async Task Version()
        {
            string version = "0.0.0.306 pre-alpha";
            var rnd = new Random();
            int val = rnd.Next(1, 10);
            switch (val)
            {
                case 1:
                case 3:
                case 9:
                    await ReplyAsync($"Heck you for asking...but I am version " + version + "...");
                    break;
                case 2:
                case 4:
                case 8:
                    await ReplyAsync($"Oh heck yeah I am today years old and version " + version + "!");
                    break;
                default:
                    await ReplyAsync($"You are looking at version " + version + "! Heck me up, fam!");
                    break;
            }
        }

    }
}
