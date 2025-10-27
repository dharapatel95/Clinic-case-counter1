using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web;

namespace WebApplication2
{
    public partial class oldcase : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchID.Text))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "⚠ Enter a Patient ID to search!";
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = @"
                    SELECT pc.CaseID, pc.PatientID, p.Name, p.Mobile, pc.Date, pc.Disease, pc.Medication, pc.PaymentType, pc.Fee, pc.DoctorAssigned
                    FROM PatientCases pc
                    INNER JOIN Patients p ON pc.PatientID = p.PatientID
                    WHERE pc.PatientID = @PID
                    ORDER BY pc.Date DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@PID", txtSearchID.Text.Trim());

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvCases.DataSource = dt;
                    gvCases.DataBind();
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Text = $" Patient found: {txtSearchID.Text.Trim()}";
                }
                else
                {
                    gvCases.DataSource = null;
                    gvCases.DataBind();
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = " No patient found with this ID.";
                }
            }
        }

        protected void btnPrintOldCaseHistory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchID.Text))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = " Select a patient first!";
                return;
            }

            string report = "";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string patientID = txtSearchID.Text.Trim();
                string patientName = "N/A";
                string patientMobile = "N/A";

                // Patient details
                using (SqlCommand cmdDetails = new SqlCommand("SELECT Name, Mobile FROM Patients WHERE PatientID=@PID", con))
                {
                    cmdDetails.Parameters.AddWithValue("@PID", patientID);
                    using (SqlDataReader drDetails = cmdDetails.ExecuteReader())
                    {
                        if (drDetails.Read())
                        {
                            patientName = drDetails["Name"].ToString();
                            patientMobile = drDetails["Mobile"].ToString();
                        }
                        drDetails.Close();
                    }
                }

                // Case history
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT Date, Disease, Medication, DoctorAssigned, Fee, PaymentType
                    FROM PatientCases
                    WHERE PatientID=@PID
                    ORDER BY Date DESC", con))
                {
                    cmd.Parameters.AddWithValue("@PID", patientID);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.HasRows)
                        {
                            lblMsg.ForeColor = System.Drawing.Color.OrangeRed;
                            lblMsg.Text = " No old medical history found for this patient.";
                            return;
                        }

                        string currentDateString = DateTime.Now.ToString("dd-MM-yyyy HH:mm");

                        report += "<style>" +
                                  "body { font-family: Arial, sans-serif; margin: 20px; font-size: 11pt; }" +
                                  ".header { text-align: center; margin-bottom: 20px; }" +
                                  "table { width: 100%; border-collapse: collapse; }" +
                                  "th, td { border: 1px solid #ddd; padding: 8px; }" +
                                  "th { background-color: #f0f0f0; }" +
                                  "</style>";

                        report += $"<div class='header'><h2>Patient Case History - {patientName}</h2></div>";
                        report += $"<div><strong>Patient ID:</strong> {patientID}<br/><strong>Mobile:</strong> {patientMobile}<br/><strong>Report Date:</strong> {currentDateString}</div>";

                        report += "<h3>Case History</h3><table><tr><th>Date</th><th>Disease</th><th>Medication</th><th>Doctor</th><th>Fee</th><th>Payment</th></tr>";

                        while (dr.Read())
                        {
                            report += "<tr>";
                            report += $"<td>{Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd")}</td>";
                            report += $"<td>{dr["Disease"]}</td>";
                            report += $"<td>{dr["Medication"].ToString().Replace("\r\n", "<br/>")}</td>";
                            report += $"<td>{dr["DoctorAssigned"]}</td>";
                            report += $"<td>{dr["Fee"]}</td>";
                            report += $"<td>{dr["PaymentType"]}</td>";
                            report += "</tr>";
                        }
                        report += "</table>";

                        string escapedReport = HttpUtility.JavaScriptStringEncode(report, false);
                        string js = $"var w = window.open('', '_blank'); w.document.write('{escapedReport}'); w.document.close(); w.print();";

                        ClientScript.RegisterStartupScript(this.GetType(), "PrintOldHistory", js, true);
                        lblMsg.ForeColor = System.Drawing.Color.Blue;
                        lblMsg.Text = $" Printing history for Patient ID: {patientID}...";
                    }
                }
            }
        }

        private void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT pc.CaseID, pc.PatientID, p.Name, p.Mobile, pc.Date, pc.Disease, pc.Medication, pc.PaymentType, pc.Fee, pc.DoctorAssigned
                    FROM PatientCases pc
                    INNER JOIN Patients p ON pc.PatientID = p.PatientID
                    ORDER BY pc.Date DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCases.DataSource = dt;
                gvCases.DataBind();
            }
        }

        protected void gvCases_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCases.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvCases_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string caseId = gvCases.DataKeys[e.RowIndex].Values["CaseID"].ToString();
            GridViewRow row = gvCases.Rows[e.RowIndex];

            // Safely find the TextBox in the TemplateField
            TextBox txtMedication = (TextBox)row.FindControl("txtMedication");
            string medication = txtMedication.Text.Trim();

            DateTime currentDate = DateTime.Today;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string updateQuery = @"
            UPDATE PatientCases
            SET Date=@Date,
                Medication=@Medication
            WHERE CaseID=@CaseID";

                SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);
                cmdUpdate.Parameters.AddWithValue("@CaseID", caseId);
                cmdUpdate.Parameters.AddWithValue("@Medication", medication);
                cmdUpdate.Parameters.AddWithValue("@Date", currentDate);

                cmdUpdate.ExecuteNonQuery();
            }

            gvCases.EditIndex = -1;
            BindGrid();
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = " Old case updated successfully!";
        }


        protected void gvCases_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCases.EditIndex = -1;
            BindGrid();
            lblMsg.Text = "";
        }

        protected void gvCases_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = gvCases.SelectedIndex;
            string patientID = gvCases.DataKeys[index].Values["PatientID"].ToString();
            txtSearchID.Text = patientID;
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = $" Patient selected: {patientID}";
        }
    }
}
