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
        }

        private class SParamter {
            public const string AttachmentTable = "AttachmentTable";
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

    }

}
