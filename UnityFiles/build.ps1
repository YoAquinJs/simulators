$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$parentFolder = $scriptPath
$extensionsToRename = ".wasm", ".js", ".data"
# Function to recursively rename files with specified extensions in subfolders
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

# Start renaming files with specified extensions in the parent folder and its subfolders
Rename-Files -path $parentFolder
