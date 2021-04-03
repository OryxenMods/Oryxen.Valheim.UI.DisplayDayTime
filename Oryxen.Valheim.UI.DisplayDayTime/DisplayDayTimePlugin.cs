using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Oryxen.Valheim.UI.DisplayDayTime
{
    [BepInPlugin(ID, "Display Day & Time in HUD", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class DisplayDayTimePlugin : BaseUnityPlugin
    {
        public const string ID = "oryxen.valheim.ui.displaydaytime";

        #region BepInEx configs
        private static ConfigEntry<bool> _displayUnderMiniMap;
        private static ConfigEntry<bool> _displayTime;
        private static ConfigEntry<bool> _displayDay;
        private static ConfigEntry<bool> _twentyFourHourClock;

        private const int FONT_SIZE = 18;
        #endregion

        #region Unity game objects & components
        private static GameObject _panel;
        private static Text _dayText;
        private static Text _timeText;
        #endregion

        private Harmony _harmony;

        private void Awake()
        {
            //Bind BepInEx configs
            _displayUnderMiniMap = Config.Bind("General", "Display under minimap", false, "Display under minimap");
            _displayTime = Config.Bind("General", "Display time", true, "Display time");
            _displayDay = Config.Bind("General", "Display day", true, "Display day");
            _twentyFourHourClock = Config.Bind("General", "24-hour clock", true, "24-hour clock");

            //Apply patches
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ID);
        }

        private void OnDestroy()
        {
            //Destroy the created game object when mod is unloaded.
            if (Hud.instance && _panel != null)
			{
                Log("Destroying panel...");
                Destroy(_panel);
                Log("Panel destroyed!");
            }

            _harmony?.UnpatchAll(ID);
        }

        private void Update()
		{
            if (!Player.m_localPlayer || !Hud.instance || !Traverse.Create(Hud.instance).Method("IsVisible").GetValue<bool>()) return;

            if (_panel == null)
			{
                CreatePanel(Hud.instance);
			}

            var rect = _panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-185, _displayUnderMiniMap.Value ? -255 : -20);

            _dayText.enabled = _displayDay.Value;
            _timeText.enabled = _displayTime.Value;

            if (_displayDay.Value)
			{
                UpdateDay();
			}

            if (_displayDay.Value)
            {
                UpdateTime();
            }
        }

        /// <summary>
        /// Creates a panel game object with the day and time game object as children to display the day and time as text.
        /// This panel is added as a child to the hudroot and anchored to the top right of the screen (above the minimap). 
        /// </summary>
        public static void CreatePanel(Hud hudInstance)
		{
            Log("Creating panel...");

            _panel = new GameObject("DayTimePanel")
            {
                layer = 5
            };
            _panel.transform.SetParent(hudInstance.m_rootObject.transform);
            var rect = _panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-185, _displayUnderMiniMap.Value ? -255 : - 20);

            Log("Panel created!");

            CreateDay(hudInstance.m_healthText.color, hudInstance.m_healthText.font);
            CreateTime(hudInstance.m_healthText.color, hudInstance.m_healthText.font);
        }

        /// <summary>
        /// Creates a day game object to display the day in a text component aligned to the left.
        /// It also creates an outline around the text.
        /// </summary>
        private static void CreateDay(Color color, Font font)
		{
            Log("Creating day text...");

            var day = new GameObject("Day")
            {
                layer = 5
            };
            day.transform.SetParent(_panel.transform);

            var rect = day.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 0);
            
            _dayText = day.AddComponent<Text>();
            _dayText.color = color;
            _dayText.font = font;
            _dayText.fontSize = FONT_SIZE;
            _dayText.enabled = true;
            _dayText.alignment = TextAnchor.MiddleLeft;

            var outline = day.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, -1);
            outline.useGraphicAlpha = true;
            outline.useGUILayout = true;
            outline.enabled = true;

            Log("Day created!");
        }

        /// <summary>
        /// Creates a time game object to display the time in a text component aligned to the right.
        /// It also creates an outline around the text.
        /// </summary>
        private static void CreateTime(Color color, Font font)
        {
            Log("Creating time text...");

            var time = new GameObject("Time")
            {
                layer = 5
            };

            time.transform.SetParent(_panel.transform);

            var rect = time.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(90, 0);

            _timeText = time.AddComponent<Text>();
            _timeText.color = color;
            _timeText.font = font;
            _timeText.fontSize = FONT_SIZE;
            _timeText.enabled = true;
            _timeText.alignment = TextAnchor.MiddleRight;

            var outline = time.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, -1);
            outline.useGraphicAlpha = true;
            outline.useGUILayout = true;
            outline.enabled = true;

            Log("Time created!");
        }

        /// <summary>
        /// Updates text component with the current day.
        /// </summary>
        private void UpdateDay()
		{
			_dayText.text = GetCurrentDayText();
		}

        /// <summary>
        /// Updates text component with the current time.
        /// </summary>
        private void UpdateTime()
        {
            _timeText.text = GetCurrentTimeText();
		}

        /// <summary>
        /// Get the current day.
        /// </summary>
        /// /// <returns>
        /// A localized text with the day.
        /// </returns>
		private string GetCurrentDayText()
		{
			if (!EnvMan.instance || Localization.instance == null) return null;

			var day = (int)typeof(EnvMan).GetMethod("GetCurrentDay", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(EnvMan.instance, null);

			return Localization.instance.Localize("$msg_newday", day.ToString());
		}

        /// <summary>
        /// Get the current time of the day.
        /// </summary>
        /// <returns>
        /// The time in 24-hour notation or 12-hour notation if config is set to true.
        /// </returns>
		private string GetCurrentTimeText()
		{
			if (!EnvMan.instance) return null;

			var fraction = (float)typeof(EnvMan).GetField("m_smoothDayFraction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(EnvMan.instance);

			var hour = (int)(fraction * 24);
			var minute = (int)((fraction * 24 - hour) * 60);
			var second = (int)((((fraction * 24 - hour) * 60) - minute) * 60);

			DateTime date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second);

			return date.ToString(_twentyFourHourClock.Value ? "HH:mm" : "hh:mm tt");
		}

        /// <summary>
        /// Log a message to the console.
        /// </summary>
        public static void Log(string message)
		{
            Debug.Log($"{ID}: {message}");
		}
	}
}
