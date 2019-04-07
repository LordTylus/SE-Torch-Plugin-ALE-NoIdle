using NLog;
using System.Threading.Tasks;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Managers;
using Torch.Managers.PatchManager;
using Torch.Session;

namespace ALE_NoIdle {
    class NoIdleManager : Manager {

        [Dependency]
        private TorchSessionManager sessionManager;

        [Dependency]
        private readonly PatchManager patchManager;
        private PatchContext ctx;

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public NoIdleManager(ITorchBase torchInstance)
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

            if (ctx == null)
                ctx = patchManager.AcquireContext();
        }

        /// <inheritdoc />
        public override void Detach() {
            base.Detach();

            patchManager.FreeContext(ctx);

            Log.Info("Detached!");
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state) {
            switch (state) {
                case TorchSessionState.Loaded:
                    Task.Delay(3000).ContinueWith((t) => {
                        Log.Debug("Patching MyLargeTurretBasePatch");
                        ctx.GetPattern(MyLargeTurretBasePatch.update).Prefixes.Add(MyLargeTurretBasePatch.idleMover);
                        patchManager.Commit();
                    });
                    break;
            }
        }
    }
}