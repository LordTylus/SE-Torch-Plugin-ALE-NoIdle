using NLog;
using Sandbox.Game.Weapons;
using System;
using System.Reflection;
using Sandbox.ModAPI;

namespace ALE_NoIdle {
    class MyLargeTurretBasePatch {

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static readonly MethodInfo update =
            typeof(MyLargeTurretBase).GetMethod(nameof(MyLargeTurretBase.UpdateAfterSimulation10), BindingFlags.Instance | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        internal static readonly MethodInfo idleMover =
            typeof(MyLargeTurretBasePatch).GetMethod(nameof(IdleMover), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

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
