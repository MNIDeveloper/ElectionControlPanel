using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using ZXing;
using ZXing.QrCode;


namespace ElectionApiFramework.Helpers
{
    public class BarcodeGen
    {
        private BarcodeWriter codeWriter = new BarcodeWriter();

        public Boolean GenerateQRcode(string filepath, int PersonID, string Pin)
        {
            string QR = PersonID.ToString() +","+Pin;
            string fileName = filepath + "Images\\QRCodes\\" + PersonID.ToString()+".jpg";
            codeWriter.Format = BarcodeFormat.QR_CODE;
            codeWriter.Write(QR)
                .Save(fileName);
            if(File.Exists(fileName)) 
            { 
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean UpdateQRCode(string filepath, int PersonID, string Pin)
        {
            string QR = PersonID.ToString() + "," + Pin;
            string fileName = filepath + "Images\\QRCodes\\" + PersonID.ToString() + ".jpg";
            if(File.Exists(fileName)) 
            {
                File.Delete(fileName);
                codeWriter.Format = BarcodeFormat.QR_CODE;
                codeWriter.Write(QR)
                    .Save(fileName);
                if (File.Exists(fileName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                codeWriter.Format = BarcodeFormat.QR_CODE;
                codeWriter.Write(QR)
                    .Save(fileName);
                if (File.Exists(fileName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
