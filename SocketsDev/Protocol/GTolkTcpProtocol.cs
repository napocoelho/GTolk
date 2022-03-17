using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SocketsDev.Protocol
{
    public static class ExtensionsHelper
    {
        public static bool HasMore<T>(this Queue<T> queue)
        {
            return (queue.Count > 0);
        }

        public static bool HasMore(this Queue queue)
        {
            return (queue.Count > 0);
        }

        public static T[] DequeueRange<T>(this Queue<T> queue, int lastOnesCount)
        {
            List<T> range = new List<T>();

            int counter = lastOnesCount;

            while (queue.HasMore() && counter >= 0)
            {
                range.Add(queue.Dequeue());
                counter--;
            }

            return range.ToArray();
        }

        public static byte[] ToByteArray(this BitArray bitArray)
        {
            byte[] byteArray = new byte[(int)Math.Ceiling((double)bitArray.Length / 8)];
            bitArray.CopyTo(byteArray, 0);
            return byteArray;
        }

        public static BitArray ToBitArray(this byte[] byteArray)
        {
            return new BitArray(byteArray);
        }

        /*
        public static ProtocolActionEnum ToProtocolActionEnum(this byte value)
        {
            int convertedValue = (int)value;
            ProtocolActionEnum action = (ProtocolActionEnum)Enum.ToObject(typeof(ProtocolActionEnum), convertedValue);
            return action;
        }

        public static byte ToByte(this ProtocolActionEnum action)
        {
            byte convertedValue = (byte)action.GetHashCode();
            return convertedValue;
        }
        */

        /*
        public static List<byte[]> Partition(this byte[] body, ulong equalLenghtPartsOf = UInt16.MaxValue)
        {
            List<byte[]> parts = new List<byte[]>();

            List<byte> part = new List<byte>();
            ulong partCounter = 0;

            for (int idx = 0; idx < body.Length; )
            {
                part.Add(body[idx]);

                idx++;
                partCounter++;

                // Se atingir o tamanho máximo permitido para cada parte ou não houver mais itens, adiciona a parte à lista de partes:
                if (partCounter == equalLenghtPartsOf || !(idx < body.Length))
                {
                    parts.Add(part.ToArray());
                    parts.Clear();
                }
            }

            return parts;
        }
        */

        public static List<byte[]> Partition(this byte[] body, ulong equalLenghtPartsOf = UInt16.MaxValue)
        {
            List<byte[]> parts = new List<byte[]>();
            Queue<byte> fullBodyQueue = new Queue<byte>(body);

            while (fullBodyQueue.HasMore())
            {
                parts.Add(fullBodyQueue.DequeueRange(ushort.MaxValue));
            }

            return parts;
        }
    }

    /*
    public class ProtocolMessage
    {
        public ProtocolActionEnum Action { get; set; }
    }

    public class TextMessage : ProtocolMessage
    {
        public string Value { get; set; }
    }

    public class FileMessage : ProtocolMessage
    {
        public byte[] Value { get; set; }
    }

    public class TypingMessage : ProtocolMessage
    {
        public bool Value { get; set; }
    }

    public class StatusMessage : ProtocolMessage
    {
        public int Value { get; set; }
    }

    public enum ProtocolActionEnum
    {
        Text = 0,
        File = 1,
        Typing = 2,
        Status = 3
    }

    public enum StatusDoContatoEnum
    {
        Offline = 0,
        Online = 1,
        Ausente = 2,
        Ocupado = 3
    }
    */

    public static class GTolkTcpProtocol
    {
        public static byte[] ReceiveX(NetworkStream stream)
        {
            List<byte> fullBody = new List<byte>();
            bool hasMoreFrames = true;

            /*
                Este protocolo define que as mensagens podem ser formadas por 1 parte principal ou 1 parte principal com 1 ou mais sub-partes (quando necessário).
                As sub-partes são necessárias quando a mensagem não couber em BODY. Sendo assim, a comunicação terá sempre uma parte principal. As sub-partes serão opcionais.
                Ex.: [ HEADER | BODY [ SUB_HEADER | SUB_BODY ] ]
                        * HEADER = 3 bytes, sendo:
                                        1o byte (boolean)       = determina se o corpo continuará em um novo frame;
                                        2o e 3o bytes (UInt16)  = determina a quantidade de bytes estão contidos em BODY. Se body for muito grande, pode ser dividido em várias partes.
                        * BODY = contém o corpo da comunicação. Seu tamanho é determinado no HEADER;
            */

            /*
                Header:
                1    = hasMoreFrames;        8 bit
                2..3 = frame body length     16 bits (unsigned)
            */



            do
            {
                // Obtendo cabeçalho:
                int HEADER_SIZE = 3;    // in bytes
                byte[] header = new byte[HEADER_SIZE];
                stream.Read(header, 0, HEADER_SIZE);

                hasMoreFrames = BitConverter.ToBoolean(header.Take(1).ToArray(), 0);
                UInt16 bodyLength = BitConverter.ToUInt16(header.Skip(1).ToArray(), 0); // ushort

                // Obtendo corpo:
                byte[] body = new byte[bodyLength];
                int bytesReceived = stream.Read(body, 0, bodyLength);

                // desembaralhada os bits (apenas NEGA os bits):
                // ( obs.: do outro lado (no envio), deve ser feito a mesma coisa )
                body = body.ToBitArray().Not().ToByteArray();

                fullBody.AddRange(body);

            } while (hasMoreFrames);

            return fullBody.ToArray();
        }

        private static void Send(NetworkStream stream, byte[] fullBody)
        {
            BitArray teste = new BitArray(fullBody);

            // dá uma embaralhada nos bits (apenas NEGA os bits):
            // ( obs.: do outro lado (na recepção), deve ser feito a mesma coisa )
            fullBody = fullBody.ToBitArray().Not().ToByteArray();

            Queue<byte[]> bodyQueue = new Queue<byte[]>(fullBody.Partition());
            
            while (bodyQueue.HasMore())
            {
                List<byte> header = new List<byte>();
                byte[] body = bodyQueue.Dequeue();
                bool hasMoreParts = bodyQueue.HasMore();

                //-----------------------------------------------------------------------------------
                // HEADER:
                //-----------------------------------------------------------------------------------

                // has more parts?
                byte[] hasMore = BitConverter.GetBytes(hasMoreParts);
                header.AddRange(hasMore);

                // body length
                byte[] bodyLen = BitConverter.GetBytes(((ushort)body.Count())); // uInt16
                header.AddRange(bodyLen);

                // send header
                stream.Write(header.ToArray(), 0, header.Count());

                //-----------------------------------------------------------------------------------
                // BODY:
                //-----------------------------------------------------------------------------------

                

                // send body
                stream.Write(body, 0, body.Count());

                //-----------------------------------------------------------------------------------

                stream.Flush();
            }
        }
    }
}