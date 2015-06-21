using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    public abstract class ServiceProviderAddin : IServiceProviderAddin
    {
        public System.Collections.Generic.List<IServiceEntry> _ColServiceEntry;
        public IServiceEntry[] ServiceEntries
        {
            get
            {
                return this.GetServiceEntyies();
            }
        }
        public ServiceProviderAddin()
        {
            this._ColServiceEntry = new System.Collections.Generic.List<IServiceEntry>();
        }
        public ServiceProviderAddin(System.Reflection.Assembly pAssembly)
            : this()
        {
            ServiceSearcher serviceSearcher = new ServiceSearcher(pAssembly);
            this._ColServiceEntry.AddRange(serviceSearcher.Search());
        }
        public void RemoveServiceEntry(string pServiceFullName)
        {
            if (pServiceFullName == null)
            {
                throw new System.ArgumentNullException("pServiceFullName");
            }
            if (this._ColServiceEntry != null && this._ColServiceEntry.Count > 0)
            {
                for (int i = 0; i < this._ColServiceEntry.Count; i++)
                {
                    if (this._ColServiceEntry[i].ServiceClass.FullName == pServiceFullName)
                    {
                        this._ColServiceEntry.Remove(this._ColServiceEntry[i]);
                        i--;
                    }
                }
            }
        }
        public void AddServiceEntry(IServiceEntry pServiceEntry)
        {
            if (pServiceEntry == null)
            {
                throw new System.ArgumentNullException("pServiceEntry");
            }
            this._ColServiceEntry.Add(pServiceEntry);
        }
        public void AddServiceEntry(System.Type pServiceInterface, System.Type pServiceClass, ServiceCreateType pServiceCreateType)
        {
            if (pServiceClass == null)
            {
                throw new System.ArgumentNullException("pServiceClass");
            }
            this.AddServiceEntry(new ServiceEntry(pServiceInterface, pServiceClass, pServiceCreateType));   //加入IServiceEntry 型態的List 
        }
        private IServiceEntry[] GetServiceEntyies()
        {
            if (this._ColServiceEntry.Count == 0)
            {
                //throw new DesignException(Resources.NotinitlizationIServiceEntryArray);
            }
            IServiceEntry[] array = new IServiceEntry[this._ColServiceEntry.Count];
            this._ColServiceEntry.CopyTo(array);
            return array;
        }
    }
}
