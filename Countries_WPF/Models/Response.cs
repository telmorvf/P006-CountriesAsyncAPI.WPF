using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countries_WPF.Models
{
    /// <summary>
    /// I gonna use this to see itf all ok with the connection
    /// </summary>
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }
}
