// Copyright 2016-2017 Confluent Inc., 2015-2016 Andreas Heider
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Derived from: rdkafka-dotnet, licensed under the 2-clause BSD License.
//
// Refer to LICENSE for more information.

using Confluent.Kafka;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;


namespace Confluent.Kafka.Examples.ProducerExample
{
    public class Program
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();  

            char letter;  

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);  
            }
            return str_build.ToString();
        }

        public static Task Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: .. brokerList key secret topicName");
                return Task.CompletedTask;
            }

            string topicName = args[3];

            var config = new ProducerConfig
            {
                BootstrapServers = args[0],
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                // Note: If your root CA certificates are in an unusual location you
                // may need to specify this using the SslCaLocation property.
                SaslUsername = args[1],
                SaslPassword = args[2]
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var cancelled = false;
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };

                try
                {
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    int count = 0;
                    while (s.Elapsed < TimeSpan.FromMinutes(15) && !cancelled) 
                    {
                        // Note: Awaiting the asynchronous produce request below prevents flow of execution
                        // from proceeding until the acknowledgement from the broker is received (at the 
                        // expense of low throughput).
                        string val = RandomString(5);
                        string key = count.ToString();
                        producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = val + " "+ count +" elapsed["+s.Elapsed+"]"}).
                        ContinueWith(task => task.IsFaulted
                                ? $"error producing message: {task.Exception.Message}"
                                : $"produced to: {task.Result.TopicPartitionOffset}");

                        count++;

                        Thread.Sleep(30);
                    }

                    s.Stop();

                }
                catch (ProduceException<string, string> e)
                {
                    Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                }
                // block until all in-flight produce requests have completed (successfully
                // or otherwise) or 10s has elapsed.
                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return Task.CompletedTask;
        }
    }
}
