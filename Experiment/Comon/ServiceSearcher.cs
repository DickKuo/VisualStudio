using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    public sealed class ServiceSearcher : SearcherBase<IServiceEntry>
    {
        public ServiceSearcher()
        {
            base.SearchAttribute = typeof(ServiceClassAttribute);
        }
        public ServiceSearcher(System.Reflection.Assembly pAssembly)
            : this()
        {
            base.Assemblies = new System.Reflection.Assembly[]
			{
				pAssembly
			};
        }
        protected override bool OnSearchType(System.Type pType, out System.Attribute pFindAttribute, ref IServiceEntry pResult)
        {
            bool flag = base.OnSearchType(pType, out pFindAttribute, ref pResult);
            if (flag)
            {
                ServiceClassAttribute serviceClassAttribute = (ServiceClassAttribute)pFindAttribute;
                if (serviceClassAttribute.Enabled)
                {
                    pResult = new ServiceEntry(serviceClassAttribute.ServiceInterface, pType, serviceClassAttribute.ServiceCreateType);
                }
                else
                {
                    //System.Console.WriteLine(string.Format(Resources.ServiceClassAttributeNotEnabled, pType.FullName));
                }
            }
            return flag;
        }
    }
}
