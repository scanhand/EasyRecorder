using ESR.Global;
using ESR.Recorder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace ESR.Files
{
    public class ESRFile
    {
        public ESRFileHeader FileHeader = new ESRFileHeader();

        private byte[] FileHeaderRaw = new byte[ESRFileHeader.HearderSize];

        public ESRFileBody FileBody = new ESRFileBody();

        public string FileName = string.Empty;

        public static bool SaveFileDialog(List<IRecorderItem> items)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ESR files (*.ESR)|*.ESR|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return false;

            //Get the path of specified file
            string filePath = saveFileDialog.FileName;

            ESRFile file = new ESRFile();
            file.FileName = filePath;
            file.FileBody.Items = items.Copy<List<IRecorderItem>>();

            using (new WaitCursor())
            {
                if (!file.SaveFile())
                {
                    System.Windows.MessageBox.Show("File Save Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }

        public bool SaveFile()
        {
            //Header
            Array.Clear(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);

            string header = null;
            try
            {
                header = this.FileHeader.ToJsonString();
            }
            catch (Exception ex)
            {
                ALog.Debug($"ESRFile SaveFile Error - Header! ({ex.Message})");
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
                ALog.Debug($"ESRFile SaveFile Error - Body! ({ex.Message})");
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
                    //ESR File Keyword
                    fs.Write(Encoding.ASCII.GetBytes(ESRFileHeader.ESRFileKeyword), 0, ESRFileHeader.ESRFileKeyword.Length);

                    //Header
                    fs.Write(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);

                    //Body
                    fs.Write(bodyArray, 0, bodyArray.Length);
                }
            }
            catch (Exception ex)
            {
                ALog.Debug($"ESRFile SaveFile Error - FileSteram! ({ex.Message})");
                return false;
            }

            return true;
        }

        public static ESRFile LoadFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ESR files (*.ESR)|*.ESR|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;

            //Get the path of specified file
            string filePath = openFileDialog.FileName;
            ESRFile file = new ESRFile();
            file.FileName = filePath;

            using (new WaitCursor())
            {
                if (!file.LoadFile())
                {
                    System.Windows.MessageBox.Show("File Load Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            return file;
        }

        public bool LoadFile()
        {
            int ret = 0;
            using (FileStream fs = File.OpenRead(this.FileName))
            {
                //File Keyword
                byte[] fileKeyword = new byte[ESRFileHeader.ESRFileKeyword.Length];

                ret = fs.Read(fileKeyword, 0, fileKeyword.Length);
                if (ret <= 0 || ret != fileKeyword.Length || Encoding.ASCII.GetString(fileKeyword) != ESRFileHeader.ESRFileKeyword)
                {
                    ALog.Debug($"ESRFile LoadFile Error - File Keyword");
                    return false;
                }

                //Header
                Array.Clear(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);
                ret = fs.Read(this.FileHeaderRaw, 0, this.FileHeaderRaw.Length);
                if (ret <= 0 || ret != this.FileHeaderRaw.Length)
                {
                    ALog.Debug($"ESRFile LoadFile Error - Header");
                    return false;
                }

                try
                {
                    string strHeader = Encoding.UTF8.GetString(this.FileHeaderRaw);
                    this.FileHeader = ESRFileHeader.FromJsonString(strHeader);
                }
                catch (Exception ex)
                {
                    ALog.Debug($"ESRFile LoadFile Error - FileHeader! ({ex.Message})");
                    return false;
                }

                //Body
                long remainLength = fs.Length - fs.Position;
                if (remainLength <= 0)
                {
                    ALog.Debug($"ESRFile LoadFile Error - Body");
                    return false;
                }

                byte[] bodyRaw = new byte[remainLength];
                ret = fs.Read(bodyRaw, 0, bodyRaw.Length);
                if (ret <= 0 || ret != bodyRaw.Length)
                {
                    ALog.Debug($"ESRFile LoadFile Error - Body");
                    return false;
                }

                try
                {
                    string strBody = Encoding.UTF8.GetString(bodyRaw);
                    this.FileBody = ESRFileBody.FromJsonString(strBody);
                }
                catch (Exception ex)
                {
                    ALog.Debug($"ESRFile LoadFile Error - FileBody! ({ex.Message})");
                    return false;
                }
            }
            return true;
        }
    }
}
