﻿using Google.Protobuf;
using System.IO;

namespace Rabbitmq
{
    static class Utils
    {
        public static T ToObject<T>(this byte[] buf) where T : IMessage<T>, new()
        {
            if (buf == null)
                return default(T);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(buf, 0, buf.Length);
                ms.Seek(0, SeekOrigin.Begin);

                MessageParser<T> parser = new MessageParser<T>(() => new T());
                return parser.ParseFrom(ms);
            }
        }
    }
}
