using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using CommTool.Business.Services;
using CommTool;
using CommTool.Service;
using CommTool.Business.Metadata;



namespace CommTool
{
    public class AssemblyLoader : DServiceContainer, IServiceNotification
    {
        private delegate void DoServiceNotificationDelegate(IServiceNotification obj);
        public const string DEFAULT_ASSEMBLY_SECTIONNAME = "Assemblys";
        private System.Collections.Generic.Dictionary<System.Type, System.Type> _forCreateClass;
        private ServiceCreatorCallback _callback;
        public AssemblyLoader()
        {
            this.InitializeComponent();
        }
        public AssemblyLoader(System.IServiceProvider parentProvider)
            : base(parentProvider)
        {
            this.InitializeComponent();
        }
        private void InitializeComponent()
        {
            this._forCreateClass = new System.Collections.Generic.Dictionary<System.Type, System.Type>();
            this._callback = new ServiceCreatorCallback(this.CreateService);
        }
        public void Load()
        {
            this.Load("Assemblys");
        }
        public void Load(string pSectionName)
        {           

            if (!string.IsNullOrEmpty(pSectionName))
            {
                System.Collections.Generic.IEnumerable<AddinConfigItem> enumerable = ConfigurationManager.GetSection(pSectionName) as System.Collections.Generic.IEnumerable<AddinConfigItem>;
                if (enumerable != null)
                {
                    //ExceptionCollection exceptionCollection = new ExceptionCollection();
                    foreach (AddinConfigItem current in enumerable)
                    {
                        try
                        {                            
                            this.Load(current.DllFile.Trim(), current.AddinClass.Trim());
                            //this.Load(,,ServiceCreateType.Callback);
                        }
                        catch (System.Exception ex2)
                        {
                            System.Reflection.ReflectionTypeLoadException ex = ex2.GetBaseException() as System.Reflection.ReflectionTypeLoadException;
                            if (ex != null)
                            {
                                System.Exception[] loaderExceptions = ex.LoaderExceptions;
                                for (int i = 0; i < loaderExceptions.Length; i++)
                                {
                                    System.Exception item = loaderExceptions[i];
                                    //exceptionCollection.Add(item);
                                }
                            }
                            else
                            {
                                //System.Text.StringBuilder arg = new System.Text.StringBuilder(this.GetLastInnerException(ex2));
                                //if (ex2.Message != Resources.NotinitlizationIServiceEntryArray)
                                //{
                                //    exceptionCollection.Add(new System.Exception(string.Format(Resources.AssemblyLoadFailed, arg, current.DllFile, current.AddinClass)));
                                //}
                            }
                        }
                    }
                    //if (exceptionCollection.Count > 0)
                    //{
                    //    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    //    foreach (System.Exception current2 in exceptionCollection)
                    //    {
                    //        stringBuilder.Append(current2.Message);
                    //    }
                    //    DesignException ex3 = new DesignException(stringBuilder.ToString());
                    //    throw ex3;
                    //}
                }
            }
        }
        private string GetLastInnerException(System.Exception ex)
        {
            if (ex.InnerException != null)
            {
                return this.GetLastInnerException(ex.InnerException);
            }
            return ex.Message;
        }
        public void Load(string pAssemblyFile, string pServiceProviderAddinClass)
        {
            if (string.IsNullOrEmpty(pAssemblyFile))
            {
                //throw new DesignException(Resources.DllFileIsEmptyInAppConfig);
            }
            if (string.IsNullOrEmpty(pServiceProviderAddinClass))
            {
                //throw new DesignException(Resources.addinClassIsEmptyInAppConfig);
            }
            System.Reflection.Assembly assembly = null;
            try
            {
                assembly = System.Reflection.Assembly.LoadFrom(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, pAssemblyFile));
            }
            catch (System.Exception ex)
            {
                //throw new DesignException(string.Format(Resources.AssemblyLoadFailed, ex.Message, pAssemblyFile, pServiceProviderAddinClass), ex);
            }
            object obj = assembly.CreateInstance(pServiceProviderAddinClass);
            if (obj == null)
            {
                //throw new DesignException(string.Format(Resources.ServiceProviderCreateError, pServiceProviderAddinClass));
            }
            IServiceProviderAddin serviceProviderAddin = obj as IServiceProviderAddin;
            if (serviceProviderAddin == null)
            {
                //throw new DesignException(string.Format(Resources.ConvertToIServiceProviderAddinFail, pServiceProviderAddinClass));
            }
            this.Load(serviceProviderAddin);
        }
        public void Load(IServiceProviderAddin serviceProviderAddin)
        {
            IServiceEntry[] serviceEntries = serviceProviderAddin.ServiceEntries;
            if (serviceEntries == null)
            {
                //throw new DesignException(string.Format(Resources.ServiceEntryNullError, serviceProviderAddin));
            }
            IServiceEntry[] array = serviceEntries;
            for (int i = 0; i < array.Length; i++)
            {
                IServiceEntry serviceEntry = array[i];
                this.Load(serviceEntry);
            }
        }
        public void Load(IServiceEntry serviceEntry)
        {
            if (serviceEntry == null)
            {
                throw new System.ArgumentNullException("serviceEntry");
            }
            this.Load(serviceEntry.ServiceInterface, serviceEntry.ServiceClass, serviceEntry.ServiceCreateType);
        }
        public void Load(System.Type serviceInterface, System.Type serviceClass, ServiceCreateType serviceCreateType)
        {
            switch (serviceCreateType)
            {
                case ServiceCreateType.Instance:
                    base.AddService(serviceInterface, this.CreateInstanceByClassType(serviceClass));
                    return;
                case ServiceCreateType.Callback:
                    if (this._forCreateClass.ContainsKey(serviceInterface))
                    {
                        string arg = (serviceInterface == null) ? string.Empty : serviceInterface.FullName;
                        string arg2 = (this._forCreateClass[serviceInterface] == null) ? string.Empty : this._forCreateClass[serviceInterface].FullName;
                        //throw new DesignException(string.Format(Resources.ServiceIsAdded, arg, arg2));
                    }
                    this._forCreateClass.Add(serviceInterface, serviceClass);
                    base.AddService(serviceInterface, this._callback);
                    return;
                case ServiceCreateType.Startup:
                    this.CreateInstanceByClassType(serviceClass);
                    return;
                //default:
                //    throw new DesignException(string.Format(Resources.EnumValueNotDefined, serviceCreateType));
            }
        }
        protected virtual object CreateInstanceByClassType(System.Type pType)
        {
            object obj = null;
            try
            {
                obj = System.Activator.CreateInstance(pType);
            }
            catch (System.Exception ex)
            {
                //throw new DesignException(string.Format(Resources.CreateServiceInstanceFailed, pType.FullName, ex.Message), ex);
            }
            if (obj == null)
            {
                //throw new DesignException(string.Format(Resources.ServiceInstanceIsNUll, pType.FullName));
            }
            return obj;
        }
        private object CreateService(IServiceContainer serviceContainer, System.Type serviceInterface)
        {
            System.Type pType = this._forCreateClass[serviceInterface];
            object result;
            try
            {
                result = this.CreateInstanceByClassType(pType);
            }
            catch (System.Exception ex)
            {
                throw new  Exception(ex.Message);
                //throw new DesignException(string.Format(Resources.CallBackCreateInstanceFailed, ex.Message), ex);
            }
            return result;
        }
        public void Start()
        {
            this.Start(new string[0]);
        }
        public void Start(string[] args)
        {
            this.DoServiceNotification(false, delegate(IServiceNotification service)
            {
                service.Start(args);
            });
        }
        public void Pause()
        {
            this.DoServiceNotification(true, delegate(IServiceNotification service)
            {
                service.Pause();
            });
        }
        public void Continue()
        {
            this.DoServiceNotification(false, delegate(IServiceNotification service)
            {
                service.Continue();
            });
        }
        public void Stop()
        {
            this.DoServiceNotification(false, delegate(IServiceNotification service)
            {
                service.Stop();
            });
        }
        private void DoServiceNotification(bool pParentBefore, AssemblyLoader.DoServiceNotificationDelegate pDelegate)
        {
            if (pParentBefore)
            {
                this.DoParentServiceNotification(pDelegate);
            }
            System.Collections.Hashtable services = base.Services;
            foreach (System.Collections.DictionaryEntry dictionaryEntry in services)
            {
                System.Type serviceType = (System.Type)dictionaryEntry.Key;
                object obj = null;
                try
                {
                    obj = base.GetService(serviceType);
                    IServiceNotification serviceNotification = obj as IServiceNotification;
                    if (serviceNotification != null)
                    {
                        pDelegate(serviceNotification);
                    }
                }
                finally
                {
                    try
                    {
                        System.IDisposable disposable = obj as System.IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            if (!pParentBefore)
            {
                this.DoParentServiceNotification(pDelegate);
            }
        }
        private void DoParentServiceNotification(AssemblyLoader.DoServiceNotificationDelegate pDelegate)
        {
            IServiceNotification serviceNotification = base.ParentProvider as IServiceNotification;
            if (serviceNotification != null)
            {
                pDelegate(serviceNotification);
            }
        }
        public override void RemoveService(System.Type serviceType, bool promote)
        {
            base.RemoveService(serviceType, promote);
            this._forCreateClass.Remove(serviceType);
        }
    }
}
