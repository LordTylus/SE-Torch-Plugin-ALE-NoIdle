using NLog;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Managers;
using Torch.Managers.PatchManager;
using Torch.Server;
using Torch.Server.Managers;
using Torch.Session;

namespace ALE_BanNotifier {
    class BanManager : Manager {

        [Dependency]
        private TorchSessionManager sessionManager;

        private IMultiplayerManagerServer multiplayerManager;

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public BanManager(ITorchBase torchInstance)
            : base(torchInstance) {
        }

        /// <inheritdoc />
        public override void Attach() {
            base.Attach();

            sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");
        }

        /// <inheritdoc />
        public override void Detach() {
            base.Detach();

            Log.Info("Detached!");
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state) {

            switch (state) {

                case TorchSessionState.Loaded:

                    Log.Info("Registering Ban Listner!");

                    multiplayerManager = Torch.CurrentSession.Managers.GetManager<IMultiplayerManagerServer>();
                    if (multiplayerManager != null)
                        multiplayerManager.PlayerBanned += PlayerBanned;
                    else
                        Log.Warn("No session manager loaded!");

                    break;

                case TorchSessionState.Unloading:

                    Log.Info("Unregistering Ban Listner!");

                    if (multiplayerManager != null)
                        multiplayerManager.PlayerBanned -= PlayerBanned;

                    break;
            }
        }

        private void PlayerBanned(ulong steamId, bool isBanned) {

            Log.Error("Player with ID " + steamId + " isBanned " + isBanned);

        }
    }
}