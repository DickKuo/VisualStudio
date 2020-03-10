namespace CommTool.Business
{
    public interface ICommService
    {
        /// <summary>
        /// 20150108 add by Dick for 取得固定長度的字串 單位為Byte
        /// </summary>
        /// <param name="pString">輸入字串</param>
        /// <param name="pLen">取值長度</param>
        /// <param name="IsLeft">預設true從左算起;false為從右算起</param>
        /// <param name="AddChar">字串長度不足時，補足字元</param>
        /// <returns></returns>
        string GetLenString(string pString, int pLen, bool IsLeft = true, char AddChar = ' ');
    }
}