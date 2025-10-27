using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace WebApplication2
{
    public partial class newcase : System.Web.UI.Page
    {
        // Disease → Doctor + Fee mapping
        private static readonly Dictionary<string, (string Doctor, int Fee)> diseaseMapping =
            new Dictionary<string, (string, int)>()
            {
                { "Diabetes", ("Dr. Sharma", 600) },
                { "Heart", ("Dr. Verma", 800) },
                { "Skin", ("Dr. Joshi", 450) },
                { "Cold", ("Dr. Rao", 300) },
                { "Other", ("Dr. General", 500) }
            };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtPatientID.Text = "";
            }
        }

        protected void ddlDisease_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignDoctorAndFee(); // whenever disease changes
        }

        private void AssignDoctorAndFee()
        {
            if (!string.IsNullOrEmpty(ddlDisease.SelectedValue) && diseaseMapping.ContainsKey(ddlDisease.SelectedValue))
            {
                var d = diseaseMapping[ddlDisease.SelectedValue];
                txtDoctor.Text = d.Doctor;
                txtFee.Text = d.Fee.ToString();
            }
            else
            {
                txtDoctor.Text = "";
                txtFee.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // ✅ Always auto-assign doctor & fee before saving
            AssignDoctorAndFee();

            if (string.IsNullOrEmpty(txtDoctor.Text))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "⚠ Please select a valid disease.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // 1️⃣ Check if patient already exists
                string getPatient = "SELECT PatientID FROM Patients WHERE Name=@Name AND Mobile=@Mobile";
                SqlCommand cmdPatient = new SqlCommand(getPatient, con);
                cmdPatient.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmdPatient.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                object pidObj = cmdPatient.ExecuteScalar();

                string patientID;

                if (pidObj != null)
                {
                    patientID = pidObj.ToString();
                    txtPatientID.Text = patientID;

                    // 2️⃣ Check if same disease already exists
                    string checkCase = "SELECT COUNT(*) FROM PatientCases WHERE PatientID=@PID AND Disease=@Disease";
                    SqlCommand cmdCase = new SqlCommand(checkCase, con);
                    cmdCase.Parameters.AddWithValue("@PID", patientID);
                    cmdCase.Parameters.AddWithValue("@Disease", ddlDisease.SelectedValue);
                    int exists = (int)cmdCase.ExecuteScalar();

                    if (exists > 0)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "❌ This patient already has a case for this disease.";
                        return;
                    }
                }
                else
                {
                    // 3️⃣ Insert new patient
                    patientID = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
                    txtPatientID.Text = patientID;

                    string insertPatient = @"INSERT INTO Patients (PatientID, Name, Mobile, Gender, Age, Weight, Address) 
                                            VALUES (@PID,@Name,@Mobile,@Gender,@Age,@Weight,@Address)";
                    SqlCommand cmdInsert = new SqlCommand(insertPatient, con);
                    cmdInsert.Parameters.AddWithValue("@PID", patientID);
                    cmdInsert.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Gender", rblGender.SelectedValue);
                    cmdInsert.Parameters.AddWithValue("@Age", txtAge.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Weight", txtWeight.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmdInsert.ExecuteNonQuery();
                }

                // 4️⃣ Insert case with auto doctor + fee
                string insertCase = @"INSERT INTO PatientCases (PatientID, Date, Disease, Medication, PaymentType, Fee, DoctorAssigned) 
                                      VALUES (@PID,@Date,@Disease,@Medication,@Payment,@Fee,@Doctor)";
                SqlCommand cmdInsertCase = new SqlCommand(insertCase, con);
                cmdInsertCase.Parameters.AddWithValue("@PID", patientID);
                cmdInsertCase.Parameters.AddWithValue("@Date", txtDate.Text);
                cmdInsertCase.Parameters.AddWithValue("@Disease", ddlDisease.SelectedValue);
                cmdInsertCase.Parameters.AddWithValue("@Medication", "");
                cmdInsertCase.Parameters.AddWithValue("@Payment", ddlPayment.SelectedValue);
                cmdInsertCase.Parameters.AddWithValue("@Fee", txtFee.Text);
                cmdInsertCase.Parameters.AddWithValue("@Doctor", txtDoctor.Text);
                cmdInsertCase.ExecuteNonQuery();

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "✅ Case saved successfully!";
            }
        }


        protected void btnPrintNewCase_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientID.Text))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "⚠ Select a patient first!";
                return;
            }

            string report = "";
            string cs = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string patientID = txtPatientID.Text.Trim();
                    string patientName = "N/A";
                    string patientMobile = "N/A";

                    // Fetch patient details
                    string patientQuery = "SELECT Name, Mobile FROM Patients WHERE PatientID=@PID";
                    using (SqlCommand cmdDetails = new SqlCommand(patientQuery, con))
                    {
                        cmdDetails.Parameters.AddWithValue("@PID", patientID);
                        using (SqlDataReader drDetails = cmdDetails.ExecuteReader())
                        {
                            if (drDetails.Read())
                            {
                                patientName = drDetails["Name"].ToString();
                                patientMobile = drDetails["Mobile"].ToString();
                            }
                        }
                    }

                    // Fetch today's new case
                    string newCaseQuery = @"
                SELECT Date, Disease, Medication, DoctorAssigned, Fee, PaymentType
                FROM PatientCases
                WHERE PatientID=@PID AND CAST(Date AS DATE)=CAST(GETDATE() AS DATE)
                ORDER BY Date DESC";

                    using (SqlCommand cmd = new SqlCommand(newCaseQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@PID", patientID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.HasRows)
                            {
                                lblMsg.ForeColor = System.Drawing.Color.OrangeRed;
                                lblMsg.Text = "⚠ No new case found for this patient today.";
                                return;
                            }

                            string currentDateString = DateTime.Now.ToString("dd-MM-yyyy HH:mm");

                            // CSS for printing cards
                            report += "<style>" +
                                      "body{font-family:Arial,sans-serif;margin:20px;font-size:11pt;color:#000;}" +
                                      ".header{border-bottom:2px solid #333;padding-bottom:10px;margin-bottom:20px;text-align:center;}" +
                                      ".case-card{border:1px solid #ccc;padding:15px;margin-bottom:20px;border-radius:5px;background:#f9f9f9;}" +
                                      ".case-card strong{display:inline-block;width:120px;}" +
                                      "</style>";

                            // Header
                            report += "<div class='header'><h1>CLINIC NAME / NEW CASE REPORT</h1></div>";

                            // Print each case as a card
                            while (dr.Read())
                            {
                                report += "<div class='case-card'>";
                                report += $"<strong>Patient ID:</strong> {patientID}<br/>";
                                report += $"<strong>Patient Name:</strong> {patientName}<br/>";
                                report += $"<strong>Mobile:</strong> {patientMobile}<br/>";
                                report += $"<strong>Date:</strong> {Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd")}<br/>";
                                report += $"<strong>Disease:</strong> {dr["Disease"]}<br/>";
                                report += $"<strong>Medication/Treatment:</strong> {dr["Medication"].ToString().Replace("\r\n", "<br/>")}<br/>";
                                report += $"<strong>Doctor:</strong> {dr["DoctorAssigned"]}<br/>";
                                report += $"<strong>Fee:</strong> {dr["Fee"]} Rs<br/>";
                                report += $"<strong>Payment Type:</strong> {dr["PaymentType"]}<br/>";
                                report += "</div>";
                            }

                            string escapedReport = System.Web.HttpUtility.JavaScriptStringEncode(report, false);
                            string js = $"var w = window.open('', '_blank'); w.document.write('{escapedReport}'); w.document.close(); w.print();";
                            ClientScript.RegisterStartupScript(this.GetType(), "PrintNewCase", js, true);

                            lblMsg.ForeColor = System.Drawing.Color.Blue;
                            lblMsg.Text = $"📜 Printing new case for Patient ID: {patientID}...";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = $"Error printing new case: {ex.Message}";
                }
            }
        }


    }

}

