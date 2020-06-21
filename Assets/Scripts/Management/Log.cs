using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Miner.Management
{
    public class Log : IDisposable
    {
        private StreamWriter _writer = null;

        public Log()
        {
            _writer = new StreamWriter("errorlog.txt", false);
        }

        public void Write(string log)
        {
            _writer.Write("[" + DateTime.Now.ToString() + "] " + log + "\n\n");
        }

        public void Dispose()
        {
            _writer.Close();
        }
    }
}