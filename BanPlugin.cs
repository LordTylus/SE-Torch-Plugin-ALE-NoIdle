using NLog;
using System;
using System.IO;
using Torch;
using Torch.API;
using Torch.API.Plugins;

namespace ALE_BanNotifier {

    public class BanPlugin : TorchPluginBase
    {

        public static BanPlugin Instance { get; private set; }
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Persistent<BanConfig> _config;
        public BanConfig Config => _config?.Data;

        public void Save() => _config.Save();

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            var configFile = Path.Combine(StoragePath, "BanNotifier.cfg");

            try
            {
                _config = Persistent<BanConfig>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null) {

                Log.Info("Create Default Config, because none was found!");

                _config = new Persistent<BanConfig>(configFile, new BanConfig());
                _config.Save();
            }

            var pgmr = new BanManager(torch);
            torch.Managers.AddManager(pgmr);

            Instance = this;
        }
    }
}