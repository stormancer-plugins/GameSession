using Server.Plugins.API;
using Stormancer;
using Stormancer.Diagnostics;
using Stormancer.Plugins;
using System.Threading.Tasks;
using Stormancer.Server.GameSession.Models;
using Stormancer.Platform.Core.Cryptography;
using Stormancer.Server.Components;
using Server.Users;

namespace Stormancer.Server.GameSession
{
    class GameSessionController : ControllerBase
    {
        private readonly IGameSessionService _service;
        private readonly ILogger _logger;
        private readonly IUserSessions _sessions;
        private readonly IEnvironment _environment;

        public GameSessionController(IGameSessionService service, ILogger logger, IUserSessions sessions, IEnvironment environment)
        {
            _service = service;
            _logger = logger;
            _sessions = sessions;
            _environment = environment;
        }
        public async Task PostResults(RequestContext<IScenePeerClient> ctx)
        {
            var writer = await _service.PostResults(ctx.InputStream, ctx.RemotePeer);
            ctx.SendValue(s =>
            {
                var oldPosition = s.Position;
                writer(s, ctx.RemotePeer.Serializer());
                //_logger.Log(LogLevel.Trace, "gamesession.postresult", "sending response to postresponse message.", new { Length = s.Position - oldPosition });
            });

        }

        public Task Reset(RequestContext<IScenePeerClient> ctx)
        {
            return _service.Reset();
        }

        public Task UpdateShutdownMode(RequestContext<IScenePeerClient> ctx)
        {
            ShutdownModeParameters shutdown = ctx.ReadObject<ShutdownModeParameters>();
            return _service.UpdateShutdownMode(shutdown, ctx.RemotePeer);
        }

        public async Task GetUserFromBearerToken(RequestContext<IScenePeerClient> ctx)
        {
            var app = await _environment.GetApplicationInfos();
            var data = TokenGenerator.DecodeToken<BearerTokenData>(ctx.ReadObject<string>(), app.PrimaryKey);
            if (data == null)
            {
                throw new ClientException("Invalid Token");
            }
            var session = await _sessions.GetSession(data.PeerId);

            ctx.SendValue(session?.User.Id);
        }
        
        public class BearerTokenData
        {
            public long PeerId { get; set; }
        }
    }
}
