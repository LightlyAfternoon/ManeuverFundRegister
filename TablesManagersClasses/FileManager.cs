using System.IO;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public static class FileManager
    {
        static ApplicationContext dbContext = ApplicationContext.GetContext();

        public static byte[] attachFile(string filePath)
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

        public static void getAttachedFile(byte[] fileBytes, string pathToNewLocation)
        {
            using (FileStream stream = new(pathToNewLocation, FileMode.Create, FileAccess.Write))
            {
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}