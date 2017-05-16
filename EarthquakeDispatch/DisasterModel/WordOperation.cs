using System;
using System.Collections.Generic;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Collections;
using System.Runtime.InteropServices;

namespace DisasterModel
{
    class WordOperation
    {
        private string _strFromFilePath = string.Empty;
        private string _strOutFilePath = string.Empty;
        
        private Word._Application _wordApp = null;
        private Word._Document _curDoc = null;
        object oMissing = System.Reflection.Missing.Value;

        public string StrFromFilePath
        {
            get { return _strFromFilePath; }
            set { _strFromFilePath = value; }
        }
        public string StrOutFilePath
        {
            get { return _strOutFilePath; }
            set { _strOutFilePath = value; }
        }

        /// <summary>
        /// 打开Word文档
        /// </summary>
        public bool OpenWord(bool isVisible)
        {
            bool isOpen = false;
            if (_wordApp == null)
                _wordApp = new Word.ApplicationClass();
            if (string.IsNullOrEmpty(_strFromFilePath))
                return isOpen;
            object filepath = _strFromFilePath as object;
            object confirmVersion = false;
            _curDoc = _wordApp.Documents.Open(ref filepath, ref confirmVersion, ref oMissing, ref oMissing, ref oMissing,
               ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
               ref oMissing);
            _wordApp.Visible = isVisible;
            _wordApp.WindowState = WdWindowState.wdWindowStateMaximize;
            if (_curDoc != null) isOpen = true;
            return isOpen;
        }

        /// <summary>
        /// 在书签处插入文字或图片
        /// </summary>
        /// <param name="bookMarkName">书签的名称</param>
        /// <param name="insertContent">插入的内容，如果是图片则为图片路径</param>
        public void InsertWhenBookMark(string bookMarkName, string insertContent, bool isPicture)
        {
            //if (!ValidValue()) return;
            IEnumerator bookMarks = _curDoc.Bookmarks.GetEnumerator();
            bookMarks.Reset();
            int icount = 1;
            while (bookMarks.MoveNext())
            {
                Bookmark curBookmark = bookMarks.Current as Bookmark;
                if (curBookmark.Name == bookMarkName)
                    break;
                icount = icount + 1;
            }
            if (icount > _curDoc.Bookmarks.Count) return;
            object oCount = icount as object;
            Word.Range range = _curDoc.Bookmarks.get_Item(ref oCount).Range;

            range.Select();
            object oFalse = false as object;
            object oTure = true as object;
            object oRange = range as object;
            if (isPicture)
                _wordApp.Selection.InlineShapes.AddPicture(insertContent, ref oFalse, ref oTure, ref oRange);
            else
                range.Text = insertContent;
        }

        /// <summary>
        /// 另存为
        /// </summary>
        public void SaveAs()
        {
            if (!ValidValue()) return;
            if (string.IsNullOrEmpty(_strOutFilePath)) return;
            object oOutFile = _strOutFilePath as object;
            _curDoc.SaveAs(ref oOutFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }

        /// <summary>
        /// 关闭Word文档
        /// </summary>
        /// <param name="isSave">是否保存</param>
        public void CloseWord(bool isSave)
        {
            object IsSave = isSave as object;
            if (!ValidValue()) return;
            try
            {
                _curDoc.Close(ref IsSave, ref oMissing, ref oMissing);
                Marshal.ReleaseComObject(_curDoc);
                _curDoc = null;
                _wordApp.Quit(ref IsSave, ref oMissing, ref oMissing);
                Marshal.ReleaseComObject(_wordApp);
                _wordApp = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidValue()
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
