<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication2.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clinic Login</title>
    <style> 
        body {
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background: #f2fdf5;
        }
        .login-container {
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0px 8px 20px rgba(0,0,0,0.2);
            width: 500px;
            text-align: center;
            background: #fff;
        }
        .login-container h2 {
            margin-bottom: 30px;
            color: #138D75;
        }
        .form-group {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 18px;
        }
        .form-group label {
            flex: 1;
            font-weight: bold;
            color: #138D75;
            text-align: left;
            margin-right: 15px;
        }
        .form-control {
            flex: 2;
            padding: 12px;
            border-radius: 8px;
            border: 1px solid #ccc;
            outline: none;
            transition: 0.3s;
            font-size: 15px;
        }
        .form-control:focus {
            border-color: #2f8f46;
            box-shadow: 0px 0px 8px rgba(47,143,70,0.5);
        }
        .btn {
            border: none;
            padding: 12px;
            background-color: #138D75;
            color: white;
            border-radius: 8px;
            cursor: pointer;
            font-size: 16px;
            width: 100%;
            transition: 0.3s;
            margin-top: 10px;
        }
        .btn:hover {
            background-color: #256f39;
        }
        .error {
            color: red;
            margin-top: 12px;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Clinic Case Counter</h2>
            
            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            </div>
            
            <asp:Button ID="btnLogin" runat="server" CssClass="btn" Text="Login" OnClick="btnLogin_Click" />
            
            <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
        </div>
    </form>
</body>
</html>
