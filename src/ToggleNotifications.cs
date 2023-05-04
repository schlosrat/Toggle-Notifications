using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using UnityEngine;
using System;
using BepInEx.Logging;
using System.Reflection;

namespace ToggleNotifications
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]

    [HarmonyPatch(typeof(DiscoverableMessage))]
    [HarmonyPatch("isActive", MethodType.Setter)]
    [DiscoverableMessage("Events/Flight/Parts/SolarPanelsIneffectiveMessage", false, "Solar panels fully deployed but ineffective.", "")]

    [HarmonyPatch(typeof(NotificationEvents))]
    [HarmonyPatch("Update")]

    public class ToggleNotificationsPlugin : BaseSpaceWarpPlugin
    {
        private ConfigEntry<bool> _enableNotificationsConfig;

        public const string ModGuid = "com.github.cvusmo.Toggle-Notifications";
        public const string ModName = "Toggle Notifications";
        public const string ModVer = "0.1.0";

        private bool gameInputState = true;

        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private const string ToolbarOABButtonID = "BTN-ToggleNotificationsOAB";
        private bool _isWindowOpen;
        private Rect _windowRect;
        private static object _solarPanelsIneffectiveNotificationHandle;
        private bool _ToggleNotifications;

        public static ToggleNotificationsPlugin Instance { get; private set; }
        public new static ManualLogSource Logger { get; set; }

        public override void OnInitialized()
        {
            base.OnInitialized();
            ToggleNotificationsPlugin.Instance = this;
            this.gameInputState = GameManager.Instance.Game;

            _enableNotificationsConfig = Config.Bind("Settings section", "Enable Notifications", true, "Toggle Notifications: Enabled (true) or Disabled (false)");
            Logger.LogInfo($"Toggle Notifications Plugin: Enabled = {_enableNotificationsConfig.Value}");

            // Register Flight AppBar button
            Appbar.RegisterAppButton(
                "Toggle Notifications",
                ToolbarFlightButtonID,
                AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
                isOpen =>
                {
                    _isWindowOpen = isOpen;
                    GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
                }
            );

            // Register OAB AppBar Button
            Appbar.RegisterOABAppButton(
                "Toggle Notifications",
                ToolbarOABButtonID,
                AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
                isOpen =>
                {
                    _isWindowOpen = isOpen;
                    GameObject.Find(ToolbarOABButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
                }
            );

            //variables that disable notifications by type of notification

            var notificationEventsType = typeof(NotificationEvents);

            //insufficientstellarexposure notification
            var insufficientStellarExposureField = AccessTools.Field(notificationEventsType, "InsufficientStellarExposure");
            var insufficientStellarExposureEvent = (Delegate)insufficientStellarExposureField.GetValue(null);
            insufficientStellarExposureEvent?.DynamicInvoke(new object[] { new Action(() => { }), null });

            var solarPanelsIneffectiveTimeToWaitTillField = AccessTools.Field(notificationEventsType, "_solarPanelsIneffectiveTimeToWaitTill");
            var solarPanelsIneffectiveTimeToWaitTill = (Delegate)solarPanelsIneffectiveTimeToWaitTillField.GetValue(null);

            // Check if the notification delegate is not null before trying to disable it
            if (solarPanelsIneffectiveTimeToWaitTill != null)
            {
                // Set a custom time delay for the notification
                float timeDelay = float.MaxValue; // Set your custom time delay here
                solarPanelsIneffectiveTimeToWaitTill.DynamicInvoke(new Action(() => { }), timeDelay);
            }

            //partineffective notification
            var partIneffectiveField = AccessTools.Field(notificationEventsType, "PartIneffective");
            var partIneffectiveEvent = (Delegate)partIneffectiveField.GetValue(null);

            // Check if the notification delegate is not null before trying to disable it
            if (partIneffectiveEvent != null)
            {
                // Set a custom time delay for the notification
                float timeDelay = float.MaxValue; // Set your custom time delay here
                partIneffectiveEvent.DynamicInvoke(new Action(() => { }), timeDelay);
            }

            // Harmony creates the plugin/patch
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);
        }
        private void Update()
        {
            //check if notications are enabled/disabled?
            if (!_enableNotificationsConfig.Value)
            {
                var notificationEventsType = typeof(NotificationEvents);

                // Set the handle to null if it exists
                _solarPanelsIneffectiveNotificationHandle = null;

                // Disable solar panel ineffective notification
                var solarPanelsIneffectiveTimeToWaitTillField = AccessTools.Field(notificationEventsType, "_solarPanelsIneffectiveTimeToWaitTill");
                var solarPanelsIneffectiveTimeToWaitTill = (Delegate)solarPanelsIneffectiveTimeToWaitTillField.GetValue(null);

                // Check if the notification delegate is not null before trying to disable it
                if (solarPanelsIneffectiveTimeToWaitTill != null)
                {
                    // Set a custom time delay for the notification
                    float timeDelay = float.MaxValue; // Set your custom time delay here
                    solarPanelsIneffectiveTimeToWaitTill.DynamicInvoke(new Action(() => { }), timeDelay);
                }

                // Disable part ineffective notification
                var partIneffectiveField = AccessTools.Field(notificationEventsType, "PartIneffective");
                var partIneffectiveEvent = (Delegate)partIneffectiveField.GetValue(null);

                // Check if the notification delegate is not null before trying to disable it
                if (partIneffectiveEvent != null)
                {
                    // Set a custom time delay for the notification
                    float timeDelay = float.MaxValue; // Set your custom time delay here
                    partIneffectiveEvent.DynamicInvoke(new Action(() => { }), timeDelay);
                }
            }
        }

        //gui not super important but would be nice to have a gui for community fixes
        private void OnGUI()
        {
            // Set the UI
            GUI.skin = Skins.ConsoleSkin;

            if (_isWindowOpen)
            {
                _windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    _windowRect,
                    FillWindow,
                    "Toggle Notifications",
                    GUILayout.Height(350),
                    GUILayout.Width(350)
                );
            }
        }
        private static void FillWindow(int windowID)
        {
            GUILayout.Label("Toggle Notifications - Toggle Notifications to be added to Community Fixes");
            GUI.DragWindow(new Rect(80, 20, 500, 500));
        }
        // Method to toggle the isActive property of the DiscoverableMessage attribute
        static void Postfix(NotificationEvents __instance)
        {
            _solarPanelsIneffectiveNotificationHandle = null;
        }
    }
}