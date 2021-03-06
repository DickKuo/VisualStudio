﻿using System.IO;

namespace CommTool.Business
{
    public interface IFileService
    {
        /// <summary>
        ///  設定資料夾路徑底下檔案是否唯獨
        /// </summary>
        /// <param name="pFilePath">資料夾路徑</param>
        /// <param name="IsReadOnly">是否唯獨 true:唯獨 ; false :不唯獨</param>
        void FileReadOnly(DirectoryInfo dirInfo, bool IsReadOnly);
     }
}