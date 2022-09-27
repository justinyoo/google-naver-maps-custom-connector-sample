# Find the Dockerfile at this URL
# https://github.com/Azure/azure-functions-docker/blob/dev/host/4/bullseye/amd64/dotnet/dotnet-inproc/dotnet-core-tools.Dockerfile
FROM mcr.microsoft.com/azure-functions/dotnet:4-dotnet6-core-tools

# Uncomment following lines If you want to enable Development Container Script
# For more details https://github.com/microsoft/vscode-dev-containers/tree/main/script-library

# Avoid warnings by switching to noninteractive
ENV DEBIAN_FRONTEND=noninteractive

# Uncomment out these lines if you want to use zsh.

ARG INSTALL_ZSH=true
ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=$USER_UID

RUN apt-get update && curl -ssL https://raw.githubusercontent.com/microsoft/vscode-dev-containers/main/script-library/common-debian.sh -o /tmp/common-script.sh \
    && /bin/bash /tmp/common-script.sh "$INSTALL_ZSH" "$USERNAME" "$USER_UID" "$USER_GID" \
    && rm /tmp/common-script.sh

# Install PowerShell
RUN apt-get update && apt install -y gnupg \
    && curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add - \
    && sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-debian-bullseye-prod bullseye main" > /etc/apt/sources.list.d/microsoft.list' \
    && apt-get update && apt-get install -y powershell

# Install Bicep
RUN az bicep install
