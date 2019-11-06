using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using log4net;
using RabbitMQFramework.RabbitConfig;
using RabbitMQ.Client.Events;

namespace RabbitMQFramework.RabbitQueue
{
   
    public class RabbitMQQueue
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RabbitMQQueue));
        private string className = typeof(RabbitMQQueue).Name;
        RabbitConfig.RabbitConfig config;
        IConnection conexao;
        IBasicProperties propriedades;
        PublicationAddress endpoint;
        IModel model;

        public RabbitMQQueue(RabbitConfig.RabbitConfig _config)
        {
            this.config = _config;
            CreateConexao();
            CreateQueue();
            CreateExchange();
            CreateBinding();
            CreateEstructureMessage();
        }

        private bool CreateEstructureMessage()
        {
            try
            {
                logger.DebugFormat("[{0}] Criando Estrutura para Mensagens...", className);
                bool create = false;
                propriedades = model.CreateBasicProperties();
                propriedades.Persistent = true;
                endpoint = new PublicationAddress(ExchangeTypeString(config.exchange.type), config.exchange.nome, config.binding.routingKey);
                create = true;
                logger.DebugFormat("[{0}] Estrutura de Mensagem Criada!", className);
                return create;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("[{0}] {1}", className, ex.ToString());
                return false;
            }
        }

        private bool CreateConexao()
        {
            try
            {
                logger.DebugFormat("[{0}] Criando conexão...", className);
                bool conected = false;
                conexao = config.cf.CreateConnection();
                model = conexao.CreateModel();
                conected = true;
                logger.DebugFormat("[{0}] Conexao Criada!", className);
                return conected;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("[{0}] {1}", className, ex.ToString());
                return false;
            }

           
        }

        private bool CreateQueue()
        {
            try
            {
                logger.DebugFormat("[{0}] Criando Fila...", className);
                bool create = false;
                model.QueueDeclare(config.queue.nome, config.queue.duravel, config.queue.exclusivo, config.queue.AutoExclusao, config.queue.MetaDados);
                create = true;
                logger.DebugFormat("[{0}] Fila Criada!", className);
                return create;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("[{0}] {1}", className, ex.ToString());
                return false;
            }

           
        }

        private string ExchangeTypeString( TypeExchange _type )
        {
            string type = "";

            switch (_type)
            {
                case RabbitConfig.TypeExchange.Direct:
                    type = ExchangeType.Direct;
                    break;
                case RabbitConfig.TypeExchange.Fanout:
                    type = ExchangeType.Fanout;
                    break;
                case RabbitConfig.TypeExchange.Topic:
                    type = ExchangeType.Topic;
                    break;
            }

            return type;
        }

        private bool CreateExchange()
        {
            try
            {
                logger.DebugFormat("[{0}] Criando Exchange...", className);
                bool created = false;

                model.ExchangeDeclare(config.exchange.nome, ExchangeTypeString(config.exchange.type), config.exchange.duravel, config.exchange.AutoExclusao, config.exchange.MetaDados);
                created = true;
                logger.DebugFormat("[{0}] Exchange Criada!", className);

                return created;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("[{0}] {1}", className, ex.ToString());
                return false;
            }

           
        }

        private bool CreateBinding()
        {
            try
            {
                logger.DebugFormat("[{0}] Criando Binding...", className);

                bool created = false;

                model.QueueBind(config.binding.nomeQueue, config.binding.nomeExchange, config.binding.routingKey, config.binding.metdadados);

                created = true;

                logger.DebugFormat("[{0}] Binding Criada!", className);

                return created;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("[{0}] {1}", className, ex.ToString());
                return false;
            }

          
        }

        public bool SentMessageBasic(string mensagem)
        {
            if (this.endpoint != null && this.propriedades != null && mensagem != null)
            {
                return BasicPublishMessage(this.endpoint, this.propriedades, mensagem);
            }

            return false;
        }

        private bool BasicPublishMessage(PublicationAddress endPoint, IBasicProperties propriedades, string mensagem )
        {
            
            try
            {
                logger.DebugFormat("[{0}] Enviando Mensagem...", className);

                bool created = false;

                byte[] _mensagem = Encoding.UTF8.GetBytes(mensagem);

                model.BasicPublish(endPoint, propriedades, _mensagem);

                created = true;

                logger.DebugFormat("[{0}] Mensagem Enviada!!", className);

                return created;

            }
            catch (Exception ex)
            {
                logger.ErrorFormat("[{0}] {1}", className, ex.ToString());
                return false;
            }

        }

        public string ConsumeQueue()
        {
            var consumer = new EventingBasicConsumer(this.model);
            var msg = "";
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                msg = message;
            };
            this.model.BasicConsume(queue: this.config.queue.nome,
                                     autoAck:true,
                                     consumer: consumer);

            
            return msg;

        }
    }
}
