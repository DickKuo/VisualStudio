using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    internal class ServiceEntry : IServiceEntry
    {
        private System.Type _ServiceClass;
        private System.Type _ServiceInterface;
        private ServiceCreateType _ServiceCreateType;
        public System.Type ServiceClass
        {
            get
            {
                return this._ServiceClass;
            }
        }
        public ServiceCreateType ServiceCreateType
        {
            get
            {
                return this._ServiceCreateType;
            }
        }
        public System.Type ServiceInterface
        {
            get
            {
                return this._ServiceInterface;
            }
        }
        public ServiceEntry(System.Type pServiceInterface, System.Type pServiceClass, ServiceCreateType pServiceCreateType)
        {
            this._ServiceCreateType = pServiceCreateType;
            this._ServiceClass = pServiceClass;
            this._ServiceInterface = pServiceInterface;
        }
    }
}
