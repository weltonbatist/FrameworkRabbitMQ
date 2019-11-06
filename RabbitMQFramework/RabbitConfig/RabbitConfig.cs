using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQFramework.RabbitConfig
{
    public class RabbitConfig
    {
        public ConnectionFactory cf { get; set; }
        public Queue queue { get; set; }
        public Exchange exchange { get; set; }
        public Binding binding { get; set; }
        public IConnection conexao { get; set; }
        public IBasicProperties propriedades { get; set; }
        public PublicationAddress endpoint { get; set; }
    }
}
