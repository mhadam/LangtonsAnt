function Get-ScriptDirectory {
    Split-Path -parent $PSCommandPath
}

$exeFolder = [io.path]::combine((Get-ScriptDirectory), 'LangtonsAnt\bin\Release\')
$command = [io.path]::combine($exeFolder, 'LangtonsAnt.exe')
$arg1 = 'LLRR 50000000 E'
$arg2 = 'LRLRRLRRRLRRRR 100000000 N'
Write-Host $command

Read-Host -Prompt "Press Enter to continue"

Write-Host "Running script..."
Start-Process $command -ArgumentList $arg1 -Wait
Start-Process $command -ArgumentList $arg2 -Wait

$scriptDirectory = Get-ScriptDirectory

# create destination folder
$destinationFolder = [io.path]::combine($scriptDirectory, 'outputFiles')
New-Item -Force $destinationFolder -type directory

$outputFiles = join-path -path $debugFolder -childpath "*.png"
Move-Item -Force -Path $outputFiles -Include '*.png' -Destination $destinationFolder