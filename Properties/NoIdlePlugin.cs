using NLog;
using Torch;
using Torch.API;

namespace ALE_NoIdle {

    public class NoIdlePlugin : TorchPluginBase
    {

        public static NoIdlePlugin Instance { get; private set; }
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            var pgmr = new NoIdleManager(torch);
            torch.Managers.AddManager(pgmr);

            Instance = this;
        }
    }
}