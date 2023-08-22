using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProResp3.CustomEventArgs
{
    public class MvvmMessageBoxEventArgs : EventArgs
    {
        Action<MessageBoxResult> resultAction;
        string messageBoxText;
        string caption;
        MessageBoxButton button;
        MessageBoxImage icon;
        MessageBoxResult defaultResult;
        MessageBoxOptions options;

        public MvvmMessageBoxEventArgs(Action<MessageBoxResult> resultAction, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            this.resultAction = resultAction;
            this.messageBoxText = messageBoxText;
            this.caption = caption;
            this.button = button;
            this.icon = icon;
            this.defaultResult = defaultResult;
            this.options = options;
        }

        public void Show(Window owner)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult, options);
            if (resultAction != null) resultAction(messageBoxResult);
        }

        public void Show()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
            if (resultAction != null) resultAction(messageBoxResult);
        }
    }
}
