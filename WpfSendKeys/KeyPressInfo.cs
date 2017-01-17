using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WpfSendKeys
{
    public class KeyPressInfo
    {
		private static KeyConverter _keyConverter = new KeyConverter();
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
			//Special keys we must map manually
			if (key == Key.Space) return " ";
			else if (key == Key.Enter && modifiers == ModifierKeys.None) return "\n";
			else if (key == Key.OemComma && modifiers == ModifierKeys.None) return ",";
			else if (key == Key.OemComma && modifiers == ModifierKeys.Shift) return "<";
			else if (key == Key.OemPeriod && modifiers == ModifierKeys.None) return ".";
			else if (key == Key.OemPeriod && modifiers == ModifierKeys.Shift) return ">";
			else if (key == Key.OemQuestion && modifiers == ModifierKeys.None) return "/";
			else if (key == Key.OemQuestion && modifiers == ModifierKeys.Shift) return "?";
			else if (key == Key.OemMinus && modifiers == ModifierKeys.None) return "-";
			else if (key == Key.OemMinus && modifiers == ModifierKeys.Shift) return "_";
			else if (key == Key.OemPlus && modifiers == ModifierKeys.None) return "=";
			else if (key == Key.OemPlus && modifiers == ModifierKeys.Shift) return "+";
			else if (key == Key.OemOpenBrackets && modifiers == ModifierKeys.None) return "[";
			else if (key == Key.OemOpenBrackets && modifiers == ModifierKeys.Shift) return "{";
			else if (key == Key.Oem6 && modifiers == ModifierKeys.None) return "]";
			else if (key == Key.Oem6 && modifiers == ModifierKeys.Shift) return "}";
			else if (key == Key.Oem5 && modifiers == ModifierKeys.None) return "\\";
			else if (key == Key.Oem5 && modifiers == ModifierKeys.Shift) return "|";
			else if (key == Key.Oem1 && modifiers == ModifierKeys.None) return ";";
			else if (key == Key.Oem1 && modifiers == ModifierKeys.Shift) return ":";
			else if (key == Key.Oem3 && modifiers == ModifierKeys.None) return "`";
			else if (key == Key.Oem3 && modifiers == ModifierKeys.Shift) return "~";
			else if (key == Key.OemQuotes && modifiers == ModifierKeys.None) return "\"";
			else if (key == Key.OemQuotes && modifiers == ModifierKeys.Shift) return "\"";
			else if (key == Key.D1 && modifiers == ModifierKeys.Shift) return "!";
			else if (key == Key.D2 && modifiers == ModifierKeys.Shift) return "@";
			else if (key == Key.D3 && modifiers == ModifierKeys.Shift) return "#";
			else if (key == Key.D4 && modifiers == ModifierKeys.Shift) return "$";
			else if (key == Key.D5 && modifiers == ModifierKeys.Shift) return "%";
			else if (key == Key.D6 && modifiers == ModifierKeys.Shift) return "^";
			else if (key == Key.D7 && modifiers == ModifierKeys.Shift) return "&";
			else if (key == Key.D8 && modifiers == ModifierKeys.Shift) return "*";
			else if (key == Key.D9 && modifiers == ModifierKeys.Shift) return "(";
			else if (key == Key.D0 && modifiers == ModifierKeys.Shift) return ")";
			//Special keys that don't have visible output
			else if (key == Key.Left && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Up && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Right && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Down && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Home && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.End && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Insert && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Delete && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.PageUp && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.PageDown && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Back && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Escape && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Enter && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Tab && modifiers == ModifierKeys.None) return String.Empty;
			else if (key == Key.Space && modifiers == ModifierKeys.None) return String.Empty;

			//Map regular keys back to their string form
			return _keyConverter.ConvertToInvariantString(key);
		}
	}
}