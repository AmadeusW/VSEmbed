using System.Collections.Generic;
using System.Windows.Input;

namespace WpfSendKeys
{
    public static class SendKeysParser
    {
		private static KeyConverter _keyCoverter = new KeyConverter();
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
                key = (Key)_keyCoverter.ConvertFromInvariantString(ch);
            }

			return new KeyPressInfo(key, modifiers);
        }
	}
}
