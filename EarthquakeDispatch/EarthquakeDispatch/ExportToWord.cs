using System;
using System.Collections.Generic;
using System.Text;

namespace EarthquakeDispatch
{
    class ExportToWord : IExportToWord
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
            wordOp.CloseWord(false);
        }

        public void WriteWord()
        {
            wordOp.InsertWhenBookMark("a", "Hello", false);
            wordOp.InsertWhenBookMark("b", "World", false);
            wordOp.InsertWhenBookMark("c", "My", false);
        }
    }
}
