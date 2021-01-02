Write-Host "Generating signing keys..."

$ecKeyFile = "./certs/ec/3594ab85ad5d4784a5c862fc28e53a6b.pem"
$psKeyFile = "./certs/rsa/d327491fa24349389fcf87ea53d2b1d2.pem"
$rsKeyFile = "./certs/rsa/e3626be31f7f48eca843ded4abc4cbf1.pem"

function Split-Key([string] $value, [int] $width)
{
    if ($value -eq $null)
    {
        throw "value parameter cannot be null"
    }

    if ($width -le 0)
    {
        throw "width parameter must be positive"
    }

    $lines = New-Object System.Collections.Generic.List[string]
    for ($i = 0; $i -lt $value.Length; $i += $width)
    {
        $lines.Add($value.Substring($i, [System.Math]::Min($width, $value.Length - $i)))
    }

    return $lines
}

Write-Host -NoNewline "Checking for ECDsa signing key..."
if (Test-Path $ecKeyFile -PathType Leaf)
{
    Write-Host "`tALREADY EXISTS"
}
else
{
    $ecKeyPath = [System.IO.Path]::GetDirectoryName($ecKeyFile)
    New-Item -Path $ecKeyPath -ItemType Directory -Force | Out-Null
    $ecdsa = [System.Security.Cryptography.ECDsa]::Create([System.Security.Cryptography.ECCurve+NamedCurves]::nistP256)
    $ecKey = [System.Convert]::ToBase64String($ecdsa.ExportECPrivateKey())
    Out-File -FilePath $ecKeyFile -Encoding ascii -InputObject "-----BEGIN EC PRIVATE KEY-----"
    Split-Key -value $ecKey -width 64 | ForEach-Object {
        Out-File -FilePath $ecKeyFile -Encoding ascii -Append -InputObject $_
    }
    Out-File -FilePath $ecKeyFile -Encoding ascii -Append -InputObject "-----END EC PRIVATE KEY-----"
    Write-Host "`tGENERATED"
}

Write-Host -NoNewline "Checking for PSA signing key..."
if (Test-Path $psKeyFile -PathType Leaf)
{
    Write-Host "`t`tALREADY EXISTS"
}
else
{
    $psKeyPath = [System.IO.Path]::GetDirectoryName($psKeyFile)
    New-Item -Path $psKeyPath -ItemType Directory -Force | Out-Null
    $rsa = [System.Security.Cryptography.RSA]::Create(2048)
    $psKey = [System.Convert]::ToBase64String($rsa.ExportRSAPrivateKey())
    Out-File -FilePath $psKeyFile -Encoding ascii -InputObject "-----BEGIN RSA PRIVATE KEY-----"
    Split-Key -value $psKey -width 64 | ForEach-Object {
        Out-File -FilePath $psKeyFile -Encoding ascii -Append -InputObject $_
    }
    Out-File -FilePath $psKeyFile -Encoding ascii -Append -InputObject "-----END RSA PRIVATE KEY-----"
    Write-Host "`t`tGENERATED"
}

Write-Host -NoNewline "Checking for RSA signing key..."
if (Test-Path $rsKeyFile -PathType Leaf)
{
    Write-Host "`t`tALREADY EXISTS"
}
else
{
    $rsKeyPath = [System.IO.Path]::GetDirectoryName($rsKeyFile)
    New-Item -Path $rsKeyPath -ItemType Directory -Force | Out-Null
    $rsa = [System.Security.Cryptography.RSA]::Create(2048)
    $rsKey = [System.Convert]::ToBase64String($rsa.ExportRSAPrivateKey())
    Out-File -FilePath $rsKeyFile -Encoding ascii -InputObject "-----BEGIN RSA PRIVATE KEY-----"
    Split-Key -value $rsKey -width 64 | ForEach-Object {
        Out-File -FilePath $rsKeyFile -Encoding ascii -Append -InputObject $_
    }
    Out-File -FilePath $rsKeyFile -Encoding ascii -Append -InputObject "-----END RSA PRIVATE KEY-----"
    Write-Host "`t`tGENERATED"
}
