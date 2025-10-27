<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bookappointment.aspx.cs" Inherits="WebApplication2.bookappointment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book Appointment</title>
    <style>
        body {
            font-family: 'Segoe UI';
            background: #f2f7ff;
        }

        .container {
            width: 60%;
            margin: 50px auto;
            background: white;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.1);
            padding: 30px;
        }

        h2 {
            text-align: center;
            color: #0066cc;
        }

        table {
            width: 100%;
            border-spacing: 10px;
        }

        .btn {
            background-color: #007bff;
            color: white;
            padding: 7px 14px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }

            .btn:hover {
                background-color: #0056b3;
            }

        .msg {
            text-align: center;
            margin-top: 15px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>📅 Book Appointment</h2>

            <table>
                <tr>
                    <td>Patient ID:</td>
                    <td>
                        <asp:TextBox ID="txtPatientID" runat="server" CssClass="form-control" />
                        <asp:Button ID="btnFetch" runat="server" Text="🔍 Fetch Details" CssClass="btn" OnClick="btnFetch_Click" />
                    </td>
                </tr>

                <tr>
                    <td>Date:</td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" CssClass="form-control" /></td>
                </tr>

                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" ReadOnly="true" CssClass="form-control" /></td>
                </tr>

                <tr>
                    <td>Mobile:</td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" ReadOnly="true" CssClass="form-control" /></td>
                </tr>

                <tr>
                    <td>Gender:</td>
                    <td>
                        <asp:RadioButtonList ID="rblGender" runat="server" ReadOnly="true" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Male" Value="Male" />
                            <asp:ListItem Text="Female" Value="Female" />
                        </asp:RadioButtonList>
                    </td>
                </tr>

                <tr>
                    <td>Age:</td>
                    <td>
                        <asp:TextBox ID="txtAge" runat="server" ReadOnly="true" CssClass="form-control" /></td>
                </tr>

                <tr>
                    <td>Doctor:</td>
                    <td>
                        <asp:TextBox ID="txtDoctor" runat="server" ReadOnly="true" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td>Payment Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Cash" Value="Cash" />
                            <asp:ListItem Text="UPI" Value="UPI" />
                        </asp:DropDownList>
                    </td>
                </tr>



                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnBook" runat="server" Text="📘 Book Appointment" CssClass="btn" OnClick="btnBook_Click" />
                    </td>
                </tr>
            </table>

            <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
