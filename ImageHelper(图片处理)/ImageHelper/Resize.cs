using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ImageHelper
{
    /// <summary>
    /// 图片调整处理 缩图
    /// </summary>
    public class Resize
    {
        /// <summary>
        /// 获取缩略图片
        /// 指定当目标缩略图大于原图时 不处理 
        /// </summary>
        /// <param name="original_image">原图</param>
        /// <param name="titCase">缩图方式</param>
        /// <param name="target_width">指定目标缩略图的宽度</param>
        /// <param name="target_height">指定目标缩略图的高度</param>
        /// <returns></returns>
        public static Bitmap GetThumbnailImage(Image original_image, ThumbnailImageType titCase, int target_width, int target_height)
        {
            //原图高宽
            int origin_width = original_image.Width, origin_height = original_image.Height;
            float orignRatio = (float)origin_width / (float)origin_height;//原图 宽高 比例
            int new_width = target_width, new_height = target_height; //新图片 宽高
            float target_ratio = (float)target_width / (float)target_height;
            //缩略图生成方式
            switch (titCase)
            {
                //缩放全图到正确比例
                case ThumbnailImageType.Zoom:
                    {
                        #region 缩图比例
                        if (target_width > 0 && target_height <= 0) //只有宽
                        {
                            new_height = (int)Math.Floor((float)target_width / orignRatio);//只有高
                        }
                        else if (target_height > 0 && target_width <= 0)
                        {
                            new_width = (int)Math.Floor((float)target_height * orignRatio);
                        }
                        else if (target_width > 0 && target_height > 0)//宽高同时限制
                        {
                            if (orignRatio > 1)//宽>高
                            {
                                new_height = (int)Math.Floor((float)target_width / orignRatio);//只有高
                            }
                            else
                            {
                                new_width = (int)Math.Floor((float)target_height * orignRatio);
                            }
                        }
                        #endregion
                    }
                    break;
            }



            // 创建缩略图
            Bitmap returnImage = new System.Drawing.Bitmap(new_width, new_height);
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(returnImage);
            //指定画布大小并以透明色填充空余部分
            graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, new_width, new_height));

            //设置高质量绘画
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //输出缩略图
            graphic.DrawImage(original_image, 0, 0, new_width, new_height);

            return returnImage;
        }

    }

    #region [ThumbnailImageType]缩率图生成类型
    /// <summary>
    /// 缩率图生成类型
    /// </summary>
    public enum ThumbnailImageType
    {
        /// <summary>
        /// 等比缩放
        /// 限制在设定在指定w与h的矩形内的最大图片
        /// </summary>
        Zoom = 1,
        /// <summary>
        /// 截取生成
        /// </summary>
        //Cut = 2
    }
    #endregion
}
