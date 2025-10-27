<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oldcase.aspx.cs" Inherits="WebApplication2.oldcase" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Old Case - Search & Update</title>
    <style>
        body {
            font-family: 'Times New Roman';
            background: #f5f5f5;
            padding: 40px;
            color: #333;
        }

        .form-table {
            background: #fff;
            border-radius: 12px;
            padding: 25px 35px;
            box-shadow: 0px 6px 20px rgba(0,0,0,0.15);
            margin: auto;
            width: 90%;
        }

        td {
            padding: 10px 15px;
        }

        .form-control {
            width: 95%;
            padding: 8px;
            border-radius: 6px;
            border: 1px solid #ccc;
        }

        .btn {
            padding: 8px 16px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }

        .btn-search, .btn-update {
            background: #138D75;
            color: white;
        }

            .btn-search:hover, .btn-update:hover {
                background: #53bb95;
            }

        .search-section {
            text-align: center;
            margin-bottom: 40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="search-section">
            <asp:TextBox ID="txtSearchID" runat="server" CssClass="form-control" Width="300px" placeholder="Enter Patient ID"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn btn-search" OnClick="btnSearch_Click" />
            <asp:Button ID="btnPrintOldCaseHistory" runat="server" Text="Print Patient History" CssClass="btn" OnClick="btnPrintOldCaseHistory_Click" />
            <br />
            <br />
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <asp:GridView ID="gvCases" runat="server" AutoGenerateColumns="False" CssClass="form-table"
            DataKeyNames="CaseID,PatientID"
            AutoGenerateSelectButton="True"
            OnSelectedIndexChanged="gvCases_SelectedIndexChanged"
            OnRowEditing="gvCases_RowEditing"
            OnRowUpdating="gvCases_RowUpdating"
            OnRowCancelingEdit="gvCases_RowCancelingEdit">
            <Columns>
                <asp:BoundField DataField="CaseID" HeaderText="Case ID" ReadOnly="true" />
                <asp:BoundField DataField="PatientID" HeaderText="Patient ID" ReadOnly="true" />
                <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="true" />
                <asp:BoundField DataField="Mobile" HeaderText="Mobile" ReadOnly="true" />
                <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="true" />
                <asp:BoundField DataField="Disease" HeaderText="Disease" ReadOnly="true" />
<asp:TemplateField HeaderText="Medication">
    <ItemTemplate>
        <%# Eval("Medication") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtMedication" runat="server" Text='<%# Bind("Medication") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>
                <asp:BoundField DataField="PaymentType" HeaderText="Payment" ReadOnly="true" />
                <asp:BoundField DataField="Fee" HeaderText="Fee" ReadOnly="true" />
                <asp:BoundField DataField="DoctorAssigned" HeaderText="Doctor" ReadOnly="true" />
                <asp:CommandField ShowSelectButton="True" />
                <asp:CommandField ShowEditButton="True" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
