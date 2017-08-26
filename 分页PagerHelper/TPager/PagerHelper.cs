using System;
using System.Web;

public class PagerHelper
{
    #region 获取分页的Html代码
    /// <summary>
    /// 获取分页的Html代码
    /// 当前页码方法内部根据Request["page"]获取
    /// </summary>
    /// <param name="pageSize">每一页数量</param>
    /// <param name="totalCount">总数量</param>
    /// <param name="url">伪静态地址如/news/list-1-{0}.html</param>
    /// <param name="maxPageNum">最多显示的页码个数（100页 每次只显示8个其他隐藏）</param>
    /// <returns></returns>
    public static string GetPageHtml(int pageSize, int totalCount, string url, int maxPageNum = 8)
    {
        HttpRequest Request = HttpContext.Current.Request;
        int curPageIndex = 1;
        if (!string.IsNullOrWhiteSpace(Request["page"]))
        {
            curPageIndex = Convert.ToInt32(Request["page"] ?? "1");
            curPageIndex = curPageIndex <= 0 ? 1 : curPageIndex;
        }

        System.Text.StringBuilder pageHtml = new System.Text.StringBuilder();
        //if (pageIndex > 1)
        //{
        pageHtml.Append(curPageIndex == 1 ? "<a href=\"javascript:void(0);\">首页</a>" : "<a   href=\"" + string.Format(url, 1) + "\">首页</a>");
        pageHtml.Append(curPageIndex > 1 ? "<a href=\"" + string.Format(url, curPageIndex - 1) + "\">上一页</a>" : "<a href=\"javascript:void(0);\">上一页</a>");
        //}
        int pageCount = GetPageCount(pageSize, totalCount);//总页码
        //获取显示区域第一个开始位置 如 1 9 17
        int firstNum = curPageIndex % maxPageNum == 0 ? curPageIndex - (maxPageNum - 1) : curPageIndex - curPageIndex % maxPageNum + 1;
        if (firstNum > maxPageNum)
        {
            pageHtml.Append("<a   href=\"" + string.Format(url, firstNum - 1) + "\">...</a>");
        }

        for (int i = firstNum; i < firstNum + maxPageNum; i++)
        {
            if (i > pageCount) break;
            string css = string.Empty;
            if (i == curPageIndex)
            {
                css = "class=\"currentpage\"";
            }
            pageHtml.Append("<a " + css + " href=\"" + string.Format(url, i) + "\">" + i + "</a>");

        }
        if (pageCount >= firstNum + maxPageNum)
        {
            pageHtml.Append("<a   href=\"" + string.Format(url, firstNum + maxPageNum) + "\">...</a>");
        }
        //if (pageCount > curPageIndex)
        //{
        pageHtml.Append(curPageIndex < pageCount ? "<a href=\"" + string.Format(url, curPageIndex + 1) + "\">下一页</a>" : "<a href=\"javascript:void(0);\">下一页</a>");
        pageHtml.Append("<a href=\"" + string.Format(url, pageCount) + "\">尾页</a>");
        //}
        pageHtml.Append(string.Format("<a href=\"javascript:void(0);\">共{0}页,{1}条</a>", pageCount, totalCount));
        return pageHtml.ToString();
    }

    #endregion

    #region 获取页码总数
    /// <summary>
    /// 获取页码总数
    /// </summary>
    /// <param name="pageSize">每一页 数量</param>
    /// <param name="totalCount">总数量</param>
    /// <returns></returns>
    private static int GetPageCount(int pageSize, int totalCount)
    {
        int pageNumbers = 0;
        if (totalCount % pageSize != 0)
        {
            pageNumbers = totalCount / pageSize + 1;
        }
        else
        {
            pageNumbers = totalCount / pageSize;
        }
        pageNumbers = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalCount) / Convert.ToDouble(pageSize)));
        return pageNumbers;
    }
    #endregion

}