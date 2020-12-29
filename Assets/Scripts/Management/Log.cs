using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Miner.Management
{
    public class Log : IDisposable
    {
        private StreamWriter _writer = null;

        public static Log Instance = null;

        public Log()
        {
            if (Instance == null)
            {
                Instance = this;
                _writer = new StreamWriter("errorlog.txt", false);
            }
            else
            {
                Debug.LogWarning("Try of create more than one instance of Log class! This must be avoided!");
                Dispose();
            }
        }

        public void Write(string log)
        {
            _writer.Write("[" + DateTime.Now.ToString() + "] " + log + "\n");
        }

        public void WriteWithStackTrace(string log)
        {
            _writer.Write("[" + DateTime.Now.ToString() + "] " + log + "\nStack Trace:\n" + UnityEngine.StackTraceUtility.ExtractStackTrace() + "\n");
        }

        public void WriteException(Exception exception)
        {
            Debug.LogWarning(exception);
            _writer.Write("[" + DateTime.Now.ToString() + "] " + exception.Message + "\nStack Trace:\n" + UnityEngine.StackTraceUtility.ExtractStackTrace() + "\n");
        }

        public void Dispose()
        {
            _writer.Close();
        }
    }
}