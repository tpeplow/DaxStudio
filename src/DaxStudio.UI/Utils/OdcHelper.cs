﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaxStudio.UI.Utils
{
    public static class OdcHelper
    {
        public static void CreateOdcFile(string datasource, string database, string cube)
        {
            string odcHeader = @"
<html xmlns:o=""urn:schemas-microsoft-com:office:office""
xmlns=""http://www.w3.org/TR/REC-html40"">

<head>
<meta http-equiv=Content-Type content=""text/x-ms-odc; charset=utf-8"">
<meta name=ProgId content=ODC.Cube>
<meta name=SourceType content=OLEDB>
<meta name=Catalog content=PRS>
<meta name=Table content=Model>
<title>DAX Studio ODC</title>
<xml id=docprops><o:DocumentProperties
  xmlns:o=""urn:schemas-microsoft-com:office:office""
  xmlns=""http://www.w3.org/TR/REC-html40"">
  <o:Name>mtbsql608v-dev_mssqlinst01 PRS Model</o:Name>
 </o:DocumentProperties>
</xml>";
            var odcBody = @"<xml id=msodc><odc:OfficeDataConnection
  xmlns:odc=""urn:schemas-microsoft-com:office:odc""
  xmlns=""http://www.w3.org/TR/REC-html40"">
  <odc:Connection odc:Type=""OLEDB"">
   <odc:ConnectionString>Provider=MSOLAP;Integrated Security=SSPI;Persist Security Info=True;Data Source={0};Update Isolation Level=2;Initial Catalog={1}</odc:ConnectionString>
   <odc:CommandType>Cube</odc:CommandType>
   <odc:CommandText>{2}</odc:CommandText>
  </odc:Connection>
 </odc:OfficeDataConnection>
</xml>";

var odcFooter = @"
<style>
<!--
    .ODCDataSource
    {
    behavior: url(dataconn.htc);
    }
-->
</style>
 
</head>

<body onload='init()' scroll=no leftmargin=0 topmargin=0 rightmargin=0 style='border: 0px'>
<table style='border: solid 1px threedface; height: 100%; width: 100%' cellpadding=0 cellspacing=0 width='100%'> 
  <tr> 
    <td id=tdName style='font-family:arial; font-size:medium; padding: 3px; background-color: threedface'> 
      &nbsp; 
    </td> 
     <td id=tdTableDropdown style='padding: 3px; background-color: threedface; vertical-align: top; padding-bottom: 3px'>

      &nbsp; 
    </td> 
  </tr> 
  <tr> 
    <td id=tdDesc colspan='2' style='border-bottom: 1px threedshadow solid; font-family: Arial; font-size: 1pt; padding: 2px; background-color: threedface'>

      &nbsp; 
    </td> 
  </tr> 
  <tr> 
    <td colspan='2' style='height: 100%; padding-bottom: 4px; border-top: 1px threedhighlight solid;'> 
      <div id='pt' style='height: 100%' class='ODCDataSource'></div> 
    </td> 
  </tr> 
</table> 

  
<script language='javascript'> 

function init() { 
  var sName, sDescription; 
  var i, j; 
  
  try { 
    sName = unescape(location.href) 
  
    i = sName.lastIndexOf(""."") 
    if (i>=0) { sName = sName.substring(1, i); } 
  
    i = sName.lastIndexOf(""/"") 
    if (i>=0) { sName = sName.substring(i+1, sName.length); } 

    document.title = sName; 
    document.getElementById(""tdName"").innerText = sName; 

    sDescription = document.getElementById(""docprops"").innerHTML; 
  
    i = sDescription.indexOf(""escription>"") 
    if (i>=0) { j = sDescription.indexOf(""escription>"", i + 11); } 

    if (i>=0 && j >= 0) { 
      j = sDescription.lastIndexOf(""</"", j); 

      if (j>=0) { 
          sDescription = sDescription.substring(i+11, j); 
        if (sDescription != """") { 
            document.getElementById(""tdDesc"").style.fontSize=""x-small""; 
          document.getElementById(""tdDesc"").innerHTML = sDescription; 
          } 
        } 
      } 
    } 
  catch(e) { 

    } 
  } 
</script> 

</body> 
 
</html>

";

            var odcPath = OdcFilePath();
            File.WriteAllText(odcPath, odcHeader + string.Format(odcBody, datasource, database, cube) + odcFooter);

        }

        public static string OdcFilePath()
        {
            // TODO - should we write to MyDocuments or ApplicationData ??
            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create);
            var dsPath = Path.Combine(myDocs, "My Data Sources", "DaxStudio.odc");
            
            // The following line is only here to fix up an issue caused by a preview release of this feature 
            // which was creating a DaxStudio.odc subfolder which then block creation of this as a file
            if (Directory.Exists(dsPath)) Directory.Delete(dsPath);
            
            // ensure that the folder exists
            Directory.CreateDirectory(Path.GetDirectoryName(dsPath));
            return dsPath;
        }
    }
}
