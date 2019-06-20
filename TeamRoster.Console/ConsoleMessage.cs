using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.App
{
    public class ConsoleMessage
    {
        private static int _defaultMessageTimeout = 1000;

        /// <summary>
        /// Shows the message on the console.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="messageTimeout">The message timeout controls how long the message stays on screen.
        /// This parameter is optional, omit it to use the default setting</param>
        /// <remarks>The default message timeout is 1 second</remarks>
        public static void ShowMessage(string message, int? messageTimeout = null)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            System.Threading.Thread.Sleep(messageTimeout ?? _defaultMessageTimeout);
        }

    }
}
