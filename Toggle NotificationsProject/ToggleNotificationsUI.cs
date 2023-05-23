using KSP.Messages;
using KSP.Sim.Definitions;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class ToggleNotificationsUI : MonoBehaviour
    {
        internal static ToggleNotificationsUI instance;
        internal static NotificationToggle toggleNotification;
        internal ToggleNotificationsPlugin mainPlugin;
        private MessageCenter messageCenter;
        private SubscriptionHandle gamePauseHandle;
        private PartBehaviourModule partBehaviourModule;
        private SubscriptionHandle _onActionActivateMessageHandle;

        //buttons
        internal int selectedButton1 = 1;
        internal int selectedButton2 = 1;
        internal int selectedButton3 = 1;
        internal int selectedButton4 = 1;

        internal bool isToggled;
        internal bool isToggled2;
        internal ToggleNotificationsUI(ToggleNotificationsPlugin mainPlugin, bool _isGUIenabled, MessageCenter messageCenter)
        {
            if (mainPlugin != null)
            {
                instance = this;
                this.mainPlugin = mainPlugin;
                this.messageCenter = messageCenter;
            }
            else
            {
                Debug.LogError("ToggleNotificationsPlugin instance is null. ToggleNotificationUI");
            }
        }
        private void Start()
        {
            messageCenter = FindObjectOfType<MessageCenter>();

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null");
            }
        }

        //Pause Game Notification
        private void GamePauseToggledMessageCallback(MessageCenterMessage msg)
        {
            GamePauseToggledMessage pauseToggledMessage = msg as GamePauseToggledMessage;
            if (pauseToggledMessage != null)
            {
                // Update the notification display based on the selectedButton1 value
                if (selectedButton1 == 1)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                }
                else if (selectedButton1 == 0)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                }
            }
        }
        private void ButtonToggle1(int toggleValue1)
        {
            if (mainPlugin == null || mainPlugin.notificationToggle == null) { Debug.LogError("mainPlugin or mainPlugin.notificationToggle is null ButtonToggle1"); return; }

            if (messageCenter == null) { Debug.LogError("messageCenter is null. buttonToggle1."); return; }

            if (selectedButton1 == toggleValue1) { return; }

            selectedButton1 = toggleValue1;

            if (selectedButton1 == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                AssistantToTheAssistantPatchManager.isGamePaused = false;
                AssistantToTheAssistantPatchManager.isPauseVisible = false;
                AssistantToTheAssistantPatchManager.isPausePublish = false;
                AssistantToTheAssistantPatchManager.isOnPaused = false;

                // Set isToggled to true
                isToggled = true;

                // Create and publish the GamePauseToggledMessage with IsPaused set to true
                GamePauseToggledMessage message = new GamePauseToggledMessage();
                message.IsPaused = true;
                messageCenter.Publish<GamePauseToggledMessage>(message);

                // Subscribe to the GamePauseToggledMessage and define the callback logic
                gamePauseHandle = messageCenter.Subscribe<GamePauseToggledMessage>(GamePauseToggledMessageCallback);
                //pauseStateChangedHandle = messageCenter.Subscribe<PauseStateChangedMessage>(PauseStateChangedMessageCallback);

                // Set the GUI style for TNBaseStyle.Toggle
                TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
                TNBaseStyle.ToggleError.normal.textColor = ColorTools.ParseColor("#C0E2DC");


                Debug.Log($"Toggle 1 enabled");

                Debug.Log($"Initial isToggled value: {isToggled}");
                Debug.Log($"Initial isGamePaused value: {AssistantToTheAssistantPatchManager.isGamePaused}");
                Debug.Log($"Initial isPauseVisible value: {AssistantToTheAssistantPatchManager.isPauseVisible}");
                Debug.Log($"Initial isPausePublish value: {AssistantToTheAssistantPatchManager.isPausePublish}");
                Debug.Log($"Initial isOnPaused value: {AssistantToTheAssistantPatchManager.isOnPaused}");
            }
            else if (selectedButton1 == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                AssistantToTheAssistantPatchManager.isGamePaused = true;
                AssistantToTheAssistantPatchManager.isPauseVisible = true;
                AssistantToTheAssistantPatchManager.isPausePublish = true;
                AssistantToTheAssistantPatchManager.isOnPaused = true;

                isToggled = false;

                // Create and publish the GamePauseToggledMessage with IsPaused set to false
                GamePauseToggledMessage message = new GamePauseToggledMessage();
                message.IsPaused = false;
                messageCenter.Publish<GamePauseToggledMessage>(message);

                // Subscribe to the GamePauseToggledMessage and define the callback logic
                gamePauseHandle = messageCenter.Subscribe<GamePauseToggledMessage>(GamePauseToggledMessageCallback);
                //pauseStateChangedHandle = messageCenter.Subscribe<PauseStateChangedMessage>(PauseStateChangedMessageCallback);

                // Set the GUI style for TNBaseStyle.ToggleError
                TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0E2DC");
                TNBaseStyle.ToggleError.normal.textColor = Color.red;


                Debug.Log($"Toggle 1 disabled");

                Debug.Log($"Initial isToggled value: {isToggled}");
                Debug.Log($"Initial isGamePaused value: {AssistantToTheAssistantPatchManager.isGamePaused}");
                Debug.Log($"Initial isPauseVisible value: {AssistantToTheAssistantPatchManager.isPauseVisible}");
                Debug.Log($"Initial isPausePublish value: {AssistantToTheAssistantPatchManager.isPausePublish}");
                Debug.Log($"Initial isOnPaused value: {AssistantToTheAssistantPatchManager.isOnPaused}");
            }
        }

        //Solar Panel Notification
        private void SolarPanelsIneffectiveMessageCallback(MessageCenterMessage msg)
        {
            SolarPanelsIneffectiveMessage solarToggle = msg as SolarPanelsIneffectiveMessage;
            if (solarToggle != null)
            {
                // Update the notification display based on the selectedButton1 value
                if (selectedButton2 == 1)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                }
                else if (selectedButton2 == 0)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
                }
            }
        }
        private void ButtonToggle2(int toggleValue2)
        {
            if (mainPlugin == null || mainPlugin.notificationToggle == null)
            {
                Debug.LogError("mainPlugin or mainPlugin.notificationToggle is null ButtonToggle2");
                return;
            }

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null. ButtonToggle2.");
                return;
            }

            if (selectedButton2 == toggleValue2) { return; }

            selectedButton2 = toggleValue2;

            if (selectedButton2 == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = false;
                messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                Debug.Log($"Toggle 2 disabled");
                Debug.Log($"Initial isToggled2 value: {isToggled2}");
                Debug.Log($"Initial isSolarPanelsEnabled value: {AssistantToTheAssistantPatchManager.isSolarPanelsEnabled}");
            }
            else if (selectedButton2 == 0)
            {
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = true;

                // Unsubscribe from the SolarPanelsIneffectiveMessage
                messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                // Create and publish the SolarPanelsIneffectiveMessage
                SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
                messageCenter.Publish<SolarPanelsIneffectiveMessage>(message);

                // Subscribe to the SolarPanelsIneffectiveMessage
                messageCenter.PersistentSubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
                _onActionActivateMessageHandle = messageCenter.Subscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                // Set the GUI style for TNBaseStyle.Toggle
                TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
                TNBaseStyle.ToggleError.normal.textColor = ColorTools.ParseColor("#C0E2DC");

                Debug.Log($"Toggle 2 enabled");
                Debug.Log($"Initial isToggled2 value: {isToggled2}");
                Debug.Log($"Initial isSolarPanelsEnabled value: {AssistantToTheAssistantPatchManager.isSolarPanelsEnabled}");
            }
        }
        private void ButtonToggle3(int toggleValue3)
        {
            selectedButton3 = toggleValue3;

            if (selectedButton3 == 1)
            {
                // mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
            }
            else if (selectedButton3 == 0)
            {
                // mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
            }
        }
        private void ButtonToggle4(int toggleValue4)
        {
            selectedButton4 = toggleValue4;

            if (selectedButton4 == 1)
            {
                //mainPlugin.notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, true);
            }
            else if (selectedButton4 == 0)
            {
                //mainPlugin.notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, false);
            }
        }
        internal void FillWindow(int windowID)
        {
            // Initialize the position of the buttons
            TopButtons.Init(mainPlugin.windowRect.width);

            GUILayout.BeginHorizontal();

            // MENU BAR
            GUILayout.FlexibleSpace();

            GUI.Label(new Rect(10f, 4f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            Rect closeButtonPosition = new Rect(mainPlugin.windowRect.width - 10, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);

            if (TopButtons.Button(TNBaseStyle.Cross))
                mainPlugin.CloseWindow();

            GUILayout.Space(10);

            if (TopButtons.Button(TNBaseStyle.Gear))
            {
                // Handle the gear button action here if needed
            }

            GUILayout.EndHorizontal();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Notification Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            // Group 2: Toggle Buttons
            GUILayout.BeginVertical(GUILayout.Height(60));

            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding

            Rect gamePauseToggleRect = new Rect(3, 56, buttonWidth, 20);

            GUIStyle toggleStyle = isToggled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = isToggled ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");

            bool gamePauseToggle = GUI.Toggle(gamePauseToggleRect, isToggled, "Game Pause", toggleStyle);
            toggleStyle.normal.textColor = textColor;

            if (gamePauseToggle != isToggled)
            {
                isToggled = gamePauseToggle;
                ButtonToggle1(isToggled ? 1 : 0);
            }

            bool solarToggle = GUI.Toggle(new Rect(3, 96, buttonWidth, 20), AssistantToTheAssistantPatchManager.isSolarPanelsEnabled, "Solar Panel Ineffective (soon.tm)",
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError);
            TNBaseStyle.Toggle.normal.textColor = AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = !AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? Color.red : ColorTools.ParseColor("#C0E2DC");

            if (solarToggle != AssistantToTheAssistantPatchManager.isSolarPanelsEnabled)
            {
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = solarToggle;
                ButtonToggle2(AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? 1 : 0);
            }

            bool radioButton3 = GUI.Toggle(new Rect(3, 133, buttonWidth, 20), selectedButton3 == 1, "Out of Fuel (soon.tm)", selectedButton3 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton3 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton3 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton3)
            {
                selectedButton3 = 1;
            }
            else
            {
                selectedButton3 = 0;
            }

            bool radioButton4 = GUI.Toggle(new Rect(3, 173, buttonWidth, 20), selectedButton4 == 1, "No Electricity (soon.tm)", selectedButton4 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton4 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton4 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton4)
            {
                selectedButton4 = 1;
            }
            else
            {
                selectedButton4 = 0;
            }

            GUILayout.EndVertical();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Group 3: Version Number
            GUIStyle nameLabelStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.MiddleRight
            };

            GUILayout.FlexibleSpace();

            GUILayout.Label("v0.2.2", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            mainPlugin.saverectpos();
        }
    }
}
