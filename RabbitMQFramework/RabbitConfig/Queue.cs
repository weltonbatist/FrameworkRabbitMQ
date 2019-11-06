using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQFramework.RabbitConfig
{
    public class Queue
    {
        public string nome { get; set; }
        public bool duravel { get; set; }
        public bool exclusivo { get; set; }
        public bool AutoExclusao { get; set; }
        public IDictionary<string, object> MetaDados { get; set; }
    }
}
