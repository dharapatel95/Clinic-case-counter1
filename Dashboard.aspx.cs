using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApplication2
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                // Greetings
                string username = Session["Username"].ToString();
                string shift = Session["Shift"]?.ToString() ?? "Day";
                lblUserInfo.Text = $"Jay Shree Krishna! Welcome, {username} – {shift} Shift";

                // Load counts
                lblNewCasesCount.Text = $"New Cases Today: {GetTodayNewCaseCount()}/40";
                lblOldCasesCount.Text = $"Old Cases: {GetOldCaseCount()}";
            }
        }

        private int GetTodayNewCaseCount()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(*) FROM PatientCases WHERE CAST(Date AS DATE) = CAST(GETDATE() AS DATE) AND Fee<>100";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count;
            }
        }

        private int GetOldCaseCount()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(*) FROM PatientCases WHERE Fee=100";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count;
            }
        }
    }
}
