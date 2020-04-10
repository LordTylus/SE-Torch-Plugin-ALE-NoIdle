using NLog;
using Sandbox.Game.Weapons;
using System;
using System.Reflection;
using Sandbox.ModAPI;
using Torch.Managers.PatchManager;

namespace ALE_NoIdle {
    
    [PatchShim]
    public static class MyLargeTurretBasePatch {

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static readonly MethodInfo UpdateMethod =
            typeof(MyLargeTurretBase).GetMethod(nameof(MyLargeTurretBase.UpdateAfterSimulation10), BindingFlags.Instance | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        internal static readonly MethodInfo IdleMoverPatch =
            typeof(MyLargeTurretBasePatch).GetMethod(nameof(IdleMover), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        public static void Patch(PatchContext ctx) {

            ctx.GetPattern(UpdateMethod).Prefixes.Add(IdleMoverPatch);

            Log.Debug("Patching Successful MyLargeTurretBase!");
        }

        public static bool IdleMover(MyLargeTurretBase __instance) {

            IMyLargeTurretBase turret = __instance as IMyLargeTurretBase;

            if (turret.EnableIdleRotation) {
                turret.EnableIdleRotation = false;
                turret.SyncEnableIdleRotation();
            }

            return true;
        }
    }
}
