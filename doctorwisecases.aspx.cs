using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class doctorWiseCases : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDoctorWiseCases();
            }
        }

        private void BindDoctorWiseCases()
        {
            DataTable dtDoctors = new DataTable();
            dtDoctors.Columns.Add("DoctorAssigned", typeof(string));
            dtDoctors.Columns.Add("TotalCases", typeof(int));
            dtDoctors.Columns.Add("PatientNames", typeof(string));

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Get all today patients (New, Old, Appointments)
                string query = @"
    SELECT DoctorAssigned, p.Name
    FROM PatientCases pc
    INNER JOIN Patients p ON pc.PatientID = p.PatientID
    WHERE CAST(pc.Date AS DATE) = CAST(GETDATE() AS DATE)
    
    UNION
    
    SELECT DoctorAssigned, p.Name
    FROM Appointments a
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    WHERE CAST(a.AppointmentDate AS DATE) = CAST(GETDATE() AS DATE)
    
    ORDER BY DoctorAssigned";


                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                Dictionary<string, List<string>> doctorPatients = new Dictionary<string, List<string>>();

                while (dr.Read())
                {
                    string doctor = dr["DoctorAssigned"].ToString();
                    string patient = dr["Name"].ToString();

                    if (!doctorPatients.ContainsKey(doctor))
                        doctorPatients[doctor] = new List<string>();

                    doctorPatients[doctor].Add(patient);
                }
                dr.Close();

                // Fill DataTable
                // Fill DataTable
                // Fill DataTable
                foreach (var kvp in doctorPatients)
                {
                    DataRow row = dtDoctors.NewRow();
                    row["DoctorAssigned"] = kvp.Key;
                    row["TotalCases"] = kvp.Value.Count;
                    row["PatientNames"] = string.Join("<br />", kvp.Value); // single string
                    dtDoctors.Rows.Add(row);
                }

                // Bind GridView directly
                gvDoctorCases.DataSource = dtDoctors;
                gvDoctorCases.DataBind();

                // ✅ No Repeater binding needed anymore




            }
        }
    }
}

