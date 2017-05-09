using System;
namespace EarthquakeDispatch
{
    interface IExportToWord
    {
        void Finish();
        void InitWord(string loc);
        void WriteWord();
    }
}
