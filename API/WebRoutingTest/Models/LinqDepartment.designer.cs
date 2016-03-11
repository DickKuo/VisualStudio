﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.18444
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebRoutingTest.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="MVCTEST")]
	public partial class LinqDepartmentDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 擴充性方法定義
    partial void OnCreated();
    partial void InsertDepartment(Department instance);
    partial void UpdateDepartment(Department instance);
    partial void DeleteDepartment(Department instance);
    partial void InsertEmployee(Employee instance);
    partial void UpdateEmployee(Employee instance);
    partial void DeleteEmployee(Employee instance);
    #endregion
		
		public LinqDepartmentDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["MVCTESTConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public LinqDepartmentDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LinqDepartmentDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LinqDepartmentDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LinqDepartmentDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Department> Department
		{
			get
			{
				return this.GetTable<Department>();
			}
		}
		
		public System.Data.Linq.Table<Employee> Employee
		{
			get
			{
				return this.GetTable<Employee>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Department")]
	public partial class Department : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _DepNo;
		
		private string _Name;
		
		private string _Remark;
		
    #region 擴充性方法定義
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDepNoChanging(string value);
    partial void OnDepNoChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnRemarkChanging(string value);
    partial void OnRemarkChanged();
    #endregion
		
		public Department()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DepNo", DbType="NVarChar(20)", IsPrimaryKey=true)]
		public string DepNo
		{
			get
			{
				return this._DepNo;
			}
			set
			{
				if ((this._DepNo != value))
				{
					this.OnDepNoChanging(value);
					this.SendPropertyChanging();
					this._DepNo = value;
					this.SendPropertyChanged("DepNo");
					this.OnDepNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(20)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remark", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string Remark
		{
			get
			{
				return this._Remark;
			}
			set
			{
				if ((this._Remark != value))
				{
					this.OnRemarkChanging(value);
					this.SendPropertyChanging();
					this._Remark = value;
					this.SendPropertyChanged("Remark");
					this.OnRemarkChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Employee")]
	public partial class Employee : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _EmpNo;
		
		private string _EmpFirstName;
		
		private string _EmpLastName;
		
		private string _Tel;
		
		private string _Addr;
		
		private string _DepNo;
		
    #region 擴充性方法定義
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnEmpNoChanging(string value);
    partial void OnEmpNoChanged();
    partial void OnEmpFirstNameChanging(string value);
    partial void OnEmpFirstNameChanged();
    partial void OnEmpLastNameChanging(string value);
    partial void OnEmpLastNameChanged();
    partial void OnTelChanging(string value);
    partial void OnTelChanged();
    partial void OnAddrChanging(string value);
    partial void OnAddrChanged();
    partial void OnDepNoChanging(string value);
    partial void OnDepNoChanged();
    #endregion
		
		public Employee()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmpNo", DbType="NVarChar(20) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string EmpNo
		{
			get
			{
				return this._EmpNo;
			}
			set
			{
				if ((this._EmpNo != value))
				{
					this.OnEmpNoChanging(value);
					this.SendPropertyChanging();
					this._EmpNo = value;
					this.SendPropertyChanged("EmpNo");
					this.OnEmpNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmpFirstName", DbType="NVarChar(20)")]
		public string EmpFirstName
		{
			get
			{
				return this._EmpFirstName;
			}
			set
			{
				if ((this._EmpFirstName != value))
				{
					this.OnEmpFirstNameChanging(value);
					this.SendPropertyChanging();
					this._EmpFirstName = value;
					this.SendPropertyChanged("EmpFirstName");
					this.OnEmpFirstNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmpLastName", DbType="NVarChar(20)")]
		public string EmpLastName
		{
			get
			{
				return this._EmpLastName;
			}
			set
			{
				if ((this._EmpLastName != value))
				{
					this.OnEmpLastNameChanging(value);
					this.SendPropertyChanging();
					this._EmpLastName = value;
					this.SendPropertyChanged("EmpLastName");
					this.OnEmpLastNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Tel", DbType="NVarChar(20)")]
		public string Tel
		{
			get
			{
				return this._Tel;
			}
			set
			{
				if ((this._Tel != value))
				{
					this.OnTelChanging(value);
					this.SendPropertyChanging();
					this._Tel = value;
					this.SendPropertyChanged("Tel");
					this.OnTelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Addr", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string Addr
		{
			get
			{
				return this._Addr;
			}
			set
			{
				if ((this._Addr != value))
				{
					this.OnAddrChanging(value);
					this.SendPropertyChanging();
					this._Addr = value;
					this.SendPropertyChanged("Addr");
					this.OnAddrChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DepNo", DbType="NVarChar(20)")]
		public string DepNo
		{
			get
			{
				return this._DepNo;
			}
			set
			{
				if ((this._DepNo != value))
				{
					this.OnDepNoChanging(value);
					this.SendPropertyChanging();
					this._DepNo = value;
					this.SendPropertyChanged("DepNo");
					this.OnDepNoChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
