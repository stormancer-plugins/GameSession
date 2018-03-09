using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stormancer;
using Stormancer.Server.GameSession.Models;

namespace Stormancer.Server.GameSession
{
    public interface IGameSessionService
    {
        void SetConfiguration(dynamic metadata);
        Task<Action<Stream,ISerializer>> PostResults(Stream inputStream, IScenePeerClient remotePeer);
        Task UpdateShutdownMode(ShutdownModeParameters shutdown, IScenePeerClient remotePeer);
        Task Reset();

        bool IsHost(long peerId);
    }
}
