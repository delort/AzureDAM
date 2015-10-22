using System;
using System.IO;
using System.Web;

namespace Avanade.AzureDAM.Integrations.Facades
{
    public class WebFile
    {
        private readonly HttpPostedFileBase _file;

        public WebFile(HttpPostedFileBase file)
        {
            _file = file;
        }

        public byte[] ReadBytes()
        {
            if ((_file == null) || ((_file.ContentLength) == 0) || string.IsNullOrEmpty(_file.FileName))
                return null;

            var fileBytes = new byte[_file.ContentLength];
            _file.InputStream.Read(fileBytes, 0, Convert.ToInt32(_file.ContentLength));

            return fileBytes;
        }
        public string ContentType => _file.ContentType;
        private FileInfo GetFileInfo()
        {
            return new FileInfo(_file.FileName);
        }

        public string Extension => GetFileInfo().Extension;
    }
}
