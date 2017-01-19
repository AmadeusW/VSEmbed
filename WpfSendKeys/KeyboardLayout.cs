using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WpfSendKeys
{
	public class KeyboardLayout
	{
		public static readonly KeyboardLayout Instance = new KeyboardLayout();
		private static KeyConverter _keyCoverter = new KeyConverter();

		public KeyPressInfo GetKeyGestureForChar(char currentChar)
		{
			if (printableChars.TryGetValue(currentChar, out var result))
			{
				return result;
			}

			ModifierKeys modifiers = ModifierKeys.None;
			var ch = currentChar.ToString();
			if (char.IsUpper(ch, 0))
			{
				ch = ch.ToLower();
				modifiers = ModifierKeys.Shift;
			}

			var key = (Key)_keyCoverter.ConvertFromInvariantString(ch);
			return new KeyPressInfo(key, modifiers);
		}

		private static readonly Dictionary<char, KeyPressInfo> printableChars = new Dictionary<char, KeyPressInfo>
		{
			{' ', new KeyPressInfo(Key.Space)},
			{'\n', new KeyPressInfo(Key.Enter)},
			{',', new KeyPressInfo(Key.OemComma)},
			{'<', new KeyPressInfo(Key.OemComma, ModifierKeys.Shift)},
			{'.', new KeyPressInfo(Key.OemPeriod)},
			{'>', new KeyPressInfo(Key.OemPeriod, ModifierKeys.Shift)},
			{'/', new KeyPressInfo(Key.OemQuestion)},
			{'?', new KeyPressInfo(Key.OemQuestion, ModifierKeys.Shift)},
			{'-', new KeyPressInfo(Key.OemMinus)},
			{'_', new KeyPressInfo(Key.OemMinus, ModifierKeys.Shift)},
			{'=', new KeyPressInfo(Key.OemPlus)},
			{'+', new KeyPressInfo(Key.OemPlus, ModifierKeys.Shift)},

			{'[', new KeyPressInfo(Key.OemOpenBrackets)},
			{'{', new KeyPressInfo(Key.OemOpenBrackets, ModifierKeys.Shift)},
			{']', new KeyPressInfo(Key.Oem6)},
			{'}', new KeyPressInfo(Key.Oem6, ModifierKeys.Shift)},

			{'\\', new KeyPressInfo(Key.Oem5)},
			{'|', new KeyPressInfo(Key.Oem5, ModifierKeys.Shift)},
			{';', new KeyPressInfo(Key.Oem1)},
			{':', new KeyPressInfo(Key.Oem1, ModifierKeys.Shift)},
			{'`', new KeyPressInfo(Key.Oem3)},
			// NOTE: We replace tildas with Escape
			//{'~', new KeyPressInfo(Key.Oem3, ModifierKeys.Shift)},
			{'~', new KeyPressInfo(Key.Escape)},

			{'\'', new KeyPressInfo(Key.OemQuotes)},
			{'"', new KeyPressInfo(Key.OemQuotes, ModifierKeys.Shift)},
			{'!', new KeyPressInfo(Key.D1, ModifierKeys.Shift)},
			{'@', new KeyPressInfo(Key.D2, ModifierKeys.Shift)},
			{'#', new KeyPressInfo(Key.D3, ModifierKeys.Shift)},
			{'$', new KeyPressInfo(Key.D4, ModifierKeys.Shift)},
			{'%', new KeyPressInfo(Key.D5, ModifierKeys.Shift)},
			{'^', new KeyPressInfo(Key.D6, ModifierKeys.Shift)},
			{'&', new KeyPressInfo(Key.D7, ModifierKeys.Shift)},
			{'*', new KeyPressInfo(Key.D8, ModifierKeys.Shift)},
			{'(', new KeyPressInfo(Key.D9, ModifierKeys.Shift)},
			{')', new KeyPressInfo(Key.D0, ModifierKeys.Shift)},
		};

	}
}
