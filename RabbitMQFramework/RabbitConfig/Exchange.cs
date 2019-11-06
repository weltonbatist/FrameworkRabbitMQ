using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQFramework.RabbitConfig
{
    public enum TypeExchange
    {
        Direct,
        Fanout,
        Topic
    }

    public class Exchange
    {
        public string nome { get; set; }
        public TypeExchange type { get; set; }
        public bool AutoExclusao { get; set; }
        public IDictionary<string, object> MetaDados { get; set; }
        public bool duravel { get; set; }

    }
}
