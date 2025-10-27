using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication2
{
    public partial class DDashboard : System.Web.UI.Page
    {
        // Connection string loaded from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is logged in and is a Doctor
                if (Session["Username"] != null && Session["Role"] != null && Session["Role"].ToString() == "Doctor")
                {
                    // Session["Username"] holds the FullName of the doctor. Trim it for safety.
                    string doctorName = Session["Username"].ToString().Trim();
                    lblGreeting.Text = "Welcome Dr. " + doctorName;

                    // Load all required data for the dashboard
                    LoadDashboardData(doctorName);
                }
                else
                {
                    // Redirect if the user is not authenticated or not a doctor
                    Response.Redirect("login.aspx");
                }
            }
        }

        private void LoadDashboardData(string doctorName)
        {
            // --- CRITICAL FIX: Standardize doctor name for robust database comparison ---

            // 1. Convert the session name to lowercase and remove common titles ("Dr.", "Dr ").
            string cleanName = doctorName.Trim().ToLowerInvariant();
            if (cleanName.StartsWith("dr."))
            {
                // Remove "dr." and trim any resulting space
                cleanName = cleanName.Substring(3).Trim();
            }
            else if (cleanName.StartsWith("dr "))
            {
                // Remove "dr " and trim any resulting space
                cleanName = cleanName.Substring(2).Trim();
            }

            // This is the clean, lowercase name (e.g., "joshi") used for comparison.
            string doctorNameForQuery = cleanName;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                try
                {
                    con.Open();

                    // --- 1. Get Total Cases assigned to this doctor (History/Total) ---
                    string totalCasesQuery = @"
                        SELECT COUNT(*) 
                        FROM PatientCases 
                        WHERE LOWER(LTRIM(RTRIM(DoctorAssigned))) LIKE '%' + @DoctorName + '%'";

                    SqlCommand cmdTotal = new SqlCommand(totalCasesQuery, con);
                    cmdTotal.Parameters.AddWithValue("@DoctorName", doctorNameForQuery);

                    lblTotalCases.Text = cmdTotal.ExecuteScalar()?.ToString() ?? "0";

                    // --- 2. Load and Merge Today's Appointments and New Cases (Combined Daily Schedule) ---
                    DataTable dtToday = new DataTable();
                    dtToday.Columns.Add("Source"); // New Case / Appointment
                    dtToday.Columns.Add("CaseID");
                    dtToday.Columns.Add("PatientID");
                    dtToday.Columns.Add("Name");
                    dtToday.Columns.Add("Disease");
                    dtToday.Columns.Add("PaymentType");
                    dtToday.Columns.Add("Fee");

                    // 2a. Today's PatientCases
                    string patientCasesQuery = @"
                        SELECT pc.CaseID, pc.PatientID, p.Name, pc.Disease, pc.PaymentType, pc.Fee
                        FROM PatientCases pc
                        INNER JOIN Patients p ON pc.PatientID=p.PatientID
                        WHERE LOWER(LTRIM(RTRIM(pc.DoctorAssigned))) LIKE '%' + @DoctorName + '%'
                        AND DATEDIFF(day, pc.Date, GETDATE()) = 0";

                    SqlCommand cmdCases = new SqlCommand(patientCasesQuery, con);
                    cmdCases.Parameters.AddWithValue("@DoctorName", doctorNameForQuery);
                    SqlDataReader dr = cmdCases.ExecuteReader();
                    while (dr.Read())
                    {
                        DataRow row = dtToday.NewRow();
                        row["Source"] = "New Case";
                        row["CaseID"] = dr["CaseID"];
                        row["PatientID"] = dr["PatientID"];
                        row["Name"] = dr["Name"];
                        row["Disease"] = dr["Disease"];
                        row["PaymentType"] = dr["PaymentType"];
                        row["Fee"] = dr["Fee"];
                        dtToday.Rows.Add(row);
                    }
                    dr.Close();

                    // 2b. Today's Appointments
                    string appointmentsQuery = @"
                        SELECT a.AppointmentID AS CaseID, a.PatientID, p.Name, 'Appointment' AS Disease, 'N/A' AS PaymentType, 'N/A' AS Fee
                        FROM Appointments a
                        INNER JOIN Patients p ON a.PatientID=p.PatientID
                        WHERE LOWER(LTRIM(RTRIM(a.DoctorAssigned))) LIKE '%' + @DoctorName + '%'
                        AND DATEDIFF(day, a.AppointmentDate, GETDATE()) = 0";

                    SqlCommand cmdAppt = new SqlCommand(appointmentsQuery, con);
                    cmdAppt.Parameters.AddWithValue("@DoctorName", doctorNameForQuery);
                    SqlDataReader dr2 = cmdAppt.ExecuteReader();
                    while (dr2.Read())
                    {
                        DataRow row = dtToday.NewRow();
                        row["Source"] = "Appointment";
                        row["CaseID"] = dr2["CaseID"];
                        row["PatientID"] = dr2["PatientID"];
                        row["Name"] = dr2["Name"];
                        row["Disease"] = dr2["Disease"];
                        row["PaymentType"] = "N/A"; // Appointments typically don't have fee/payment info yet
                        row["Fee"] = "N/A";
                        dtToday.Rows.Add(row);
                    }
                    dr2.Close();

                    // Update Today's Appointments label with the total count of today's items (cases + appointments)
                    lblTodayAppointments.Text = dtToday.Rows.Count.ToString();

                    // Bind combined table to the new GridView
                    gvTodayCases.DataSource = dtToday;
                    gvTodayCases.DataBind();

                    // --- 3. Load all Assigned Cases for the GridView (History) ---
                    string allCasesQuery = @"
                        SELECT pc.CaseID, pc.PatientID, p.Name, pc.Disease, pc.Date, pc.PaymentType, pc.Fee
                        FROM PatientCases pc
                        INNER JOIN Patients p ON pc.PatientID=p.PatientID
                        WHERE LOWER(LTRIM(RTRIM(pc.DoctorAssigned))) LIKE '%' + @DoctorName + '%'
                        ORDER BY pc.Date DESC";

                    SqlDataAdapter da = new SqlDataAdapter(allCasesQuery, con);
                    da.SelectCommand.Parameters.AddWithValue("@DoctorName", doctorNameForQuery);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvDoctorCases.DataSource = dt;
                    gvDoctorCases.DataBind();
                }
                catch (Exception ex)
                {
                    // Log the exception in a real application
                    System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                    // Display an error message to the user
                    lblGreeting.Text += " (Data Loading Error)";
                    // Prevent the application from crashing and show default/error values
                    lblTotalCases.Text = "N/A";
                    lblTodayAppointments.Text = "N/A";
                }
            }
        }
    }
}
