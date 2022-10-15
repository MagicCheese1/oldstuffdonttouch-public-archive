using System.Collections.Generic;

namespace Botupgrade.DataHandling
{
    public class Setting
    {
        public string token{get; set;}
        public List<ulong> logChannel{get; set;}
        public List<ulong> Owner{get; set;}
        public ulong CoOwner{get; set;}
        public List<ulong> ModeratorDC{get; set;}
        public List<ulong> Tech{get; set;}
        public int Everyone{get; set;}
        public string UserAccountsPath {get; set;}
    }
}