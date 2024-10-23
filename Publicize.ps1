# Install assembly-publicizer: dotnet tool install -g BepInEx.AssemblyPublicizer.Cli
# See: https://github.com/BepInEx/BepInEx.AssemblyPublicizer

$timberbornDir = ".\Packages\Timberborn"
$timberbornDlls = Get-ChildItem $timberbornDir -Filter "Timberborn.*.dll" -Recurse

# Create a backup of the original dlls

$timberbornDlls | ForEach-Object {
    $backupPath = $_.FullName + ".bak"
    Copy-Item $_.FullName $backupPath
}

# Publicize the dlls

#Write-Host "assembly-publicizer --overwrite --publicize-compiler-generated $($timberbornDlls | ForEach-Object { $timberbornDir."\".$($_.Name) } )"
#assembly-publicizer --overwrite --publicize-compiler-generated $($timberbornDlls | ForEach-Object { "$($timberbornDir)\$($_.Name)" } )
$timberbornDlls | ForEach-Object{
    assembly-publicizer --overwrite --publicize-compiler-generated $timberbornDir"\"$_
}

# Restore the original dlls

#$timberbornDlls | ForEach-Object {
#    $backupPath = $_.FullName + ".bak"
#    Copy-Item $backupPath $_.FullName
#    Remove-Item $backupPath
#}
