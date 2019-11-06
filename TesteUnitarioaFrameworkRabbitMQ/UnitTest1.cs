using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQFramework.RabbitConfig;
using RabbitMQ.Client;
using RabbitMQFramework.RabbitQueue;
using System.Collections.Generic;

namespace TesteUnitarioaFrameworkRabbitMQ
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CriandoUmaFila()
        {
            RabbitConfig rbtconf = new RabbitConfig();
            RabbitMQQueue rabbitMQ;

            //Conexao
            ConnectionFactory cf = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            //Fila
            Queue queue = new Queue();
            queue.nome = "Fila01";
            queue.exclusivo = false;
            queue.duravel = true;
            queue.AutoExclusao = false;
            System.Collections.Generic.Dictionary<string, object> dicQue = new Dictionary<string, object>();
            dicQue.Add("App-Cript", "225689");
            queue.MetaDados = dicQue;

            //Exchange servidor.Modelo.ExchangeDeclare("exFila01",ExchangeType.Topic);
            Exchange ex = new Exchange();
            ex.nome = "exFila01";
            ex.type = TypeExchange.Topic;
            ex.duravel = true;
            ex.AutoExclusao = false;
            System.Collections.Generic.Dictionary<string, object> dicEx = new Dictionary<string, object>();
            dicEx.Add("Ex-Cript", "crpt");
            ex.MetaDados = dicEx;

            //Bind servidor.Modelo.QueueBind("Fila01","exFila01","ToFila01");
            Binding bd = new Binding();
            bd.nomeQueue = "Fila01";
            bd.nomeExchange = "exFila01";
            bd.routingKey = "ToFila01";
            System.Collections.Generic.Dictionary<string, object> diBd = new Dictionary<string, object>();
            diBd.Add("BD-Cript", "bdrcp");
            bd.metdadados = diBd;

            rbtconf.binding = bd;
            rbtconf.cf = cf;
            rbtconf.exchange = ex;
            rbtconf.queue = queue;

            rabbitMQ = new RabbitMQQueue(rbtconf);

            //Enviando Mensagem

           

            for(int i = 0; i <= 1000000; i++)
            {
                rabbitMQ.SentMessageBasic($"Mensagem da Terra! Algum sinal de vida em Marte? {i.ToString()}/100000");
            }

            string retorno = "";

            while (true)
            {
                try
                {
                    retorno = rabbitMQ.ConsumeQueue();

                    if (retorno != "")
                    {
                        Console.WriteLine(retorno);
                    }
                    else
                    {
                        Console.WriteLine("Mensagem não encontrada...");
                    }
                }
                catch (Exception erro)
                {

                    Console.WriteLine(erro.ToString());
                    Console.ReadLine();
                }
                
            }
            

        }
    }
}
