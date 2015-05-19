using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
namespace CommTool.Service
{
    public class DServiceContainer : IServiceContainer, IServiceProvider
    {
        private System.IServiceProvider parentProvider;
        private System.Collections.Hashtable services;
        private static TraceSwitch TRACESERVICE;
        private IServiceContainer Container
        {
            get
            {
                IServiceContainer result = null;
                if (this.parentProvider != null)
                {
                    result = (IServiceContainer)this.parentProvider.GetService(typeof(IServiceContainer));
                }
                return result;
            }
        }
        protected System.Collections.Hashtable Services
        {
            get
            {
                if (this.services == null)
                {
                    this.services = new System.Collections.Hashtable();
                }
                return this.services;
            }
        }
        public System.IServiceProvider ParentProvider
        {
            get
            {
                return this.parentProvider;
            }
            set
            {
                this.parentProvider = value;
            }
        }
        static DServiceContainer()
        {
            DServiceContainer.TRACESERVICE = new TraceSwitch("TRACESERVICE", "ServiceProvider: Trace service provider requests.");
        }
        public DServiceContainer()
        {
        }
        public DServiceContainer(System.IServiceProvider parentProvider)
        {
            this.parentProvider = parentProvider;
        }
        public void AddService(System.Type serviceType, ServiceCreatorCallback callback)
        {
            this.AddService(serviceType, callback, false);
        }
        public void AddService(System.Type serviceType, object serviceInstance)
        {
            this.AddService(serviceType, serviceInstance, false);
        }
        public void AddService(System.Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            if (promote)
            {
                IServiceContainer container = this.Container;
                if (container != null)
                {
                    container.AddService(serviceType, callback, promote);
                    return;
                }
            }
            if (serviceType == null)
            {
                throw new System.ArgumentNullException("serviceType");
            }
            if (callback == null)
            {
                throw new System.ArgumentNullException("callback");
            }
            if (this.Services.ContainsKey(serviceType))
            {
                string arg_4D_0 = serviceType.FullName;
                throw new System.ArgumentException("ErrorServiceExists", "serviceType");
            }
            this.Services[serviceType] = callback;
        }
        public void AddService(System.Type serviceType, object serviceInstance, bool promote)
        {
            if (promote)
            {
                IServiceContainer container = this.Container;
                if (container != null)
                {
                    container.AddService(serviceType, serviceInstance, promote);
                    return;
                }
            }
            if (serviceType == null)
            {
                throw new System.ArgumentNullException("serviceType");
            }
            if (serviceInstance == null)
            {
                throw new System.ArgumentNullException("serviceInstance");
            }
            if (!(serviceInstance is ServiceCreatorCallback) && !serviceInstance.GetType().IsCOMObject && !serviceType.IsAssignableFrom(serviceInstance.GetType()))
            {
                string arg_62_0 = serviceType.FullName;
                throw new System.ArgumentException("ErrorInvalidServiceInstance");
            }
            if (this.Services.ContainsKey(serviceType))
            {
                string arg_82_0 = serviceType.FullName;
                throw new System.ArgumentException("ErrorServiceExists", "serviceType");
            }
            this.Services[serviceType] = serviceInstance;
        }
        public object GetService(System.Type serviceType)
        {
            object obj;
            if (serviceType == typeof(IServiceContainer))
            {
                obj = this;
            }
            else
            {
                obj = this.Services[serviceType];
            }
            if (obj is ServiceCreatorCallback)
            {
                obj = ((ServiceCreatorCallback)obj)(this, serviceType);
                if (obj != null && !obj.GetType().IsCOMObject && !serviceType.IsAssignableFrom(obj.GetType()))
                {
                    obj = null;
                }
            }
            if (obj == null && this.parentProvider != null)
            {
                obj = this.parentProvider.GetService(serviceType);
            }
            return obj;
        }
        public void RemoveService(System.Type serviceType)
        {
            this.RemoveService(serviceType, false);
        }
        public virtual void RemoveService(System.Type serviceType, bool promote)
        {
            if (promote)
            {
                IServiceContainer container = this.Container;
                if (container != null)
                {
                    container.RemoveService(serviceType, promote);
                    return;
                }
            }
            if (serviceType == null)
            {
                throw new System.ArgumentNullException("serviceType");
            }
            this.Services.Remove(serviceType);
        }
    }
}
