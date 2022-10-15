using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace Botupgrade.Commands
{
    public class AudioCommands : ModuleBase<SocketCommandContext>
    {
        [Command("Join"), Summary("nie twój biznes dziunia")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Message.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

           // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();
        }

        [Command("Play", RunMode = RunMode.Async), Summary("Najlepsza sieć w polsce")]
        public async Task Play()
        {
            
        }
    }
}