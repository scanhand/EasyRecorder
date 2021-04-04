using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Files
{
    public class AMKFile
    {
        private AMKFileHeader FileHeader = new AMKFileHeader();

        private byte[] FileHeaderRaw = new byte[AMKFileHeader.HearderSize];

        private AMKFileBody FileBody = new AMKFileBody();

        private string FileName = string.Empty;

        public bool SaveFile()
        {
            Array.Clear(FileHeaderRaw, 0, FileHeaderRaw.Length);

            string header = FileHeader.ToJsonString();
            byte[] headerArray = Encoding.UTF8.GetBytes(header);

            Array.Copy(headerArray, FileHeaderRaw, headerArray.Length);

            byte[] bodyArray = Encoding.UTF8.GetBytes(FileBody.ToJsonString());

            using (FileStream fs = File.Open(FileName, FileMode.CreateNew))
            {
                //Header
                fs.Write(FileHeaderRaw, 0, FileHeaderRaw.Length);

                //Body
                fs.Write(bodyArray, 0, bodyArray.Length);
            }

            return true;
        }
    }
}
