using System.Collections.Generic;
using System.IO;

namespace Реестр_маневренного_фонда
{
    public static class DeleteTempFilesClass
    {
        public static void deleteTempFiles()
        {
            try
            {
                DirectoryInfo directoryInfo = new(Path.GetTempPath() + @"\ManeuverFund");
                directoryInfo.Delete(true);
            }
            catch { }
        }

    }
}