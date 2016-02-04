using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Menu_Engineering
{
    public interface DatabaseInterFace
    {
        /// <summary>
        /// 更新菜單食材明細
        /// </summary>
        /// <param name="menu">菜單</param>
        /// <param name="dt">明細表</param>
        void UpdateMenuFood(Menus menu, DataTable dt);

        /// <summary>
        /// 刪除菜單類別
        /// </summary>
        /// <param name="MenuCollectionsId"></param>
         void DeleMenuCollection(string MenuCollectionsId);
    }

    public class Database : DatabaseInterFace
    {
        
        /// <summary>
        /// 更新菜單食材明細
        /// </summary>
        /// <param name="dt"></param>
        public void UpdateMenuFood(Menus menu, DataTable dt)
        {
            string sql = string.Empty;

            using (SqlConnection scon = new SqlConnection(SQLHelper.SHelper._sqlconnection))
            {
                SqlCommand scm = new SqlCommand();
                scon.Open();
                scm.Connection = scon;
                SqlTransaction transaction = scon.BeginTransaction();
                scm.Transaction = transaction;
                try
                {
                    sql = string.Format("Delete  MenuFood Where MenuId ='{0}'", menu.MenuId);
                    scm.CommandText = sql;
                    scm.ExecuteNonQuery();
                    
                    sql = string.Format("Insert Into Menu (MenuId,MenuCollectionId,Name,SalePrice,NumberSold,Photo,Remark)values(@MenuId,@MenuCollectionId,@Name,@SalePrice,@NumberSold,@Photo,@Remark)");
                    Dictionary<string, object> Dic = new Dictionary<string, object>();
                    Dic.Add("MenuId",menu.MenuId);
                    Dic.Add("MenuCollectionId", menu.MenuCollectionId);
                    Dic.Add("Name", menu.Name);
                    Dic.Add("SalePrice", menu.SalePrice);
                    Dic.Add("NumberSold", menu.NumberSold);
                    Dic.Add("Photo", menu.Photo);
                    Dic.Add("Remark", menu.Remark);
                    SQLHelper.SHelper.ExeNoQueryUseParameter(sql, Dic);

                    SQLHelper.SHelper.SqlBulkCopy(dt);
                    transaction.Commit();
                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                } 
            }
        }



        /// <summary>
        /// 刪除菜單類別
        /// </summary>
        /// <param name="MenuCollectionsId"></param>
        public void DeleMenuCollection(string MenuCollectionsId)
        {
            string sql = string.Empty;
            sql = string.Format("Delete MenuCollections where MenuCollectionsId =@MenuCollectionsId");
            Dictionary<string, object> Dic = new Dictionary<string, object>();
            Dic.Add("MenuCollectionsId", MenuCollectionsId);
            SQLHelper.SHelper.ExeNoQueryUseParameter(sql, Dic);
        }
    }
}
