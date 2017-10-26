using CommTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class AttachmentsDAO : CommBase {

        private class SP {
            public const string AddAttachment = "AddAttachment";
            public const string GetAddAttachmentsByTransSN = "GetAddAttachmentsByTransSN";
        }

        private class SParamter {
            public const string AttachmentTable = "AttachmentTable";
            public const string TargetID = "TargetID";
        }

        public int AddAttachmentsByBatch(List<Attachments> AttachmentsList) {  
            try {
                DataTable AttachmentTable = ObjectUtility.ToDataTable(AttachmentsList, Attachments.GetTableTypeColumn());
                USP.AddParameter(SParamter.AttachmentTable, AttachmentTable);
                DataTable dt = USP.ExeProcedureGetDataTable(SP.AddAttachment);
                if (dt != null && dt.Rows.Count > 0) {
                    return Convert.ToInt32(dt.Rows[0][0]);
                }
                else {
                    return -1;
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }

        /// <summary>取得交易單得附檔</summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public List<Attachments> GetAddAttachmentsByTransSN(int SN) {
            List<Attachments> AttachmentsList = new List<Attachments>();
            USP.AddParameter(SParamter.TargetID, SN);
            AttachmentsList = USP.ExeProcedureGetObjectList(SP.GetAddAttachmentsByTransSN,new Attachments());
            return AttachmentsList;
        }


    }

}
