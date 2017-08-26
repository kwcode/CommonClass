<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        html, body, div, span, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, code, del, dfn, em, img, q, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td, i { margin: 0; padding: 0; border: 0; font-weight: inherit; font-style: inherit; font-size: 100%; font-family: inherit; vertical-align: baseline; }
        body { background: #fff; font: 12px/1.5 Tahoma; color: #000; }
        a { text-decoration: none; cursor: pointer; }
        /*分页*/
        .page { clear: both; text-align: center; margin-top: 10px; margin-bottom: 20px; }
        .page a { border: 1px solid #dbdbdb; background: #fff; padding: 5px 10px; margin: 1px; display: inline-block; color: #000; }
        .page a:hover { text-decoration: none; background-color: #2196F3; color: #fff; }
        .page span a { border: 1px solid #1f5b13; background: #fff; padding: 2px 7px; margin: 1px; display: inline-block; color: #104c00; }
        .page span a:hover { text-decoration: none; background-color: #a3c79c; }
        .page .currentpage { background-color: #ff8800; color: #fff; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page">
            <asp:Literal runat="server" ID="ltHtml"></asp:Literal>
        </div>
    </form>
    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            ltHtml.Text = PagerHelper.GetPageHtml(3, 100, "/tpager_demo.aspx?page={0}");
        }
    </script>
</body>
</html>
