for /f "delims=_" %%f in ('dir /b "..\*.proto"') do (
.\protoc.exe -I=.. --csharp_out=..\Compiled "..\%%f"
)
pause