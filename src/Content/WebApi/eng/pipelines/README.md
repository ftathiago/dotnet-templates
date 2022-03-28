# PIPELINE TEMPLATEs

## Based on brewtech pipelines

This pipeline purpose is provide code analysis and development/QA environments delivery.
Whenever you open a Pull Request to develop ou release/* branches, the pipeline will:

- Run Dependency Track analysis
- Run SonarQube analysis (with test running)
- Run Docker image build (without pull)

When you merge code at develop and release/* a new code validation will start and if it be successful, then a delivery process will be started, pulling e deploying a new docker imagem/deployment file at k8s.

## Variáveis

### templates/sonar-analysis.yml

- Sonar.ServiceConnection -> *(CodeQuality)*
- WebApi.Sonar.ProjectKey -> *(WebApi-CodeQuality)*
- WebApi.Sonar.ProjectName -> *(WebApi-CodeQuality)*
- WebApi.Sonar.Duplication.Exclusions -> *(WebApi-CodeQuality)*
- WebApi.Sonar.Coverage.Exclusions -> *(WebApi-CodeQuality)*

### templates/csharp-build.yaml

- WebApi.Build.NugetPath -> *(WebApi-CodeQuality)*

### templates/docker.yml

- WebApi.Docker.ServiceConnection -> *(WebApi-NonProd-Docker)*
- WebApi.Docker.RepositoryName -> *(WebApi-NonProd-Docker)*
- WebApi.Docker.ImageTag -> *(WebApi-NonProd-Docker)*

### templates/dtrack.yml

- DTrack.Url -> *(CodeQuality)*
- WebApi.DTrack.ProjectKey -> *(WebApi-CodeQuality)*
- WebApi.DTrack.SolutionFileName -> *(WebApi-CodeQuality)*
- WebApi.DTrack.ApiKey -> *(WebApi-CodeQuality)*

### templates/deploy.yml

- WebApi.DeployFiles.Path -> *(WebApi-NonProd-Deploy)*
- WebApi.kube_azureContainerRegistry -> *(WebApi-NonProd-Deploy)*
- WebApi.kube_ServiceEndpoint -> *(WebApi-NonProd-Deploy)*
- WebApi.kube_namespace -> *(WebApi-NonProd-Deploy)*
- WebApi.kube_deploy_file -> *(WebApi-NonProd-Deploy)*
- WebApi.kube_deploy -> *(WebApi-NonProd-Deploy)*
