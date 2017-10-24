using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ImageHelper
{
    public class Exif
    {
        #region 获取图片Exif属性
        public static ImageExif GetExifInfo(Image img)
        {
            System.Drawing.Imaging.PropertyItem[] pt = img.PropertyItems;
            ImageExif exif = new ImageExif();
            for (int i = 0; i < pt.Length; i++)
            {
                PropertyItem p = pt[i];
                switch (pt[i].Id)
                {
                    // 设备制造商 20. 
                    case 0x010F:
                        exif.Make = System.Text.ASCIIEncoding.ASCII.GetString(pt[i].Value);
                        break;
                    case 0x0110: // 设备型号 25. 
                        exif.Model = GetValueOfType2(p.Value);
                        break;
                    case 0x0132: // 拍照时间 30.
                        exif.DateTime = GetValueOfType2(p.Value);
                        break;
                    case 0x829A: // .曝光时间 
                        exif.ExposureTime = GetValueOfType5(p.Value) + " sec";
                        break;
                    case 0x8827: // ISO 40.  
                        exif.ISO = GetValueOfType3(p.Value);
                        break;
                    case 0x010E: // 图像说明info.description
                        exif.ImageTitle = GetValueOfType2(p.Value);
                        break;
                    case 0x920a: //相片的焦距
                        exif.FocalLength = GetValueOfType5A(p.Value) + " mm";
                        break;
                    case 0x829D: //相片的光圈值
                        exif.Aperture = GetValueOfType5A(p.Value);
                        break;
                    case 0x0112:  //方向
                        exif.Orientation = ShortToString(p.Value, 0);
                        break;
                    case 0x011A:
                        exif.XResolution = RationalToSingle(p.Value, 0);
                        break;
                    case 0x011B:
                        exif.YResolution = RationalToSingle(p.Value, 0);
                        break;
                    case 0x0128:
                        exif.ResolutionUnit = RationalToSingle(p.Value, 0);
                        break;
                    case 0x0131:
                        exif.Software = ASCIIToString(p.Value);
                        break;
                    case 0x0002:
                        exif.GPSLatitude = string.Format("{0}°{1}′{2}″", RationalToSingle(p.Value, 0), RationalToSingle(p.Value, 8), RationalToSingle(p.Value, 16));
                        break;
                    case 0x0004:
                        exif.GPSLongitude = string.Format("{0}°{1}′{2}″", RationalToSingle(p.Value, 0), RationalToSingle(p.Value, 8), RationalToSingle(p.Value, 16));
                        break;
                    case 0x0006:
                        exif.GPSAltitude = RationalToSingle(p.Value, 0);
                        break;

                }
            }
            return exif;

        }
        static string ByteToString(byte[] b, int startindex)
        {
            if (startindex + 1 <= b.Length)
            {
                return ((char)b[startindex]).ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        static string ShortToString(byte[] b, int startindex)
        {
            if (startindex + 2 <= b.Length)
            {
                return BitConverter.ToInt16(b, startindex).ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        static string RationalToSingle(byte[] b, int startindex)
        {
            if (startindex + 8 <= b.Length)
            {
                return (BitConverter.ToSingle(b, startindex) / BitConverter.ToSingle(b, startindex + 4)).ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        static string ASCIIToString(byte[] b)
        {
            return Encoding.ASCII.GetString(b);
        }

        private static string GetValueOfType2(byte[] b)// 对type=2 的value值进行读取
        {
            return System.Text.Encoding.ASCII.GetString(b);
        }
        private static string GetValueOfType3(byte[] b) //对type=3 的value值进行读取
        {
            if (b.Length != 2) return "unknow";
            return Convert.ToUInt16(b[1] << 8 | b[0]).ToString();
        }
        private static string GetValueOfType5(byte[] b) //对type=5 的value值进行读取
        {
            if (b.Length != 8) return "unknow";
            UInt32 fm, fz;
            fm = 0;
            fz = 0;
            fz = Convert.ToUInt32(b[7] << 24 | b[6] << 16 | b[5] << 8 | b[4]);
            fm = Convert.ToUInt32(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);
            return fm.ToString() + "/" + fz.ToString() + " sec";
        }
        private static string GetValueOfType5A(byte[] b)//获取光圈的值
        {
            if (b.Length != 8) return "unknow";
            UInt32 fm, fz;
            fm = 0;
            fz = 0;
            fz = Convert.ToUInt32(b[7] << 24 | b[6] << 16 | b[5] << 8 | b[4]);
            fm = Convert.ToUInt32(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);
            double temp = (double)fm / fz;
            return (temp).ToString();
        }
        #endregion
    }
    /// <summary>
    /// 图片属性信息
    /// </summary>
    public class ImageExif
    {
        /// <summary>
        /// 设备制造商
        /// </summary>
        public string Make { set; get; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string Model { set; get; }
        /// <summary>
        /// 方向
        /// </summary>
        public string Orientation { get; set; }
        /// <summary>
        /// 水平分辨率
        /// </summary>
        public string XResolution { get; set; }
        /// <summary>
        /// 垂直分辨率
        /// </summary>
        public string YResolution { get; set; }
        /// <summary>
        /// 分辨率单位
        /// </summary>
        public string ResolutionUnit { get; set; }
        /// <summary>
        /// 软件
        /// </summary>
        public string Software { get; set; }
        /// <summary>
        /// 拍照时间
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// GPS纬度
        /// </summary>
        public string GPSLatitude { get; set; }
        /// <summary>
        /// GPS经度
        /// </summary>
        public string GPSLongitude { get; set; }
        /// <summary>
        /// GPS高度
        /// </summary>
        public string GPSAltitude { get; set; }
        /// <summary>
        /// 图片描述
        /// </summary>
        public string ImageTitle { get; set; }
        /// <summary>
        /// 曝光时间
        /// </summary>
        public string ExposureTime { get; set; }
        /// <summary>
        /// 感光度
        /// </summary>
        public string ISO { get; set; }
        /// <summary>
        /// 相机焦距
        /// </summary>
        public string FocalLength { get; set; }
        /// <summary>
        /// 光圈值
        /// </summary>
        public string Aperture { get; set; }
    }
}
