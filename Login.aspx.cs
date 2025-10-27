using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication2
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear old sessions when login page loads
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "⚠ Please enter Username and Password.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT UserId, FullName, Role, Shift FROM Users WHERE Username=@Username AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader dr = cmd.ExecuteReader();
                //Response.Write(" Login query executed.<br>");

                if (dr.Read())
                {
                    // Save user info in session
                    Session["UserId"] = dr["UserId"];
                    Session["Username"] = dr["FullName"]; // Display name
                    Session["Role"] = dr["Role"];
                    Session["Shift"] = dr["Shift"];

                    // Redirect based on role
                    string role = dr["Role"].ToString();
                    if (role == "Doctor")
                        Response.Redirect("DDashboard.aspx");
                    else if (role == "Admin")
                        Response.Redirect("Dashboard.aspx");
                    else // Staff or other roles

                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                   // Response.Write(" No matching user found.<br>");
                   // Response.End();

                    lblError.Text = " Invalid Username or Password!";
                }
            }
        }
    }
}
