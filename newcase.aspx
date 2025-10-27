<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newcase.aspx.cs" Inherits="WebApplication2.newcase" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Case Entry</title>
    <style>
        body {
            font-family: Arial;
            background: #f2fdf5;
            padding: 30px;
        }
        .form-box {
            width: 700px;
            margin: auto;
            background: #fff;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 0 10px #999;
        }
        td {
            padding: 8px;
            vertical-align: top;
        }
        label {
            font-weight: bold;
        }
        .form-control {
            width: 95%;
            padding: 7px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }
        .btn {
            background: #28a745;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }
        .btn:hover {
            background: #1e7e34;
        }
        h2 {
            text-align: center;
            color: #138D75;
        }
        .error {
            color: red;
            font-size: 13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-box">
            <h2>New Case Registration</h2>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                ForeColor="Red" HeaderText="Please fix the following errors:" />

            <table>
                <!-- Patient ID & Date -->
                <tr>
                    <td><label>Patient ID:</label></td>
                    <td><asp:TextBox ID="txtPatientID" runat="server" CssClass="form-control" ReadOnly="true" /></td>
                    <td><label>Date:</label></td>
                    <td><asp:TextBox ID="txtDate" runat="server" CssClass="form-control" ReadOnly="true" /></td>
                </tr>

                <!-- Name -->
                <tr>
                    <td><label>Name:</label></td>
                    <td colspan="3">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Name is required" CssClass="error" Display="Dynamic" />
                    </td>
                </tr>

                <!-- Mobile -->
                <tr>
                    <td><label>Mobile:</label></td>
                    <td colspan="3">
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" MaxLength="10" />
                        <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile"
                            ErrorMessage="Mobile number is required" CssClass="error" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile"
                            ValidationExpression="^[6-9]\d{9}$"
                            ErrorMessage="Enter valid 10-digit mobile number" CssClass="error" Display="Dynamic" />
                    </td>
                </tr>

                <!-- Gender -->
                <tr>
                    <td><label>Gender:</label></td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Male">Male</asp:ListItem>
                            <asp:ListItem Value="Female">Female</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="rblGender"
                            ErrorMessage="Please select gender" CssClass="error" Display="Dynamic" />
                    </td>
                </tr>

                <!-- Age & Weight -->
                <tr>
                    <td><label>Age:</label></td>
                    <td>
                        <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" MaxLength="3" />
                        <asp:RequiredFieldValidator ID="rfvAge" runat="server"
                            ControlToValidate="txtAge" ErrorMessage="Age required" CssClass="error" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revAge" runat="server"
                            ControlToValidate="txtAge" ValidationExpression="^[0-9]{1,3}$"
                            ErrorMessage="Enter valid numeric age" CssClass="error" Display="Dynamic" />
                    </td>
                    <td><label>Weight:</label></td>
                    <td>
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" MaxLength="3" />
                        <asp:RegularExpressionValidator ID="revWeight" runat="server"
                            ControlToValidate="txtWeight" ValidationExpression="^[0-9]{1,3}$"
                            ErrorMessage="Enter valid numeric weight" CssClass="error" Display="Dynamic" />
                    </td>
                </tr>

                <!-- Address -->
                <tr>
                    <td><label>Address:</label></td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                    </td>
                </tr>

                <!-- Disease -->
                <tr>
                    <td><label>Disease:</label></td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlDisease" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Select Disease --" Value="" />
                            <asp:ListItem Value="Diabetes">Diabetes</asp:ListItem>
                            <asp:ListItem Value="Heart">Heart</asp:ListItem>
                            <asp:ListItem Value="Skin">Skin</asp:ListItem>
                            <asp:ListItem Value="Cold">Cold</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDisease" runat="server" ControlToValidate="ddlDisease"
                            InitialValue="" ErrorMessage="Please select a disease" CssClass="error" Display="Dynamic" />
                    </td>
                </tr>

                <!-- Doctor -->
                <tr>
                    <td><label>Doctor:</label></td>
                    <td colspan="3">
                        <asp:TextBox ID="txtDoctor" runat="server" CssClass="form-control" ReadOnly="true" />
                    </td>
                </tr>

                <!-- Fee -->
                <tr>
                    <td><label>Fee:</label></td>
                    <td colspan="3">
                        <asp:TextBox ID="txtFee" runat="server" CssClass="form-control" ReadOnly="true" />
                    </td>
                </tr>

                <!-- Payment -->
                <tr>
                    <td><label>Payment:</label></td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Select Payment Method --" Value="" />
                            <asp:ListItem>Cash</asp:ListItem>
                            <asp:ListItem>Card</asp:ListItem>
                            <asp:ListItem>UPI</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPayment" runat="server" ControlToValidate="ddlPayment"
                            InitialValue="" ErrorMessage="Please select payment method" CssClass="error" Display="Dynamic" />
                    </td>
                </tr>

                <!-- Save Button -->
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <asp:Button ID="btnSave" runat="server" Text="Save Case" CssClass="btn" OnClick="btnSave_Click" />
                        <asp:Button ID="btnPrintNewCase" runat="server" Text="Print New Case" CssClass="btn btn-print" OnClick="btnPrintNewCase_Click" />

                    </td>
                </tr>
            </table>

            <br />
            <asp:Label ID="lblMsg" runat="server" ForeColor="Green" />
        </div>
    </form>
</body>
</html>
