"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" ^
	.\Lymer.UserUI\Lymer.UserUI.csproj ^
	/p:Platform=AnyCPU ^
	/p:VisualStudioVersion=12.0 ^
	/t:WebPublish ^
	/p:WebPublishMethod=FileSystem ^
	/p:DeleteExistingFiles=True ^
	/p:publishUrl=BuildOutput