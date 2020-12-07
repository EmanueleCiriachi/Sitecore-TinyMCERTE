$version = $env:APPVEYOR_BUILD_VERSION
if ($version -eq $null) {
    $version = "1.0.0"
}
"Package version: " + $version

Remove-Item build\package -Recurse -ErrorAction Ignore
Remove-Item build\artifacts -Recurse -ErrorAction Ignore
New-Item -Name build\package -ItemType directory
New-Item -Name build\artifacts -ItemType directory
New-Item -Name build\package\Data -ItemType directory
New-Item -Name build\package\bin -ItemType directory
New-Item -Name build\package\App_Config\Include -ItemType directory


Copy-Item .\bin\debug\TinyMCERTE* .\build\package\bin
Copy-Item .\sitecore\* .\build\package\sitecore -recurse
Copy-Item .\App_Config\Include\TinyEditor.config .\build\package\App_Config\Include
Copy-Item .\App_Config\Include\TinyMce.FieldType.config .\build\package\App_Config\Include
Copy-Item .\serialization\* .\build\package\Data -recurse

$packageCmd = "Sitecore.Courier.Runner.exe -t build\package -o build\artifacts\TinyMCERTE." + $version + ".update -r"
iex $packageCmd