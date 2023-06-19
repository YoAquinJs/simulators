$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$parentFolder = $scriptPath
$extensionsToRename = ".wasm", ".js", ".data"

function Rename-Files {
    param(
        [Parameter(Mandatory=$true)]
        [string]$path
    )

    $files = Get-ChildItem -File -Path $path -Include * -Recurse | Where-Object {$_.Extension -in $extensionsToRename}

    foreach ($file in $files) {
        $folderName = $file.Directory.Name
        $newFileName = $folderName + $file.Name.Substring($file.Name.IndexOf('.'))
        $newPath = Join-Path -Path $file.Directory.FullName -ChildPath $newFileName

        Write-Host "Renaming $($file.FullName) to $($newPath)"
        Rename-Item -Path $file.FullName -NewName $newFileName
    }
}


Rename-Files -path $parentFolder