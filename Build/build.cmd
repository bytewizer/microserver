del *.nupkg

NuGet.exe pack ..\Core\MicroServer.Core.csproj -Build -Properties Configuration=Release
NuGet.exe pack ..\Net\SntpService\MicroServer.Net.SntpService.csproj -Build -Properties Configuration=Release
NuGet.exe pack ..\Net\HttpService\MicroServer.Net.HttpService.csproj -Build -Properties Configuration=Release
NuGet.exe pack ..\Net\DnsService\MicroServer.Net.DnsService.csproj -Build -Properties Configuration=Release
NuGet.exe pack ..\Net\DhcpService\MicroServer.Net.DhcpService.csproj -Build -Properties Configuration=Release
NuGet.exe pack ..\ServiceManager\MicroServer.ServiceManager.csproj -Build -Properties Configuration=Release


echo NuGet.exe push SampleDotNetPackage.2.0.0.0.nupkg {NuGetServerUrl}

