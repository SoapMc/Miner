using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Management.Exceptions
{
    public class ExecutionException : Exception
    {
        public ExecutionException() { }
        public ExecutionException(string message) : base(message) { }
        public ExecutionException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidEventArgsException : Exception
    {
        public InvalidEventArgsException() { }
        public InvalidEventArgsException(string message) : base(message) { }
        public InvalidEventArgsException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class InvalidSaveStateException : Exception
    {
        public InvalidSaveStateException() { }
        public InvalidSaveStateException(string message) : base(message) { }
        public InvalidSaveStateException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class InvalidSettingException : Exception
    {
        public InvalidSettingException() { }
        public InvalidSettingException(string message) : base(message) { }
        public InvalidSettingException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class PartNotFoundException : Exception
    {
        public PartNotFoundException() { }
        public PartNotFoundException(string message) : base(message) { }
        public PartNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class WorldGenerationException : Exception
    {
        public WorldGenerationException() { }
        public WorldGenerationException(string message) : base(message) { }
        public WorldGenerationException(string message, System.Exception inner) : base(message, inner) { }
    }
}