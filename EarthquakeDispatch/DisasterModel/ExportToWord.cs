using System;
using System.Collections.Generic;
using System.Text;

namespace DisasterModel
{
    public class ExportToWord 
    {
        private WordOperation wordOp = null;

        public void InitWord(string loc)
        {
            if (wordOp == null)
                wordOp = new WordOperation();

            wordOp.StrFromFilePath = loc;
            wordOp.StrOutFilePath = loc;

            if (!wordOp.OpenWord(false))
            {
                throw new Exception("创建Word文档失败，请确认本机是否安装了Word!");
            }
        }

        public void Finish()
        {
            wordOp.SaveAs();
            wordOp.CloseWord(true);
        }

        public void WriteWord(string bookmark, string content, bool isPic)
        {
            wordOp.InsertWhenBookMark(bookmark, content, isPic);
        }
    }
}
