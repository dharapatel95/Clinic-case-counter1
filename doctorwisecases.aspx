<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="doctorWiseCases.aspx.cs" Inherits="WebApplication2.doctorWiseCases" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Wise Today Cases</title>
    <style>
        body { font-family: Arial; background: #f2f2f2; padding: 30px; }
        h2 { text-align:center; color:#138D75; }
        .grid { margin:auto; width:80%; background:#fff; padding:20px; border-radius:10px; box-shadow:0 0 10px #999; }
        table { width:100%; border-collapse:collapse; }
        th, td { padding:10px; border:1px solid #ccc; text-align:left; }
        th { background:#28a745; color:#fff; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="grid">
        <h2>Doctor Wise Today's Cases</h2>
        <asp:GridView ID="gvDoctorCases" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="DoctorAssigned" HeaderText="Doctor" />
        <asp:BoundField DataField="TotalCases" HeaderText="Total Patients Today" />
        <asp:TemplateField HeaderText="Patients">
            <ItemTemplate>
                <%# Eval("PatientNames") %>  
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

    </div>
        </form>
</body>
</html>
