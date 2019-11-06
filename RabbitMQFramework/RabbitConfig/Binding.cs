using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQFramework.RabbitConfig
{
    public class Binding
    {
        public string nomeQueue { get; set; }
        public string nomeExchange { get; set; }
        public string routingKey { get; set; }
        public IDictionary<string, object> metdadados { get; set; }
    }
}
