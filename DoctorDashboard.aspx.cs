using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication2
{
    public partial class DoctorDashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if session exists
            if (Session["Role"] == null || Session["FullName"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (Session["Role"].ToString() != "Doctor")
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string doctorName = Session["Username"].ToString();
             //   string shift = Session["Shift"] != null ? Session["Shift"].ToString() : "Not Set";
                lblGreeting.Text = "Welcome Dr. " + doctorName;

                //lblGreeting.Text = "Welcome Dr. " + doctorName + " (" + shift + " Shift)";
                LoadDoctorData(doctorName);
            }
        }

        private void LoadDoctorData(string doctorName)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // total cases
                SqlCommand cmdTotal = new SqlCommand(
                    "SELECT COUNT(*) FROM PatientCases WHERE DoctorAssigned=@d", con);
                cmdTotal.Parameters.AddWithValue("@d", doctorName);
                lblTotalCases.Text = cmdTotal.ExecuteScalar().ToString();

                // today's cases
                SqlCommand cmdToday = new SqlCommand(
                    "SELECT COUNT(*) FROM PatientCases WHERE DoctorAssigned=@d AND CONVERT(date, Date)=CONVERT(date, GETDATE())", con);
                cmdToday.Parameters.AddWithValue("@d", doctorName);
                lblTodayCases.Text = cmdToday.ExecuteScalar().ToString();

                // assigned cases list
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT CaseID, PatientID, Name, Disease, Date, PaymentType, Fee 
                      FROM PatientCases 
                      WHERE DoctorAssigned=@d ORDER BY Date DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@d", doctorName);

                DataTable dt = new DataTable();
                da.Fill(dt);
                gvDoctorCases.DataSource = dt;
                gvDoctorCases.DataBind();
            }
        }
    }
}
