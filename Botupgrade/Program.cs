using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Audio;
using Discord.Net;
using Discord.Rpc;

using Botupgrade.DataHandling;

namespace Botupgrade
{
    class Program
    {
        public static DiscordSocketClient Client;
        public static CommandService Commands;   
        public static SocketGuild Endevar;

        private static void Main(string[] args) => new Program ().MainAsync().GetAwaiter ().GetResult ();

        private async Task MainAsync()
        {
            ReloadSettings.Reload();
            Console.Clear ();

            Client = new DiscordSocketClient (new DiscordSocketConfig {
                LogLevel = LogSeverity.Debug
            });


            Commands = new CommandService (new CommandServiceConfig {
                CaseSensitiveCommands = false,
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Debug
            });


            Client.MessageReceived += Client_MessageReceived;
            await Commands.AddModulesAsync (Assembly.GetEntryAssembly ());

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;
            Client.ReactionAdded += Client_ReactionAdded;
            Client.UserJoined += Client_UserJoined;
            Client.UserLeft += Client_UserLeft;
            Client.ReactionRemoved += Client_ReactionRemoved;
            try{
            await Client.LoginAsync (TokenType.Bot, ESettings.token);
            await Client.StartAsync ();
            await Task.Delay (-1);
            } catch(Exception ex)
            {
                Console.WriteLine($" {DateTime.Now} at Auth] Error: \"{ex.Message}\"");
                Console.WriteLine(" Press ENTER to continue");
                Console.ReadLine();
                return;
            }
        } 
        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
           // var user = reaction.User.Value as SocketGuildUser;
           // var role = (user as IGuildUser).Guild.Roles.FirstOrDefault (x => x.Name == "Nowy");
           // if (user.Roles.Contains (role))
           // {
           //     if (reaction.MessageId == 516318859838357507)
           //      {
           //         if (reaction.Emote.Name == "👍") 
           //         {
           //             await (user as IGuildUser).RemoveRoleAsync (role);
           //             role = (user as IGuildUser).Guild.Roles.FirstOrDefault (x => x.Name == "Gracz");
           //             await (user as IGuildUser).AddRoleAsync (role);
           //             ulong id = 516166485069135882;
           //             var chnl = Client.GetChannel (id) as IMessageChannel;
           //             await chnl.SendMessageAsync ($"{reaction.User.Value.Mention} Zaakceptował regulamin!");
           //         }
           //     }
           // }
        }

        private async Task Client_ReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            
        }

        private async Task Client_UserLeft(SocketGuildUser user)
        {
            var channel = Client.GetChannel(523490219614535683) as SocketTextChannel;
            await channel.SendMessageAsync($"{user.Mention} Opuścił serwer :slight_frown:");
        }

        private async Task Client_UserJoined(SocketGuildUser user)
        {
           // var role = (user as IGuildUser).Guild.Roles.FirstOrDefault (x => x.Name == "Nowy");
           // await (user as IGuildUser).AddRoleAsync (role);
            var channel = Client.GetChannel(523490219614535683) as SocketTextChannel;
            await channel.SendMessageAsync($"{user.Mention}, witaj na serwerze {channel.Guild.Name} :tada: :hugging: :sunglasses:");
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync($"Baw się dobrze na naszym serwerze **{user.Guild}** 😉!! Pamiętaj przeczytać #❓regulamin❓ !!");
        }

       

        private async Task Client_Log(LogMessage Message)
        {
            //Log
            Console.WriteLine($" { DateTime.Now } at { Message.Source }] { Message.Message }");
        }

        private async Task Client_Ready()
        {
            Endevar = Client.GetGuild(523480673290289153);
            //Set the game the bot is playing
            await Client.SetGameAsync("Bycie pomocnym | g!help", "https://google.com/", StreamType.NotStreaming);

            //Get Info about Server
            foreach(var Guild in Client.Guilds)
            {
                var gpath = Directory.CreateDirectory(Assembly.GetEntryAssembly().Location.Replace("Botupgrade.dll","Guilds/" + Guild.Name + "/"));
                File.WriteAllText(gpath + "GUILDID.txt", Guild.Id.ToString());
                var rpath = Directory.CreateDirectory(gpath.FullName + "/allroles/");
                foreach(var role in Guild.Roles)
                {
                    File.WriteAllText(rpath.FullName + role.Name + ".txt", role.Id.ToString());              
                }
                var upath = Directory.CreateDirectory(gpath.FullName + "/users/");
                foreach(var user in Guild.Users)
                {
                    var uspath = Directory.CreateDirectory(upath.FullName + user.Username + "\\");
                    File.WriteAllText(uspath.FullName + "USERID.txt", user.Id.ToString());
                    foreach(var role in user.Roles)
                    {
                        File.WriteAllText(uspath.FullName + role.Name + ".txt", role.Id.ToString());
                    }
                }
            }
        }

        private async Task Client_MessageReceived(SocketMessage message)
        {
            //check if message is command and execute
            var Message = message as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            if(Context.Message == null || Context.Message.Content == "") return;
            if(Context.User.IsBot) return;
            
            int ArgPos = 0;
            if (!(Message .HasStringPrefix("g!", ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;
            var Result = await Commands.ExecuteAsync(Context, ArgPos);
            if(!Result.IsSuccess)
                Console.WriteLine($" { DateTime.Now } at Commands] Something went wrong with executing a command. Message: { Context.Message.Content } | Error: { Result.ErrorReason }");

        }
    }

    
}
