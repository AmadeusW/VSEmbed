using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VSEmbed.Contracts
{
    public interface IEmbeddedTextViewHost
	{
		void SetContentType(string contentType);
		void SendKey(Key key, ModifierKeys modifiers = ModifierKeys.None);
		void SendKeystrokes(string input);
		void Show();
		void Close();
	}
}
