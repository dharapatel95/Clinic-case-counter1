<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoctorDashboard.aspx.cs" Inherits="WebApplication2.DoctorDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Dashboard</title>
    <style>
        body {
            font-family: 'Times New Roman', sans-serif;
            background-color: #f4f6f7;
            margin: 0;
            padding: 30px;
        }

        .container {
            background: white;
            padding: 25px;
            border-radius: 12px;
            width: 90%;
            margin: auto;
            box-shadow: 0 6px 20px rgba(0,0,0,0.15);
        }

        h2 {
            color: #117a65;
            text-align: center;
        }

        .stats {
            display: flex;
            justify-content: space-around;
            margin-top: 30px;
        }

        .card {
            background-color: #d1f2eb;
            border-radius: 10px;
            padding: 20px;
            width: 25%;
            text-align: center;
            box-shadow: 0 3px 10px rgba(0,0,0,0.1);
        }

        .grid-section {
            margin-top: 40px;
        }

        .grid-section h3 {
            text-align: center;
            color: #117a65;
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
        }

        .gridview th, .gridview td {
            border: 1px solid #ccc;
            padding: 10px;
            text-align: center;
        }

        .gridview th {
            background-color: #138d75;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>
                <asp:Label ID="lblGreeting" runat="server" Text="Welcome, Doctor!"></asp:Label>
            </h2>

            <div class="stats">
                <div class="card">
                    <h3>Total Cases</h3>
                    <asp:Label ID="lblTotalCases" runat="server" Font-Size="Large"></asp:Label>
                </div>
                <div class="card">
                    <h3>Today's Appointments</h3>
                    <asp:Label ID="lblTodayCases" runat="server" Font-Size="Large"></asp:Label>
                </div>
            </div>

            <div class="grid-section">
                <h3>All Assigned Cases</h3>
                <asp:GridView ID="gvDoctorCases" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                    <Columns>
                        <asp:BoundField DataField="CaseID" HeaderText="Case ID" />
                        <asp:BoundField DataField="PatientID" HeaderText="Patient ID" />
                        <asp:BoundField DataField="Name" HeaderText="Patient Name" />
                        <asp:BoundField DataField="Disease" HeaderText="Disease" />
                        <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" />
                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" />
                        <asp:BoundField DataField="Fee" HeaderText="Fee" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
