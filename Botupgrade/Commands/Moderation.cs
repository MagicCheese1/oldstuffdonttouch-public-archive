using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

using Botupgrade.Commands;
using Botupgrade.DataHandling;

namespace Botupgrade.Commands
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("Game <nazwa gry>"), Summary("ustaw grę w którą bot aktualnie gra"), Alias("Game")]
        public async Task Game(params string[] Game)
        {
            if (Utility.PermissionLevel(Context.User as SocketGuildUser) > Convert.ToInt32(ESettings.ModeratorDC[1]))
            {
                await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");
                return;
            }
            string sGame = "";
            foreach(var arg in Game)
            {
                sGame += " " + arg;
            }
            await Program.Client.SetGameAsync(sGame, "https://google.com");
        }
        [Command("reload"), Summary("odśwież ustawianie (settings.json)")]
        public async Task Reload()
        {
            if(Utility.hasRole((SocketGuildUser)Context.User, ESettings.Tech[1]))
            {
                await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");
                return;
            }
            ReloadSettings.Reload();
            await Context.Channel.SendMessageAsync(":white_check_mark: ustawienia zostaly odświeżone pomyślnie!");
        }
        
        //[Command("backdoor <serverid>"),Alias("backdoor"), Summary("Znajdź/Stwórz zaproszenie na podany serwer")]
        public async Task backdoor(ulong GuildId)
        {
            if(Utility.PermissionLevel((SocketGuildUser)Context.User) > Convert.ToInt32(ESettings.ModeratorDC[1]))
            {
                await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");
                return;
            }
            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.Channel.SendMessageAsync($":x: Tworzenie zaproszenia na ten serwer({GuildId}) nie powiodło się. (Bot nie ma dostępu do tego serwera)");
                return;
            }

            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
            try
            {
                var Invites = await Guild.GetInvitesAsync();
                if (Invites.Count() < 1)
                {
                    await Guild.TextChannels.First().CreateInviteAsync();
                }

                Invites = null;
                Invites = await Guild.GetInvitesAsync();
                EmbedBuilder Embed = new EmbedBuilder();
                Embed.WithTitle($"Zaproszenia na Serwer {Guild.Name}:");
                Embed.WithThumbnailUrl(Guild.IconUrl); 
                Embed.WithColor(40, 200, 150);
                int b=0;
                foreach (var Current in Invites)
                {
                    b++;
                    Embed.AddField( new string('-', 65), $"[zaproszenie {b}]({Current.Url})");
                }

                await Context.Channel.SendMessageAsync("", false, Embed.Build());
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($":x: Tworzenenie zaproszenia dla serwera **{Guild.Name}** Nie powiodło się! (``{ex.Message}``)");
                return;
            }
        }
        [Command("purge <Amount>"), Summary("usun podaną ilość wiadomości"), Alias("purge")]
        public async Task PurgeChat(uint amount)
        {
            var messages = await this.Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();

            await this.Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await this.ReplyAsync($"Usuwanie zakończone. _Ta wiadomość zostanie usunięta za {delay / 1000} sekund._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Command("ban <Użytkownik> <powdód> <czas(dni)>"), Summary("zbanuj Użytkownika"), Alias("ban")]
        public async Task ban(IGuildUser user = null, string reason = null, int TimeDays = 1)
        {
            if (Utility.PermissionLevel(Context.User as SocketGuildUser) > Convert.ToInt32(ESettings.ModeratorDC[1]))
                {await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");return;}
            if(user == null)
                {await Context.Channel.SendMessageAsync("musisz sprecyzować kogo chcesz zbanować!");return;}
            if(user.IsBot)
                {await Context.Channel.SendMessageAsync(":x: Nie możesz zbanować Bota!"); return;}
            if(Utility.PermissionLevel(user as SocketGuildUser) <= Convert.ToInt32(ESettings.ModeratorDC[1])) 
                {await Context.Channel.SendMessageAsync(":x: Nie możesz zbanować moderatora!"); return;}   
            await log($"Użytkownik {user} został zbanowany na {TimeDays} Dni za {reason}");
            await Context.Guild.AddBanAsync(user,TimeDays,reason);
        }
        [Command("kick <użytkownik>"), Summary("Wyrzuć użytkownika"), Alias("kick")]
        public async Task kick(IGuildUser user = null, string reason = "brak powodu")
        {
            if (Utility.PermissionLevel(Context.User as SocketGuildUser) > Convert.ToInt32(ESettings.ModeratorDC[1]))
                {await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");return;}
            if(user == null)
                {await Context.Channel.SendMessageAsync("musisz sprecyzować kogo chcesz wyrzucić!");return;}
            if(user.IsBot)
                {await Context.Channel.SendMessageAsync(":x: Nie możesz wyrzucić Bota!"); return;}
            if(Utility.PermissionLevel(user as SocketGuildUser) >= Convert.ToInt32(ESettings.ModeratorDC[1])) 
                {await Context.Channel.SendMessageAsync(":x: Nie możesz wyrzucić moderatora!"); return;}   
            await log($"Użytkownik {user} został Wyrzucony za {reason}");
            await user.KickAsync(reason);
        }
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Command("unban <użytkownik>"),Alias("unban"), Summary("odbanuj użytkownika")]
        public async Task unban(IGuildUser user = null)
        {
            if (Utility.PermissionLevel(Context.User as SocketGuildUser) > Convert.ToInt32(ESettings.ModeratorDC[1]))
                {await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");return;}
            if(user == null)
                {await Context.Channel.SendMessageAsync("musisz sprecyzować kogo chcesz odbanować!");return;}
            if (!Context.Guild.RemoveBanAsync(user).IsCompletedSuccessfully)
            {
                await Context.Channel.SendMessageAsync("coś poszło nie tak!");
                return;
            }
            await log($":tada: **{user}** został odbanowany!");
        }

        public async Task log(string Message)
        {
            await Program.Client.GetGuild(ESettings.logChannel[0])
            .GetTextChannel(ESettings.logChannel[1])
            .SendMessageAsync(Message);
        }
    }
}