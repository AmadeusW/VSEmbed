﻿using System.Collections.Generic;

namespace System.Windows.Input.Test
{
    public static class SendKeysParser
    {
        public static IEnumerable<KeyPressInfo> Parse(string text)
        {
			List<KeyPressInfo> result = new List<KeyPressInfo>();
            int current = 0;

            while (current < text.Length)
            {
                var key = ParseChar(text[current]);
				if (key.Key != Key.None)
				{
					result.Add(key);
				}
				current++;
            }

            return result;
        }

        private static KeyPressInfo ParseChar(char currentChar)
        {
            var key = Key.None;
            var modifiers = ModifierKeys.None;
            var ch = currentChar.ToString();

            KeyPressInfo knownKeyPress = KeyboardLayout.Instance.GetKeyGestureForChar(currentChar);
            if (knownKeyPress != null)
            {
                key = knownKeyPress.Key;
                modifiers = knownKeyPress.Modifiers;
            }
            else
            {
                if (char.IsUpper(ch, 0))
                {
                    ch = ch.ToLower();
                    modifiers = ModifierKeys.Shift;
                }
                key = (Key)new KeyConverter().ConvertFromInvariantString(ch);
            }

			return new KeyPressInfo(key, modifiers);
        }

		private readonly static Dictionary<string, KeyPressInfo> specialValues = new Dictionary<string, KeyPressInfo>()
		{
			{"+", new KeyPressInfo(Key.OemPlus, ModifierKeys.Shift)},
			{"^", new KeyPressInfo(Key.D6, ModifierKeys.Shift)},
			{"%", new KeyPressInfo(Key.D5, ModifierKeys.Shift)},
			{"{", new KeyPressInfo(Key.OemOpenBrackets, ModifierKeys.Shift)},
			{"}", new KeyPressInfo(Key.Oem6, ModifierKeys.Shift)},
			{"[", new KeyPressInfo(Key.OemOpenBrackets)},
			{"]", new KeyPressInfo(Key.Oem6)},
			{"(", new KeyPressInfo(Key.D9, ModifierKeys.Shift)},
			{")", new KeyPressInfo(Key.D0, ModifierKeys.Shift)},
			{"~", new KeyPressInfo(Key.Oem3, ModifierKeys.Shift)},
			{"BACKSPACE", new KeyPressInfo(Key.Back)},
			{"BS", new KeyPressInfo(Key.Back)},
			{"BKSP", new KeyPressInfo(Key.Back)},
			{"CAPSLOCK", new KeyPressInfo(Key.CapsLock)},
			{"DEL", new KeyPressInfo(Key.Delete)},
			{"DELETE", new KeyPressInfo(Key.Delete)},
			{"DOWN", new KeyPressInfo(Key.Down)},
			{"END", new KeyPressInfo(Key.End)},
			{"ENTER", new KeyPressInfo(Key.Enter)},
			{"ESC", new KeyPressInfo(Key.Escape)},
			{"HELP", new KeyPressInfo(Key.Help)},
			{"HOME", new KeyPressInfo(Key.Home)},
			{"INSERT", new KeyPressInfo(Key.Insert)},
			{"INS", new KeyPressInfo(Key.Insert)},
			{"LEFT", new KeyPressInfo(Key.Left)},
			{"NUMLOCK", new KeyPressInfo(Key.NumLock)},
			{"PGDN", new KeyPressInfo(Key.PageDown)},
			{"PGUP", new KeyPressInfo(Key.PageUp)},
			{"PRTSC", new KeyPressInfo(Key.PrintScreen)},
			{"RIGHT", new KeyPressInfo(Key.Right)},
			{"SCROLLOCK", new KeyPressInfo(Key.Scroll)},
			{"TAB", new KeyPressInfo(Key.Tab)},
			{"UP", new KeyPressInfo(Key.Up)},
			{"F1", new KeyPressInfo(Key.F1)},
			{"F2", new KeyPressInfo(Key.F2)},
			{"F3", new KeyPressInfo(Key.F3)},
			{"F4", new KeyPressInfo(Key.F4)},
			{"F5", new KeyPressInfo(Key.F5)},
			{"F6", new KeyPressInfo(Key.F6)},
			{"F7", new KeyPressInfo(Key.F7)},
			{"F8", new KeyPressInfo(Key.F8)},
			{"F9", new KeyPressInfo(Key.F9)},
			{"F10", new KeyPressInfo(Key.F10)},
			{"F11", new KeyPressInfo(Key.F11)},
			{"F12", new KeyPressInfo(Key.F12)},
			{"F13", new KeyPressInfo(Key.F13)},
			{"F14", new KeyPressInfo(Key.F14)},
			{"F15", new KeyPressInfo(Key.F15)},
			{"F16", new KeyPressInfo(Key.F16)},
		};
	}
}
