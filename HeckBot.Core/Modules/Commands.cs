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
            
            HeckGuild guild = GetContextGuild();
            HeckUser usr = null;
            HeckUser mUsr = null;
            IUser dMUser = null;

            if (Context.Message.ReferencedMessage != null)
            {
                dMUser = Context.Message.ReferencedMessage.Author;
                Name = "For your message: " + Context.Message.ReferencedMessage.Content;
            }
            else
            {
                dMUser = Context.Message.MentionedUsers.FirstOrDefault();
            }
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
            mUsr = utils.GetHeckUser(dMUser.Id.ToString(), guild.ID, dMUser.Username, true);
            usr = utils.GetHeckUser(Context.Message.Author.Id.ToString(), guild.ID, Context.Message.Author.Username, true);

            if ((Value ?? 0) == 0)
                Value = 1;
            if ((usr.AvailableHecks - Math.Abs(Value.Value)) < 0)
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
            HeckUser usr = utils.GetHeckUser(Context.Message.Author.Id.ToString(), guild.ID, Context.Message.Author.Username, true);
            HeckUser mUsr = utils.GetHeckUser(dMUser.Id.ToString(), guild.ID, dMUser.Username, true);

            if ((Value ?? 0) == 0)
                Value = 1;
            if ((usr.AvailableHecks - Math.Abs(Value.Value)) < 0)
            {
                await ReplyAsync("Oh heck! You have don't have enough hecks left to give for that amount of heck.");
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
            HeckUser usr = utils.GetHeckUser(dUser.Id.ToString(), guild.ID, dUser.Username, true);
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
                    builder.AddField(index.ToString() + ". " + ttl.User.UserName, ttl.HecksWeighted + " Heck(s) Weighted");
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
            HeckParameter param = utils.GetHeckParameterByName("HELP");
            if (param != null)
            {
                await ReplyAsync(param.Param_Value);
            }
        }

        [Command("who")]
        public async Task Who()
        {
            HeckParameter param = utils.GetHeckParameterByName("WHO");
            if (param != null)
            {
                await ReplyAsync(param.Param_Value);
            }
        }

        [Command("roadmap")]
        public async Task RoadMap()
        {
            HeckParameter param = utils.GetHeckParameterByName("ROADMAP");
            if (param != null)
            {
                await ReplyAsync(param.Param_Value);
            }
        }

        [Command("version")]
        public async Task Version()
        {
            HeckParameter param = utils.GetHeckParameterByName("VERSION");
            if (param != null)
            {
                string version = param.Param_Value;
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

        [Command("rec")]
        public async Task ReceivedHecks(string userm = null)
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
            HeckUser usr = utils.GetHeckUser(dUser.Id.ToString(), guild.ID, dUser.Username, true);
            List<HeckTextRecord> hecks = utils.GetHecksReceived(usr.ID);

            if (hecks.Count > 0)
            {
                var builder = new EmbedBuilder();
                int index = 1;
                builder.WithTitle(dUser.Username + " Hecks Received");

                foreach (HeckTextRecord heck in hecks)
                {
                    builder.AddField(index.ToString(), heck.Value);
                    index++;
                }
                builder.WithColor(Color.Red);
                Embed embed = builder.Build();
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
                await ReplyAsync($"Could not locate hecks received for user.");
        }

        [Command("sent")]
        public async Task SentHecks(string userm = null)
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
            HeckUser usr = utils.GetHeckUser(dUser.Id.ToString(), guild.ID, dUser.Username, true);
            List<HeckTextRecord> hecks = utils.GetHecksSent(usr.ID);

            if (hecks.Count > 0)
            {
                var builder = new EmbedBuilder();
                int index = 1;
                builder.WithTitle(dUser.Username + " Hecks Sent");

                foreach (HeckTextRecord heck in hecks)
                {
                    builder.AddField(index.ToString(), heck.Value);
                    index++;
                }
                builder.WithColor(Color.Red);
                Embed embed = builder.Build();
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
                await ReplyAsync($"Could not locate hecks sent for user.");
        }

    }
}
