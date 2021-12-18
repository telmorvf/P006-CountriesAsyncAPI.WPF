using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Countries_WPF.Services
{
    public class DialogService
    {
        /// <summary>
        /// Default Message Box
        /// </summary>
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
