using System;
namespace DisasterModel
{
    interface IExportToWord
    {
        void Finish();
        void InitWord(string loc);
        void WriteWord();
    }
}
