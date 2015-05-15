// Partial Copyright (c) LogoUI Software Solutions LTD
// Author: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;

namespace LogoFX.UI.Data.Async
{    
    public class Error
    {
        private readonly string _description;
        private readonly Exception _exception;
        
        public Error(string description, Exception exception)
        {
            _exception = exception;
            _description = description;
        }
       
        public string Description
        {
            get { return _description; }
        }
        
        public Exception Exception
        {
            get { return _exception; }
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description)?"Error occured":Description;
        }
    }
}