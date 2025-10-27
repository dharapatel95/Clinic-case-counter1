using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace WebApplication2
{
    public partial class bookappointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // 🔹 Fetch patient basic details
                string patientQuery = "SELECT Name, Mobile, Gender, Age, Weight, Address FROM Patients WHERE PatientID=@PID";
                SqlCommand cmd = new SqlCommand(patientQuery, con);
                cmd.Parameters.AddWithValue("@PID", txtPatientID.Text.Trim());

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtName.Text = dr["Name"].ToString();
                    txtMobile.Text = dr["Mobile"].ToString();
                    rblGender.SelectedValue = dr["Gender"].ToString();
                    txtAge.Text = dr["Age"].ToString();
                    //txtWeight.Text = dr["Weight"].ToString();
                    //txtAddress.Text = dr["Address"].ToString();
                    dr.Close();

                    // 🔹 Now fetch doctor assigned from PatientCases
                    string doctorQuery = "SELECT TOP 1 DoctorAssigned FROM PatientCases WHERE PatientID=@PID ORDER BY Date DESC";
                    SqlCommand cmdDoctor = new SqlCommand(doctorQuery, con);
                    cmdDoctor.Parameters.AddWithValue("@PID", txtPatientID.Text.Trim());

                    object docObj = cmdDoctor.ExecuteScalar();
                    if (docObj != null)
                    {
                        txtDoctor.Text = docObj.ToString();
                    }
                    else
                    {
                        txtDoctor.Text = "";
                        lblMsg.Text = "⚠ No doctor assigned yet for this patient.";
                        lblMsg.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }
                else
                {
                    lblMsg.Text = "❌ No patient found with this ID.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // 1️⃣ Check if patient already has a case today
                string caseQuery = @"SELECT COUNT(*) FROM PatientCases 
                             WHERE PatientID = @PID AND Date = @Date";
                SqlCommand caseCmd = new SqlCommand(caseQuery, con);
                caseCmd.Parameters.AddWithValue("@PID", txtPatientID.Text.Trim());
                caseCmd.Parameters.AddWithValue("@Date", txtDate.Text.Trim());
                int caseCount = (int)caseCmd.ExecuteScalar();

                if (caseCount > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "⚠ This patient already has a case today. No need to book an appointment.";
                    return;
                }

                // 2️⃣ Check if appointment already booked for today
                string appointmentCheck = @"SELECT COUNT(*) FROM Appointments 
                                    WHERE PatientID = @PID AND AppointmentDate = @Date";
                SqlCommand appCmd = new SqlCommand(appointmentCheck, con);
                appCmd.Parameters.AddWithValue("@PID", txtPatientID.Text.Trim());
                appCmd.Parameters.AddWithValue("@Date", txtDate.Text.Trim());
                int appointmentCount = (int)appCmd.ExecuteScalar();

                if (appointmentCount > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.OrangeRed;
                    lblMsg.Text = "⚠ Appointment already booked for this patient today.";
                    return;
                }

                // 3️⃣ Proceed to book new appointment
                string insertQuery = @"INSERT INTO Appointments 
                               (PatientID, AppointmentDate, DoctorAssigned, Status)
                               VALUES (@PID, @Date, @Doctor, @Status)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@PID", txtPatientID.Text.Trim());
                insertCmd.Parameters.AddWithValue("@Date", txtDate.Text.Trim());
                insertCmd.Parameters.AddWithValue("@Doctor", txtDoctor.Text.Trim());
                insertCmd.Parameters.AddWithValue("@Status", "Booked");

                insertCmd.ExecuteNonQuery();

                // 🔹 After booking appointment, update payment and fees in PatientCases
                string updateQuery = @"UPDATE PatientCases 
                       SET PaymentType = @PaymentType, 
                           Fee = 100, 
                           Date = @Date 
                       WHERE PatientID = @PID";


                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@PaymentType", ddlPaymentType.SelectedValue);
                updateCmd.Parameters.AddWithValue("@Date", txtDate.Text.Trim());
                updateCmd.Parameters.AddWithValue("@PID", txtPatientID.Text.Trim());

                updateCmd.ExecuteNonQuery();




                //lblMsg.ForeColor = System.Drawing.Color.Green;
                //lblMsg.Text = "✅ Appointment booked and payment info updated successfully!";

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "✅ Appointment booked successfully!";
            }
        }

    }
}
