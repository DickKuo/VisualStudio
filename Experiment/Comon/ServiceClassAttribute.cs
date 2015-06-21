using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ServiceClassAttribute : System.Attribute
    {
        private readonly System.Type _ServiceInterface;
        private readonly ServiceCreateType _ServiceCreateType;
        private readonly bool _enabled;
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
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
        }
        public ServiceClassAttribute(System.Type pServiceInterface, ServiceCreateType pServiceCreateType)
        {
            this._ServiceCreateType = pServiceCreateType;
            this._ServiceInterface = pServiceInterface;
            this._enabled = true;
        }
        public ServiceClassAttribute(bool pEnabled)
        {
            if (pEnabled)
            {
                throw new System.ArgumentOutOfRangeException("pEnabled", "pEnabled must is false.");
            }
            this._enabled = pEnabled;
        }
    }
}
