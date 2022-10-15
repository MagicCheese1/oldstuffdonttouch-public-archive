using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace Youtube_Download_Test {
    class Program {
        static void Main (string[] args) => new Program ().MainAsync ().GetAwaiter ().GetResult ();

        private async Task MainAsync () {
            while (true) {
                var client = new YoutubeClient ();
                Console.Clear ();
                Console.Write ("URL(\"Search\" for Search): ");
                var url = Console.ReadLine ();
                if (url == "Search") {
                    goto Search;
                }
                var id = YoutubeClient.ParseVideoId (url);
                Console.WriteLine ("ID: " + id);

                var video = await client.GetVideoAsync (id);

                Console.WriteLine ($"Title: {video.Title}");
                Console.WriteLine ($"Author: {video.Author}");
                Console.WriteLine ($"Duration: {video.Duration}");

                Console.ReadKey ();

                var streamInfoSet = await client.GetVideoMediaStreamInfosAsync (id);
                var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality ();
                //Console.WriteLine("Resolution: " +streamInfo.Resolution);
                Console.WriteLine ("Video Quality: " + streamInfo.VideoQuality);
                Console.WriteLine ("Video Size: " + ((streamInfo.Size / 1024f) / 1024f) + "MB");
                Ask:
                    Console.WriteLine ("Are you sure you want to download this video? (Y/N/A(Only Audio))");
                var Input = Console.ReadLine ();

                if (Input.ToLower () == "n") {
                    continue;
                } else if (Input.ToLower () == "a") {
                    var streamInfoA = streamInfoSet.Audio.WithHighestBitrate ();
                    Console.WriteLine ("Bitrate: " + streamInfoA.Bitrate);
                    Console.WriteLine ("Audio Size: " + ((streamInfoA.Size / 1024f) / 1024f) + "MB");
                    var ext2 = streamInfoA.Container.GetFileExtension ();
                    var t2 = client.DownloadMediaStreamAsync (streamInfoA, $"downloaded_video.{ext2}");
                    Console.WriteLine ("Download Started");
                    t2.Wait ();
                    Console.WriteLine ("Download Completed Successfully!");
                    Console.ReadKey ();
                    continue;
                } else if (Input.ToLower () != "y") {
                    goto Ask;
                }
                var ext = streamInfo.Container.GetFileExtension ();
                var t = client.DownloadMediaStreamAsync (streamInfo, $"downloaded_video.{ext}");
                Console.WriteLine ("Download Started");
                t.Wait ();
                Console.WriteLine ("Download Completed Successfully!");
                Console.ReadKey ();

                Search:
                    Console.Clear ();
                Console.Write ("Search:");
                var r = Console.ReadLine ();
                var f = await client.SearchVideosAsync (r, 1);
                foreach (var Video in f) {
                    Console.WriteLine ($"Title: {Video.Title}\nAuthor: {Video.Author}\nDuration {Video.Duration}\nID: {Video.Id}\nURL: {@"https://www.youtube.com/watch?v=" + Video.Id} \n");
                }
                Console.ReadKey ();
            }
        }

    }
}