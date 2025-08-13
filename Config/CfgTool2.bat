@echo off
set jsonFilePath=".\Excel\CfgImport2.json"
set exePath=".\ExcelTool\ExcelTool.exe"
%exePath% %jsonFilePath%
pause