<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jQuery_GriedCtrl.ascx.cs" Inherits="CCIS.UIComponents.UserControl.jQuery_GriedCtrl" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>jsGrid - Basic Scenario</title>
    <link rel="stylesheet" type="text/css" href="demos.css" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300,600,400' rel='stylesheet' type='text/css'>

    <link rel="stylesheet" type="text/css" href="css/jsgrid.css" />
    <link rel="stylesheet" type="text/css" href="css/theme.css" />

   <%-- <script src="external/jquery/jquery-1.8.3.js"></script>--%>
    <script src="src/db.js"></script>

    <script src="src/jsgrid.core.js"></script>
    <script src="src/jsgrid.load-indicator.js"></script>
    <script src="src/jsgrid.load-strategies.js"></script>
    <script src="src/jsgrid.sort-strategies.js"></script>
    <script src="src/jsgrid.field.js"></script>
    <script src="src/fields/jsgrid.field.text.js"></script>
    <script src="src/fields/jsgrid.field.number.js"></script>
    <script src="src/fields/jsgrid.field.select.js"></script>
    <script src="src/fields/jsgrid.field.checkbox.js"></script>
    <script src="src/fields/jsgrid.field.control.js"></script>


    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        html {
            height: 100%;
        }

        body {
            height: 100%;
            padding: 10px;
            color: #262626;
            font-family: 'Helvetica Neue Light', 'Open Sans', Helvetica;
            font-size: 14px;
            font-weight: 300;
        }

        h1 {
            margin: 0 0 8px 0;
            font-size: 24px;
            font-family: 'Helvetica Neue Light', 'Open Sans', Helvetica;
            font-weight: 300;
        }

        h2 {
            margin: 16px 0 8px 0;
            font-size: 18px;
            font-family: 'Helvetica Neue Light', 'Open Sans', Helvetica;
            font-weight: 300;
        }

        ul {
            list-style: none;
        }

        a {
            color: #2ba6cb;
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
                color: #258faf;
            }

        input, button, select {
            font-family: 'Helvetica Neue Light', 'Open Sans', Helvetica;
            font-weight: 300;
            font-size: 14px;
            padding: 2px;
        }

        .navigation {
            width: 200px;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            padding: 10px;
            border-right: 1px solid #e9e9e9;
        }

        .jsgrid-grid-body{
            height : 500px;
        }

        .navigation li {
            margin: 10px 0;
        }

        .demo-frame {
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            left: 200px;
        }

        iframe[name='demo'] {
            display: block;
            width: 100%;
            height: 100%;
            border: none;
        }
    </style>

</head>
<body>
    <div id="jsGrid"></div>

    <script>
        $(function() {

            $("#jsGrid").jsGrid({
                height: "70%",
                width: "100%",
                filtering: true,
                editing: true,
                inserting: true,
                sorting: true,
                paging: true,
                autoload: true,
                pageSize: 15,
                pageButtonCount: 5,
                deleteConfirm: "Do you really want to delete the client?",
                controller: db,
                fields: [
                    { name: "Name", type: "text", width: 150 },
                    { name: "Age", type: "number", width: 50 },
                    { name: "Address", type: "text", width: 200 },
                    { name: "Country", type: "select", items: db.countries, valueField: "Id", textField: "Name" },
                    { name: "Married", type: "checkbox", title: "Is Married", sorting: false },
                    { type: "control" }
                ]
            });

        });
    </script>
</body>
</html>
