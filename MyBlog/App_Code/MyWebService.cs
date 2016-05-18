using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.UI;
using System.Data;
using System.IO;
using MyBlog;

/// <summary>
/// WebService 的摘要说明
/// </summary>
[WebService(Namespace = "MyWebService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public Boolean LogIn(String UserName, String Password, bool RememberMe)
    {
        var manager = new UserManager();
        ApplicationUser user = manager.Find(UserName, Password);
        if (user != null)
        {
            IdentityHelper.SignIn(manager, user, RememberMe);
            return true;
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public Boolean Register(String UserName, String Password)
    {
        var manager = new UserManager();
        var user = new ApplicationUser() { UserName = UserName };
        IdentityResult result = manager.Create(user, Password);
        if (result.Succeeded)
        {
            return LogIn(UserName, Password, false);
        }
        else
        {
            return false;
        }
    }

    [WebMethod]
    public Boolean UploadAvatar(byte[] b, String UserName)
    {
        try
        {
            MemoryStream m = new MemoryStream(b);
            using (FileStream fs = File.Open(Server.MapPath(@"\Avatar\" + UserName + ".png"), FileMode.Create))
            {
                m.WriteTo(fs);
                m.Close();
                fs.Close();
                return true;
            }
        }
        catch (Exception xx)
        {
            return false;
        }
    }

    [WebMethod]
    public byte[] GetAvatar(String UserName)
    {
        return File.ReadAllBytes(@"\Avatar\" + UserName + ".png");
    }

    [WebMethod]
    public int AddArticle(String title, String author, String text)
    {
        return DBHelper.NonQuery("INSERT INTO article (Title,Author,Date,Text) VALUES ('" + title + "','" + author + "',getdate(),'" + text + "')");
    }

    [WebMethod]
    public int EditArticle(String id, String title, String text)
    {
        return DBHelper.NonQuery("UPDATE article SET Title = '" + title + "', Text = '" + text + "' WHERE id = " + id);
    }

    [WebMethod]
    public int DeleteArticle(String id)
    {
        return DBHelper.NonQuery("DELETE FROM article WHERE id = " + id);
    }

    [WebMethod]
    public int AddComment(String articleId, String author, String text)
    {
        return DBHelper.NonQuery("INSERT INTO comment (ArticleId,Author,Date,Text) VALUES (" + articleId + ",'" + author + "',getdate(),'" + text + "')");
    }

    [WebMethod]
    public int EditComment(String id, String text)
    {
        return DBHelper.NonQuery("UPDATE comment SET Text = '" + text + "' WHERE id = " + id);
    }

    [WebMethod]
    public int DeleteComment(String id)
    {
        return DBHelper.NonQuery("DELETE FROM comment WHERE id = " + id);
    }

    [WebMethod]
    public int AddFollowUser(String user1, String user2)
    {
        return DBHelper.NonQuery("INSERT INTO follow VALUES ('" + user1 + "','" + user2 + "')");
    }

    [WebMethod]
    public int CancelFollowUser(String user1, String user2)
    {
        return DBHelper.NonQuery("DELETE FROM follow WHERE User1 = '" + user1 + "' AND User2 = '" + user2 + "'");
    }

    [WebMethod]
    public int AddArticleLabel(String article, String label)
    {
        return DBHelper.NonQuery("INSERT INTO article_label VALUES (" + article + ",'" + label + "')");
    }

    [WebMethod]
    public int CancelArticleLabel(String article, String label)
    {
        return DBHelper.NonQuery("DELETE FROM article_label WHERE Article = " + article + " AND Label = '" + label + "'");
    }

    [WebMethod]
    public int AddFocusLabel(String user, String label)
    {
        return DBHelper.NonQuery("INSERT INTO user_label VALUES ('" + user + "','" + label + "')");
    }

    [WebMethod]
    public int CancelFocusLabel(String user, String label)
    {
        return DBHelper.NonQuery("DELETE FROM user_label WHERE Users = '" + user + "' AND Label = '" + label + "'");
    }

    [WebMethod]
    public DataSet GetFollowUser(String id)
    {
        return DBHelper.DataSet("SELECT User1 FROM Follow WHERE User2 = '" + id + "'");
    }

    [WebMethod]
    public DataSet GetFocusUser(String id)
    {
        return DBHelper.DataSet("SELECT User2 FROM Follow WHERE User1 = '" + id + "'");
    }

    [WebMethod]
    public DataSet GetFocusLabel(String id)
    {
        return DBHelper.DataSet("SELECT Label FROM User_Label WHERE Users = '" + id + "'");
    }

    [WebMethod]
    public DataSet GetArticleInFocusLabel(String id)
    {
        return DBHelper.DataSet("SELECT Article.* FROM Article,Article_Label,User_Label " +
            "WHERE User_Label.Users = '" + id + "' "+
            "AND User_Label.Label = Article_Label.Label "+
            "AND Article_Label.Article = Article.Id");
    }

    [WebMethod]
    public DataSet GetArticleInFocusUser(String id)
    {
        return DBHelper.DataSet("SELECT Article.* FROM Article,Follow " +
            "WHERE Follow.User1 = '" + id + "' " +
            "AND Follow.User2 = Article.Author");
    }

    [WebMethod]
    public DataSet GetUserArticle(String id)
    {
        return DBHelper.DataSet("SELECT * FROM Article WHERE Author = '"+id+"'");
    }

    [WebMethod]
    public DataSet GetArticleComment(String articleId)
    {
        return DBHelper.DataSet("SELECT * FROM Comment WHERE Article = " + articleId);
    }

    [WebMethod]
    public DataSet GetArticleLabel(String articleId)
    {
        return DBHelper.DataSet("SELECT Label FROM Article_Label WHERE Article = " + articleId);
    }

    [WebMethod]
    public DataSet GetArticleHasLabel(String label)
    {
        return DBHelper.DataSet("SELECT Article.* FROM Article_Label,Article WHERE Label = '" + label + "' AND Id = Article_Label.Article");
    }

}
