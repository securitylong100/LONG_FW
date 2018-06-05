using System;

namespace Com.Nidec.Mes.Framework
{
    internal interface UnhandledExceptionHandler
    {

        void HandleException(object sender, UnhandledExceptionEventArgs e);
    }
}
