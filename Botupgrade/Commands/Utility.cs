using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using Botupgrade.DataHandling;

namespace Botupgrade.Commands
{
    public class Utility : ModuleBase<SocketCommandContext>
    {
        [Command ("Help"), Summary ("pokazuje wszystkie komendy"), Alias ("h")]
        public async Task Help()
        {
            EmbedBuilder embed = new EmbedBuilder ();
            embed.WithColor (176, 52, 55);
            embed.WithDescription ("to są wszystkie komendy Tego serwera");
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            foreach(var Command in Program.Commands.Commands)
            {
                embed.AddField("g!" +Command.Name, Command.Summary);
            }
            await Context.Channel.SendMessageAsync ("", false, embed.Build ());
            await Context.Channel.SendMessageAsync("Jeżeli potrzebujesz pomocy Administratora napisz na kanale #❗pomoc❗");
        }
        [Command("administracjaMC"), Summary("Pokazuje administracje serwera Minecraft")]
        public async Task admin()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(176, 52, 55);
            embed.WithDescription("Administracja serwera minecraft");
            embed.AddField("**Właściciel**",$"{Program.Endevar.GetUser(469194209543323648).Username} / Kor_Zon");
            embed.AddField("**Współ-Właściciel**", $"{Program.Endevar.GetUser(513818440117518338).Username} / HNCAdmin");
            embed.AddField("**Opiekun**", $"Ferionnex");
            embed.AddField("**Admini**", $"{Program.Endevar.GetUser(354359350757687327).Username} / kubS169");
            embed.AddField("**Moderatorzy**", $"{Program.Endevar.GetUser(317903061206958080).Username} / Lurnite\nMakwoni\nTropicalSnowman");
            embed.AddField("**Helper**", "DomiNosek1337");
            embed.AddField("**Chatmod**",$"{Program.Endevar.GetUser(503639921286709248).Username} / Kokonek");
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            await Context.Channel.SendMessageAsync ("", false, embed.Build ());
        }
        [Command("administracjaDC"), Summary("Pokazuje administracje serwera discord")]
        public async Task adminDC()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(176, 52, 55);
            embed.WithDescription("Administracja serwera discord");
            embed.AddField("**Właściciel**","Dodison#0994");
            embed.AddField("**Współ-Właściciel**", "GRZANKA#8145");
            embed.AddField("**Moderatoratorzy**", "Poxiton#7146");
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            await Context.Channel.SendMessageAsync ("", false, embed.Build ());
        }


        [Command("Saytts <Wiadomość>"), Summary("say with tts"), Alias("saytts")]
        public async Task saytts([Remainder] string Input = "None")
        {
            if (PermissionLevel(Context.User as SocketGuildUser) > Convert.ToInt32(ESettings.Owner[1]))
            {
                await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");
                return;
            }
            if(Input != "None")
             await Context.Channel.SendMessageAsync(Input, true);
        }

        [Command("Say <Wiadomość>"), Summary("say"), Alias("say")]
        public async Task say([Remainder] string Input = "None")
        {
            if (PermissionLevel(Context.User as SocketGuildUser) > Convert.ToInt32(ESettings.ModeratorDC[1]))
            {
                await Context.Channel.SendMessageAsync(":x: Nie posiadasz uprawnień aby użyć tej komendy!");
                return;
            }
            if(Input != "None")
             await Context.Channel.SendMessageAsync(Input, false);
        }
        public static int PermissionLevel(SocketGuildUser user)
        {
            if(user.Roles.Contains(user.Guild.GetRole(ESettings.Owner[0]))) //Does the user have role with the Id of the Owner role?
                return Convert.ToInt32(ESettings.Owner[1]); //return Permission level of Owner
            else if(user.Roles.Contains(user.Guild.GetRole(ESettings.Tech[0]))) //Does the user have role with the Id of the Admin role?
                return Convert.ToInt32(ESettings.Tech[1]); //return Permission level of Admin
            else if(user.Roles.Contains(user.Guild.GetRole(ESettings.ModeratorDC[0])))//Does the user have role with the Id of the Moderator role?
                return Convert.ToInt32(ESettings.ModeratorDC[1]); //return Permission level of Moderator
            else
                return ESettings.Everyone; 

        }
        public static bool hasRole(SocketGuildUser user, ulong PermissionLevel)
        {
            if((user.Roles.Contains(user.Guild.GetRole(ESettings.Owner[1])) || user.Roles.Contains(user.Guild.GetRole(ESettings.coOwner))) && PermissionLevel == ESettings.Owner[0]) //Does the user have role with the Id of the Tech role?
                {return true;} //return Permission level of Tech
            else if(user.Roles.Contains(user.Guild.GetRole(ESettings.Tech[1])) && PermissionLevel == ESettings.Tech[0]) //Does the user have role with the Id of the Tech role?
                {return true;} //return Permission level of Tech
            else if(user.Roles.Contains(user.Guild.GetRole(ESettings.ModeratorDC[1])) && PermissionLevel == ESettings.ModeratorDC[0]) //Does the user have role with the Id of the Tech role?
                {return true;} //return Permission level of Tech
            else
                {return false;} //return Permission level of Tech
        }   

    }
}