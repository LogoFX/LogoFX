// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Async
{
    public delegate void ResultCallback<T>(AsyncResponceEventArgs<T> ev);
    public delegate void ResultCallback(AsyncResponceEventArgs ev);
}
