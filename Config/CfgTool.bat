@echo off
set jsonFilePath=".\Excel\CfgImport.json"
set exePath=".\net8.0\ExcelTool.exe"
%exePath% %jsonFilePath%
pause