<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebApplication2.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Dashboard</title>
    <style>
        body {
            font-family: 'Times New Roman', sans-serif;
            background: #f5f5f5;
            padding: 40px;
            color: #333;
        }

        .header {
            text-align: center;
            margin-bottom: 30px;
        }

        .greeting {
            font-size: 22px;
            font-weight: bold;
            color: #138D75;
        }

        .counts {
            margin: 15px 0;
            font-size: 16px;
            color: #000;
        }

        .buttons {
            text-align: center;
            margin-bottom: 25px;
        }

        .btn {
            padding: 10px 20px;
            margin: 5px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
        }

        .btn-new {
            background: #28a745;
            color: white;
        }

        .btn-old {
            background: #007bff;
            color: white;
        }

        .btn:hover {
            opacity: 0.9;
        }

        iframe {
            width: 100%;
            height: 600px;
            border: 1px solid #ccc;
            border-radius: 5px;
            background: #fff;
        }
    </style>

    <script type="text/javascript">
        function loadPage(page) {
            document.getElementById("mainFrame").src = page;
        }
    </script>
</head>
<body>
    <div class="header">
        <div class="greeting">
            <asp:Label ID="lblUserInfo" runat="server"></asp:Label>
        </div>
        <div class="counts">
            <asp:Label ID="lblNewCasesCount" runat="server"></asp:Label> | 
            <asp:Label ID="lblOldCasesCount" runat="server"></asp:Label>
        </div>
    </div>

    <div class="buttons">
    <button class="btn btn-new" onclick="loadPage('newcase.aspx')">📝 New Case</button>
    <button class="btn btn-old" onclick="loadPage('oldcase.aspx')">📁 Old Case</button>
    <button class="btn btn-new" onclick="loadPage('bookappointment.aspx')">📞 Book Appointment</button>
    <button class="btn btn-old" onclick="loadPage('doctorwisecases.aspx')">👨‍⚕️ Today's Doctor Cases</button>
</div>


    <iframe id="mainFrame" runat="server" src=""></iframe>
</body>
</html>
