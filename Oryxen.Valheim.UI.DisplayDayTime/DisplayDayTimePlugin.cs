﻿using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Oryxen.Valheim.UI.DisplayDayTime
{
    [BepInPlugin(ID, "Display Day & Time in HUD", "1.1.0")]
    [BepInProcess("valheim.exe")]
    public class DisplayDayTimePlugin : BaseUnityPlugin
    {
        public const string ID = "oryxen.valheim.ui.displaydaytime";

        #region BepInEx configs
        private static ConfigEntry<bool> _displayUnderMiniMap;
        private static ConfigEntry<bool> _displayDay;
        private static ConfigEntry<bool> _displayTime;
        private static ConfigEntry<bool> _displayBackground;
        private static ConfigEntry<bool> _twentyFourHourClock;
        private static ConfigEntry<int> _fontSize;
        private static ConfigEntry<string> _fontName;
        private static ConfigEntry<Color> _fontColor;
        private static ConfigEntry<Color> _backgroundColor;
        #endregion

        private const string DEFAULT_FONT = "AveriaSansLibre-Bold";

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
            _displayDay = Config.Bind("General", "Display day", true, "Display day");
            _displayTime = Config.Bind("General", "Display time", true, "Display time");
            _displayBackground = Config.Bind("General", "Display background", false, "Display background");
            _twentyFourHourClock = Config.Bind("General", "24-hour clock", true, "24-hour clock");
            _fontSize = Config.Bind("General", "Font size", 16, "Font size");
            _fontName = Config.Bind("General", "Font name", DEFAULT_FONT, "Font name");
            _fontColor = Config.Bind("General", "Font color", new Color(1, 1, 1, 0.791f), "Font color");
            _backgroundColor = Config.Bind("General", "Background color", new Color(0, 0, 0, 0.3921569f), "Background color");

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
            rect.anchoredPosition = new Vector2(-140, _displayUnderMiniMap.Value ? -255 : -25);
            var image = _panel.GetComponent<Image>();
            image.enabled = _displayBackground.Value;
            image.color = _backgroundColor.Value;

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
            rect.anchoredPosition = new Vector2(-140, _displayUnderMiniMap.Value ? -255 : - 25);
            rect.sizeDelta = new Vector2(200, 30);

            var image = _panel.AddComponent<Image>();
            image.enabled = _displayBackground.Value;
            image.color = _backgroundColor.Value;

            Log("Panel created!");

            CreateDay();
            CreateTime();
        }

        /// <summary>
        /// Creates a day game object to display the day in a text component aligned to the left.
        /// It also creates an outline around the text.
        /// </summary>
        private static void CreateDay()
		{
            Log("Creating day text...");

            var day = new GameObject("Day")
            {
                layer = 5
            };
            day.transform.SetParent(_panel.transform);

            var rect = day.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-46, 0);
            rect.sizeDelta = new Vector2(90, 30);
            
            _dayText = day.AddComponent<Text>();
            _dayText.color = _fontColor.Value;
            _dayText.font = GetFont();
            _dayText.fontSize = _fontSize.Value;
            _dayText.enabled = _displayDay.Value;
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
        private static void CreateTime()
        {
            Log("Creating time text...");

            var time = new GameObject("Time")
            {
                layer = 5
            };

            time.transform.SetParent(_panel.transform);

            var rect = time.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(46, 0);
            rect.sizeDelta = new Vector2(90, 30);

            _timeText = time.AddComponent<Text>();
            _timeText.color = _fontColor.Value;
            _timeText.font = GetFont();
            _timeText.fontSize = _fontSize.Value;
            _timeText.enabled = _displayTime.Value;
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
        /// It also updates the configured font, font size and color
        /// </summary>
        private void UpdateDay()
		{
            if (_dayText.font.name != _fontName.Value)
            {
                _dayText.font = GetFont();
            }
            _dayText.color = _fontColor.Value;
            _dayText.fontSize = _fontSize.Value;
            _dayText.text = GetCurrentDayText();
		}

        /// <summary>
        /// Updates text component with the current time.
        /// It also updates the configured font, font size and color
        /// </summary>
        private void UpdateTime()
        {
            if (_timeText.font.name != _fontName.Value)
			{
                _timeText.font = GetFont();
            }
            _timeText.color = _fontColor.Value;
            _timeText.fontSize = _fontSize.Value;
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
        /// Get the configured font from resources.
        /// </summary>
        public static Font GetFont()
		{
            var fonts = Resources.FindObjectsOfTypeAll<Font>();

            var font = fonts.FirstOrDefault(f => f.name == _fontName.Value);

            if (font == null)
            {
                return fonts.FirstOrDefault(f => f.name == DEFAULT_FONT);
            }

            return font;
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