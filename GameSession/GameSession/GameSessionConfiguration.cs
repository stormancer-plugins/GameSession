using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stormancer.Server.GameSession
{
    public class GameSessionConfiguration
    {
        public List<string> userIds { get; set; } = new List<string>();

        /// <summary>
        /// True if anyone can connect to the game session.
        /// </summary>
        public bool Public { get; set; }
        public bool canRestart { get; set; }
        public object UserData { get; set; }
    }
}
