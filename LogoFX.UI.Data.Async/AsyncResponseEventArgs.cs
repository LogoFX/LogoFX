// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;

namespace LogoFX.UI.Data.Async
{    
    public class AsyncResponceEventArgs : EventArgs
    {
        private Error _error;
        
        public AsyncResponceEventArgs() { }
        
        public AsyncResponceEventArgs(string error, Exception exception)
        {
            _error = new Error(error, exception);
        }

        public Error Error
        {
            get { return _error; }
            set { _error = value; }
        }
    }
    
    public sealed class AsyncResponceEventArgs<T> : AsyncResponceEventArgs
    {
        private T _responce = default(T);
        private object _rawResult = null;
        public T Responce
        {
            get { return _responce; }
            set { _responce = value; }
        }
        public object RawResult
        {
            get { return _rawResult; }
            set { _rawResult = value; }
        }
        public AsyncResponceEventArgs():base()
        {
        }
        public AsyncResponceEventArgs(T responce)
        {
            _responce = responce;
        }

        public AsyncResponceEventArgs(string message, Exception e)
            : base(message, e)
        {

        }
    }
}
