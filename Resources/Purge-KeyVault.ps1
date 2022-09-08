# Purges the deleted the KeyVault instance.
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $ApiVersion = "2021-10-01",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This permanently deletes the Key Vault instance

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-ApiVersion <API version>] ``

            [-Help]

    Options:
        -ApiVersion     REST API version. Default is `2021-10-01`.

        -Help:          Show this message.
"

    Exit 0
}

# Show usage
$needHelp = $Help -eq $true
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

# List soft-deleted Key Vaults
function List-DeletedKeyVaults {
    param (
        [string] $ApiVersion
    )

    $account = $(az account show | ConvertFrom-Json)

    $url = "https://management.azure.com/subscriptions/$($account.id)/providers/Microsoft.KeyVault/deletedVaults?api-version=$($ApiVersion)"
    
    # Uncomment to debug
    # $url
    
    $options = ""

    $kvs = $(az rest -m get -u $url --query "value" | ConvertFrom-Json)
    if ($kvs -eq $null) {
        $options = "No soft-deleted KeyVault instance found to purge"
        $returnValue = @{ kvs = $kvs; options = $options }
        return $returnValue
    }
    
    if ($kvs.Count -eq 1) {
        $name = $kvs.name
        $options += "    1: $name `n"
    } else {
        $kvs | ForEach-Object {
            $i = $kvs.IndexOf($_)
            $name = $_.name
            $options += "    $($i +1): $name `n"
        }
    }
    $options += "    q: Quit`n"

    $returnValue = @{ kvs = $kvs; options = $options }
    return $returnValue
}

# Purge soft-deleted Key Vaults
function Purge-DeletedKeyVaults {
    param (
        [string] $ApiVersion
    )

    $continue = $true
    $result = List-DeletedKeyVaults -ApiVersion $ApiVersion
    if ($result.kvs -eq $null) {
        $continue = $false
    }

    while ($continue -eq $true) {
        $options = $result.options
        $input = Read-Host "Select the number to purge the soft-deleted KeyVault instance or 'q' to quit: `n`n$options"
        if ($input -eq "q") {
            $continue = $false
            break
        }

        $parsed = $input -as [int]
        if ($parsed -eq $null) {
            Write-Output "Invalid input"
            $continue = $false
            break
        }

        $kvs = $result.kvs
        if ($parsed -gt $kvs.Count) {
            Write-Output "Invalid input"
            $continue = $false
            break
        }
        
        $index = $parsed - 1
        
        $url = "https://management.azure.com$($kvs[$index].id)?api-version=$($ApiVersion)"
        
        # Uncomment to debug
        # $url
        
        $kv = $(az rest -m get -u $url)
        if ($kv -ne $null) {
            $url = "https://management.azure.com$($kvs[$index].id)/purge?api-version=$($ApiVersion)"
            $deleted = $(az rest -m post -u $url)
        }

        $result = List-DeletedKeyVaults -ApiVersion $ApiVersion
        if ($result.kvs -eq $null) {
            $continue = $false
        }
    }

    if ($continue -eq $false) {
        return "`n$result.options"
    }
}

Purge-DeletedKeyVaults -ApiVersion $ApiVersion
