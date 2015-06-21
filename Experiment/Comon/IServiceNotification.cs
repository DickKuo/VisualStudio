using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    public interface IServiceNotification
    {
        void Start(string[] args);
        void Pause();
        void Continue();
        void Stop();
    }
}
