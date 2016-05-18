using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using MyBlog;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebService ws = new WebService();
        DataSet ds = ws.GetUserArticle(HttpContext.Current.User.Identity.Name);
        ArticleList.DataSource = ds;
        ArticleList.DataBind();
    }
}