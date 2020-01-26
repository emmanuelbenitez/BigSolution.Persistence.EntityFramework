[CmdletBinding()]
[OutputType([void])]
param(
    [Parameter(Mandatory = $false)]
    [int]
    $Build = ('{0:yy}{1:000}' -f [datetime]::Today, [datetime]::Today.DayOfYear),

    [Parameter(Mandatory = $false)]
    [int]
    $Revision = (([datetime]::Now - [datetime]::Today).TotalSeconds / 1.4),

    [Parameter(Mandatory = $false)]
    [bool]
    $PreRelease = $true,

    [Parameter(Mandatory = $false)]
    [string]
    $PrivateKeyPath
)

#Clear-Host

# build and package solution
#https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages-snupkg
dotnet build -p:DelaySign=false`;PreRelease=true`;Configuration=Release`;Build=$($Build)`;Revision=$($Revision)`;AssemblyOriginatorKeyFile=$($PrivateKeyPath)`;GeneratePackageOnBuild=true