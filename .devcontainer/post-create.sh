## Install apt packages
sudo apt-get update \
    && sudo apt-get install -y dos2unix

## Configure git
git config --global pull.rebase false
git config --global core.autocrlf false

## Install node.js 16 LTS
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash
export NVM_DIR="$HOME/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"  # This loads nvm
[ -s "$NVM_DIR/bash_completion" ] && \. "$NVM_DIR/bash_completion"  # This loads nvm bash_completion
nvm install --lts

## Enable local HTTPS for .NET
dotnet dev-certs https --trust

# Install Bicep
az bicep install

## Install oh-my-zsh
# sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)"

git clone https://github.com/zsh-users/zsh-completions.git $HOME/.oh-my-zsh/custom/plugins/zsh-completions
git clone https://github.com/zsh-users/zsh-syntax-highlighting.git $HOME/.oh-my-zsh/custom/plugins/zsh-syntax-highlighting
git clone https://github.com/zsh-users/zsh-autosuggestions.git $HOME/.oh-my-zsh/custom/plugins/zsh-autosuggestions

git clone https://github.com/romkatv/powerlevel10k.git "$HOME/.oh-my-zsh/custom/themes/powerlevel10k" --depth=1
ln -s "$HOME/.oh-my-zsh/custom/themes/powerlevel10k/powerlevel10k.zsh-theme" "$HOME/.oh-my-zsh/custom/themes/powerlevel10k.zsh-theme"

# . ~/.zshrc
