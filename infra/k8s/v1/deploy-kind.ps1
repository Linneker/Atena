param(
    [string]$ClusterName = "atena",
    [string]$ImageName = "atena-api:local",
    [string]$Namespace = "atena",
    [switch]$SkipBuild
)

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RepoRoot  = [System.IO.Path]::GetFullPath((Join-Path $ScriptDir "..\..\.."))
$KindCfg   = Join-Path $RepoRoot "infra\k8s\kind-config.yaml"
$Manifests = Join-Path $RepoRoot "infra\k8s\v1"
$Dockerfile = Join-Path $RepoRoot "src\Api\Acme.Sistemas.Atena.Api\Dockerfile"

Push-Location $RepoRoot
try {
    function Assert-LastExitCode([string]$Step) {
        if ($LASTEXITCODE -ne 0) { throw "$Step failed (exit $LASTEXITCODE)." }
    }

    Write-Host "[1/6] Verificando Docker..."
    docker version --format "{{.Server.APIVersion}}" | Out-Null
    Assert-LastExitCode "Docker check"

    Write-Host "[2/6] Garantindo cluster Kind '$ClusterName'..."
    $clusters = kind get clusters 2>$null
    if (-not ($clusters -contains $ClusterName)) {
        kind create cluster --name $ClusterName --config $KindCfg
        Assert-LastExitCode "Kind cluster creation"
    } else {
        Write-Host "  Cluster '$ClusterName' já existe."
    }

    Write-Host "[3/6] Trocando contexto kubectl..."
    kubectl config use-context "kind-$ClusterName" | Out-Null
    Assert-LastExitCode "kubectl context switch"

    if (-not $SkipBuild) {
        Write-Host "[4/6] Buildando imagem '$ImageName'..."
        docker build -t $ImageName -f $Dockerfile $RepoRoot
        Assert-LastExitCode "Docker build"

        Write-Host "    Carregando imagem no Kind..."
        kind load docker-image $ImageName --name $ClusterName
        Assert-LastExitCode "kind load image"
    }

    Write-Host "[5/6] Aplicando manifests de $Manifests..."
    kubectl apply -f $Manifests
    Assert-LastExitCode "kubectl apply"

    Write-Host "[6/6] Aguardando deployments..."
    kubectl -n $Namespace wait --for=condition=Available deployment/atena-api --timeout=300s
    kubectl -n $Namespace wait --for=condition=Available deployment/atena-nfe-worker --timeout=300s

    Write-Host ""
    Write-Host "Deploy concluído. Para acessar a API:"
    Write-Host "  kubectl -n $Namespace port-forward svc/atena-api 5000:80"
    Write-Host "  curl http://127.0.0.1:5000/health"
}
finally {
    Pop-Location
}
