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
        public AMKFileHeader FileHeader = new AMKFileHeader();

        private byte[] FileHeaderRaw = new byte[AMKFileHeader.HearderSize];

        public AMKFileBody FileBody = new AMKFileBody();

        public string FileName = string.Empty;

        public bool SaveFile()
        {
            //Header
            Array.Clear(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);

            string header = null;
            try
            {
                header = this.FileHeader.ToJsonString();
            }
            catch(Exception ex) 
            { 
                ALog.Debug($"AMKFile SaveFile Error - Header! ({ex.Message})");
                return false;
            }

            byte[] headerArray;
            headerArray = Encoding.UTF8.GetBytes(header);
            Array.Copy(headerArray, this.FileHeaderRaw, headerArray.Length);

            //Body
            byte[] bodyArray;
            try
            {
                bodyArray = Encoding.UTF8.GetBytes(this.FileBody.ToJsonString());
            }
            catch (Exception ex)
            {
                ALog.Debug($"AMKFile SaveFile Error - Body! ({ex.Message})");
                return false;
            }

            //FileStream
            try
            {
                //If already the file is exist, first delete it.
                if (File.Exists(this.FileName))
                    File.Delete(this.FileName);

                using (FileStream fs = File.Open(this.FileName, FileMode.CreateNew))
                {
                    //AMK File Keyword
                    fs.Write(Encoding.ASCII.GetBytes(AMKFileHeader.AMKFileKeyword), 0, AMKFileHeader.AMKFileKeyword.Length);

                    //Header
                    fs.Write(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);

                    //Body
                    fs.Write(bodyArray, 0, bodyArray.Length);
                }
            }
            catch (Exception ex)
            {
                ALog.Debug($"AMKFile SaveFile Error - FileSteram! ({ex.Message})");
                return false;
            }

            return true;
        }

        public bool LoadFile()
        {
            int ret = 0;
            using (FileStream fs = File.OpenRead(this.FileName))
            {
                //File Keyword
                byte[] fileKeyword = new byte[AMKFileHeader.AMKFileKeyword.Length];

                ret = fs.Read(fileKeyword, 0, fileKeyword.Length);
                if(ret <= 0 || ret != fileKeyword.Length || Encoding.ASCII.GetString(fileKeyword) != AMKFileHeader.AMKFileKeyword)
                {
                    ALog.Debug($"AMKFile LoadFile Error - File Keyword");
                    return false;
                }

                //Header
                Array.Clear(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);
                ret = fs.Read(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);
                if(ret <= 0 || ret != this.FileHeaderRaw.Length)
                {
                    ALog.Debug($"AMKFile LoadFile Error - Header");
                    return false;
                }
                
                try
                {
                    string strHeader = Encoding.UTF8.GetString(this.FileHeaderRaw);
                    this.FileHeader = AMKFileHeader.FromJsonString(strHeader);
                }
                catch(Exception ex)
                {
                    ALog.Debug($"AMKFile LoadFile Error - FileHeader! ({ex.Message})");
                    return false;
                }

                //Body
                long remainLength = fs.Length - fs.Position;
                if(remainLength <= 0)
                {
                    ALog.Debug($"AMKFile LoadFile Error - Body");
                    return false;
                }

                byte[] bodyRaw = new byte[remainLength];
                ret = fs.Read(bodyRaw, 0, bodyRaw.Length);
                if (ret <= 0 || ret != bodyRaw.Length)
                {
                    ALog.Debug($"AMKFile LoadFile Error - Body");
                    return false;
                }

                try
                {
                    string strBody = Encoding.UTF8.GetString(bodyRaw);
                    this.FileBody = AMKFileBody.FromJsonString(strBody);
                }
                catch (Exception ex)
                {
                    ALog.Debug($"AMKFile LoadFile Error - FileBody! ({ex.Message})");
                    return false;
                }
            }
            return true;
        }
    }
}
