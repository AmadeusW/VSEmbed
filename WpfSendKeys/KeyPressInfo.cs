using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WpfSendKeys
{
    public class KeyPressInfo
    {
		public string Input { get; }
        public Key Key { get; set; }
        public ModifierKeys Modifiers { get; set; }

        public KeyPressInfo(Key key, ModifierKeys modifierKeys = ModifierKeys.None)
        {
            this.Key = key;
            this.Modifiers = modifierKeys;
			this.Input = getPrintableString(this.Key, this.Modifiers);
        }

        public override string ToString()
        {
            var result = Key.ToString();
            if (Modifiers != ModifierKeys.None)
            {
                result = Modifiers.ToString() + " + " + result;
            }
            return result;
        }

		private static string getPrintableString(Key key, ModifierKeys modifiers)
		{
			char printableCharacter = getPrintableCharacter(key, modifiers);
			if (printableCharacter != default(char))
			{
				return printableCharacter.ToString();
			}
			else
			{
				return String.Empty;
			}
		}

		private static char getPrintableCharacter(Key key, ModifierKeys modifiers)
		{
			if (key == Key.Space) return ' ';
			else if (key == Key.Enter && modifiers == ModifierKeys.None) return '\n';
			else if (key == Key.OemComma && modifiers == ModifierKeys.None) return ',';
			else if (key == Key.OemComma && modifiers == ModifierKeys.Shift) return '<';
			else if (key == Key.OemPeriod && modifiers == ModifierKeys.None) return '.';
			else if (key == Key.OemPeriod && modifiers == ModifierKeys.Shift) return '>';
			else if (key == Key.OemQuestion && modifiers == ModifierKeys.None) return '/';
			else if (key == Key.OemQuestion && modifiers == ModifierKeys.Shift) return '?';
			else if (key == Key.OemMinus && modifiers == ModifierKeys.None) return '-';
			else if (key == Key.OemMinus && modifiers == ModifierKeys.Shift) return '_';
			else if (key == Key.OemPlus && modifiers == ModifierKeys.None) return '=';
			else if (key == Key.OemPlus && modifiers == ModifierKeys.Shift) return '+';
			else if (key == Key.OemOpenBrackets && modifiers == ModifierKeys.None) return '[';
			else if (key == Key.OemOpenBrackets && modifiers == ModifierKeys.Shift) return '{';
			else if (key == Key.Oem6 && modifiers == ModifierKeys.None) return ']';
			else if (key == Key.Oem6 && modifiers == ModifierKeys.Shift) return '}';
			else if (key == Key.Oem5 && modifiers == ModifierKeys.None) return '\\';
			else if (key == Key.Oem5 && modifiers == ModifierKeys.Shift) return '|';
			else if (key == Key.Oem1 && modifiers == ModifierKeys.None) return ';';
			else if (key == Key.Oem1 && modifiers == ModifierKeys.Shift) return ':';
			else if (key == Key.Oem3 && modifiers == ModifierKeys.None) return '`';
			else if (key == Key.Oem3 && modifiers == ModifierKeys.Shift) return '~';
			else if (key == Key.OemQuotes && modifiers == ModifierKeys.None) return '\'';
			else if (key == Key.OemQuotes && modifiers == ModifierKeys.Shift) return '"';
			else if (key == Key.D1 && modifiers == ModifierKeys.Shift) return '!';
			else if (key == Key.D2 && modifiers == ModifierKeys.Shift) return '@';
			else if (key == Key.D3 && modifiers == ModifierKeys.Shift) return '#';
			else if (key == Key.D4 && modifiers == ModifierKeys.Shift) return '$';
			else if (key == Key.D5 && modifiers == ModifierKeys.Shift) return '%';
			else if (key == Key.D6 && modifiers == ModifierKeys.Shift) return '^';
			else if (key == Key.D7 && modifiers == ModifierKeys.Shift) return '&';
			else if (key == Key.D8 && modifiers == ModifierKeys.Shift) return '*';
			else if (key == Key.D9 && modifiers == ModifierKeys.Shift) return '(';
			else if (key == Key.D0 && modifiers == ModifierKeys.Shift) return ')';

			//Couldn't find it
			return default(char);
		}
	}
}