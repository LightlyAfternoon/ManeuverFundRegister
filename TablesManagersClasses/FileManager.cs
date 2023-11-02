using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class FileManager
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public byte[] attachFile(string filePath)
        {
            byte[] fileBytes;
            using (FileStream stream = new(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new(stream))
                {
                    fileBytes = reader.ReadBytes((int)stream.Length);
                }
            }

            return fileBytes;
        }

        public void getAttachedFile(byte[] fileBytes, string pathToNewLocation)
        {
            using (FileStream stream = new(pathToNewLocation, FileMode.Create, FileAccess.Write))
            {
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}