using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    public static class Factory
    {
        private static System.IServiceProvider _currentServiceProvider;   //微軟提供的Provider
        public static System.IServiceProvider CurrentServiceProvider
        {
            get
            {
                return Factory._currentServiceProvider;
            }
            set
            {
                Factory._currentServiceProvider = value;
            }
        }
        public static object GetService(System.Type serviceType)
        {
            if (serviceType == null)
            {
                throw new System.ArgumentNullException("serviceType");
            }
            if (Factory._currentServiceProvider == null)
            {
                //throw new DesignException(Resources.CurrentServiceProvidertIsNull);
            }
            object service = Factory._currentServiceProvider.GetService(serviceType);
            if (service == null)
            {
                //throw new DesignException(string.Format(Resources.FailGetService, serviceType.FullName));
            }
            return service;
        }
        public static T GetService<T>()
        {
            if (Factory._currentServiceProvider == null)
            {
                //throw new DesignException(Resources.CurrentServiceProvidertIsNull);
            }
            object service = Factory._currentServiceProvider.GetService(typeof(T));
            if (service == null)
            {
                //throw new DesignException(string.Format(Resources.FailGetService, typeof(T).FullName));
            }
            return (T)((object)service);
        }
        public static object GetServiceWithoutException(System.Type serviceType)
        {
            if (Factory._currentServiceProvider != null)
            {
                return Factory._currentServiceProvider.GetService(serviceType);
            }
            return null;
        }
    }
}
