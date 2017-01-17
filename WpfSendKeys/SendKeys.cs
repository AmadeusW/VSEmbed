using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace WpfSendKeys
{
    public class SendKeys
    {
        public static void Send(UIElement element, string text)
        {
            var sequence = Parse(text);
            foreach (var keyPressInfo in sequence)
            {
                Send(element, keyPressInfo);
            }
        }

        public static void Send(UIElement element, KeyPressInfo keyPressInfo)
        {
            KeyboardDevice keyboardDevice = InputManager.Current.PrimaryKeyboardDevice;
            if (keyPressInfo.Modifiers != ModifierKeys.None)
            {
                MockKeyboardDevice mockKeyboardDevice = MockKeyboardDevice.Instance;
                mockKeyboardDevice.Modifiers = keyPressInfo.Modifiers;
                keyboardDevice = mockKeyboardDevice;
            }
            RaiseKeyEvent(element, keyPressInfo, keyboardDevice);
        }

        public static void SendInput(UIElement element, string text)
        {
            InputManager inputManager = InputManager.Current;
            InputDevice inputDevice = inputManager.PrimaryKeyboardDevice;
            TextComposition composition = new TextComposition(inputManager, element, text);
            TextCompositionEventArgs args = new TextCompositionEventArgs(inputDevice, composition);
            args.RoutedEvent = UIElement.PreviewTextInputEvent;
            element.RaiseEvent(args);
            args.RoutedEvent = UIElement.TextInputEvent;
            element.RaiseEvent(args);
        }

        private static void RaiseKeyEvent(UIElement element, KeyPressInfo keyPressInfo, KeyboardDevice keyboardDevice)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual(element);
            int timestamp = Environment.TickCount;
            KeyEventArgs args = new KeyEventArgs(keyboardDevice, presentationSource, timestamp, keyPressInfo.Key);

            // 1) PreviewKeyDown
            args.RoutedEvent = Keyboard.PreviewKeyDownEvent;
            element.RaiseEvent(args);

            // 2) KeyDown
            args.RoutedEvent = Keyboard.KeyDownEvent;
            element.RaiseEvent(args);

            // 3) TextInput
            SendInputIfNecessary(element, keyPressInfo, keyboardDevice);

            // 4) PreviewKeyUp
            args.RoutedEvent = Keyboard.PreviewKeyUpEvent;
            element.RaiseEvent(args);

            // 5) KeyUp
            args.RoutedEvent = Keyboard.KeyUpEvent;
            element.RaiseEvent(args);
        }

        private static void SendInputIfNecessary(UIElement element, KeyPressInfo keyPressInfo, KeyboardDevice keyboardDevice)
        {
            if (keyPressInfo.Modifiers.HasFlag(ModifierKeys.Control) || keyPressInfo.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                return;
            }

            string input = keyPressInfo.Input;
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (keyPressInfo.Modifiers == ModifierKeys.Shift)
            {
                input = input.ToUpperInvariant();
            }
            else
            {
                input = input.ToLowerInvariant();
            }

            SendInput(element, input);
        }

		private static IEnumerable<KeyPressInfo> Parse(string text)
		{
			var result = new List<KeyPressInfo>();
			int current = 0;

			while (current < text.Length)
			{
				var key = KeyboardLayout.Instance.GetKeyGestureForChar(text[current]);
				if (key.Key != Key.None)
				{
					result.Add(key);
				}
				current++;
			}

			return result;
		}
	}
}
