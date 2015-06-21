using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    public abstract class SearcherBase<T>
    {
        private System.Reflection.Assembly[] _assemblies;
        private System.Type _searchAttribute;
        private bool _searchAbstractType;
        public System.Reflection.Assembly[] Assemblies
        {
            get
            {
                return this._assemblies;
            }
            set
            {
                this._assemblies = value;
            }
        }
        public System.Type SearchAttribute
        {
            get
            {
                return this._searchAttribute;
            }
            set
            {
                this._searchAttribute = value;
            }
        }
        public bool SearchAbstractType
        {
            get
            {
                return this._searchAbstractType;
            }
            set
            {
                this._searchAbstractType = value;
            }
        }
        public SearcherBase()
        {
            this._searchAbstractType = false;
        }
        public SearcherBase(System.Reflection.Assembly pAssembly, System.Type pSearchAttribute)
            : this()
        {
            if (pAssembly == null)
            {
                throw new System.ArgumentNullException("pAssembly");
            }
            if (pSearchAttribute == null)
            {
                throw new System.ArgumentNullException("pSearchAttribute");
            }
            this._assemblies = new System.Reflection.Assembly[]
			{
				pAssembly
			};
            this._searchAttribute = pSearchAttribute;
        }
        public SearcherBase(System.Reflection.Assembly[] pAssemblies, System.Type pSearchAttribute)
            : this()
        {
            if (pAssemblies == null)
            {
                throw new System.ArgumentNullException("pAssembly");
            }
            if (pSearchAttribute == null)
            {
                throw new System.ArgumentNullException("pSearchAttribute");
            }
            this._assemblies = pAssemblies;
            this._searchAttribute = pSearchAttribute;
        }
        public T[] Search()
        {
            if (this.Assemblies == null)
            {
                throw new System.ArgumentNullException("Assemblies");
            }
            if (this.SearchAttribute == null)
            {
                throw new System.ArgumentNullException("SearchAttribute");
            }
            System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
            System.Attribute attribute = null;
            T item = default(T);
            System.Reflection.Assembly[] assemblies = this._assemblies;
            for (int i = 0; i < assemblies.Length; i++)
            {
                System.Reflection.Assembly assembly = assemblies[i];
                System.Type[] types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    System.Type pType = types[j];
                    if (this.OnSearchType(pType, out attribute, ref item))
                    {
                        list.Add(item);
                    }
                }
            }
            return list.ToArray();
        }
        protected virtual bool OnSearchType(System.Type pType, out System.Attribute pFindAttribute, ref T pResult)
        {
            if (this._searchAbstractType || !pType.IsAbstract)
            {
                object[] customAttributes = pType.GetCustomAttributes(true);
                if (customAttributes != null)
                {
                    object[] array = customAttributes;
                    for (int i = 0; i < array.Length; i++)
                    {
                        System.Attribute attribute = (System.Attribute)array[i];
                        if (attribute.GetType() == this._searchAttribute)
                        {
                            pFindAttribute = attribute;
                            return true;
                        }
                    }
                }
            }
            pFindAttribute = null;
            return false;
        }
    }
}
