using Greeny.Core.Contracts;

namespace Greeny.Core.Services
{
    public class FileManagerService : IFileManagerService
    {
        private readonly string _storePath;
        public FileManagerService(string storePath)
        {
            if(string.IsNullOrWhiteSpace(storePath)) throw new ArgumentNullException(nameof(storePath));
            _storePath = storePath;
        }
        public string GetFile(string name)
        {
            var path = Path.Combine(_storePath, name);

            if (!File.Exists(path))
            {
                //TODO: залогировать ситуацйию с отсутствием  файла в хранилище 
                return string.Empty;
            }

            using (var stream = new FileStream(path, FileMode.Open))
            {
                return StreamToBase64(stream);
            }
        }

        private string StreamToBase64(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();

                return Convert.ToBase64String(imageBytes);
            }
        }

        //private stream Base64ToStream(string base64String)
        //{
        //    // Convert Base64 String to byte[]
        //    byte[] imageBytes = Convert.FromBase64String(base64String);
        //    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //    {
        //        // Convert byte[] to Image
        //        ms.Write(imageBytes, 0, imageBytes.Length);
        //        return Image.FromStream(ms, true);
        //    }
        //}
    }
}
