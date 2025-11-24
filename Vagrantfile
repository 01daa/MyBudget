Vagrant.configure("2") do |config|
  config.vm.synced_folder ".", "/vagrant", disabled: false
  config.vm.boot_timeout = 600

  # ===== BaGet (NuGet repo) =====
  config.vm.define "repo" do |m|
    m.vm.box = "generic/ubuntu2204"
    m.vm.hostname = "repo"
    m.vm.network "private_network", ip: "10.10.0.10"
    m.vm.network "forwarded_port", guest: 5555, host: 5555, host_ip: "127.0.0.1", auto_correct: true
    m.vm.provider "virtualbox" do |vb|
      vb.memory = 1024
      vb.cpus = 2
    end
    m.vm.provision "shell", inline: <<-SHELL
      set -e
      sudo apt-get update
      sudo apt-get install -y docker.io
      sudo systemctl enable --now docker
      mkdir -p /home/vagrant/baget_data
      cat > /home/vagrant/baget.env <<'EOF'
ApiKey=NUGET-SERVER-API-KEY
Storage__Type=FileSystem
Storage__Path=/var/baget/packages
Database__Type=Sqlite
Database__ConnectionString=Data Source=/var/baget/baget.db
Search__Type=Database
EOF
      sudo docker rm -f baget || true
      sudo docker run -d --restart always --name baget -p 0.0.0.0:5555:80 \
        --env-file /home/vagrant/baget.env \
        -v /home/vagrant/baget_data:/var/baget loicsharma/baget
    SHELL
  end

  # ===== Ubuntu 22.04: билд/пакет + API =====
  config.vm.define "u2204" do |m|
    m.vm.box = "generic/ubuntu2204"
    m.vm.hostname = "u2204"
    m.vm.network "private_network", ip: "10.10.0.21"
    m.vm.network "forwarded_port", guest: 5080, host: 15080, host_ip: "127.0.0.1", auto_correct: true
    m.vm.provider "virtualbox" do |vb|
      vb.memory = 4096
      vb.cpus = 2
    end
    m.vm.provision "shell", inline: <<-SHELL
      set -e
      sudo apt-get update
      sudo apt-get install -y wget apt-transport-https software-properties-common rsync
      wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O /tmp/ms.deb
      sudo dpkg -i /tmp/ms.deb
      sudo apt-get update
      sudo apt-get install -y dotnet-sdk-9.0
      mkdir -p /home/vagrant/app/data
      rsync -a --delete /vagrant/ /home/vagrant/app/
      cd /home/vagrant/app
      dotnet restore --source https://api.nuget.org/v3/index.json --ignore-failed-sources
      # Пакуем ядро в локальный фид (используем Tips, без HTTP)
      dotnet pack src/MyBudget.Core/MyBudget.Core.csproj -c Release -o /vagrant/nupkgs /p:PackageVersion=1.0.0 /p:PackageId=MyBudget.Core
      # Публикуем API и запускаем
      dotnet publish src/MyBudget.Api/MyBudget.Api.csproj -c Release -o out
      sudo pkill -9 -f 'dotnet .*MyBudget.Api.dll' || true
      nohup env ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://0.0.0.0:5080 \
        ConnectionStrings__db='Data Source=/home/vagrant/app/data/mybudget.db' \
        /usr/bin/dotnet /home/vagrant/app/out/MyBudget.Api.dll >/home/vagrant/api.log 2>&1 &
    SHELL
  end

  # ===== Debian 12: консольник, ставим пакет из локального фида =====
  config.vm.define "deb12" do |m|
    m.vm.box = "generic/debian12"
    m.vm.hostname = "deb12"
    m.vm.network "private_network", ip: "10.10.0.31"
    m.vm.network "forwarded_port", guest: 5080, host: 25080, host_ip: "127.0.0.1", auto_correct: true
    m.vm.provider "virtualbox" do |vb|
      vb.memory = 2048
      vb.cpus = 2
    end
    m.vm.provision "shell", inline: <<-SHELL
      set -e
      apt-get update
      apt-get install -y curl ca-certificates gnupg rsync
      curl -fsSL https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -o /tmp/ms.deb
      dpkg -i /tmp/ms.deb
      apt-get update
      apt-get install -y dotnet-sdk-9.0
      rm -rf /home/vagrant/consumer && mkdir -p /home/vagrant/consumer
      cd /home/vagrant/consumer
      dotnet new console -n Consumer -o .
      dotnet nuget add source /vagrant/nupkgs -n local || true
      dotnet add package MyBudget.Core --version 1.0.0 -s /vagrant/nupkgs --no-restore
      dotnet restore --source /vagrant/nupkgs --source https://api.nuget.org/v3/index.json
      # (опционально) запустить копию API на Debian
      rm -rf /home/vagrant/app2 && rsync -a --delete /vagrant/ /home/vagrant/app2/
      cd /home/vagrant/app2 && dotnet restore --source https://api.nuget.org/v3/index.json --ignore-failed-sources
      dotnet publish src/MyBudget.Api/MyBudget.Api.csproj -c Release -o out
      nohup env ASPNETCORE_URLS=http://0.0.0.0:5080 /usr/bin/dotnet /home/vagrant/app2/out/MyBudget.Api.dll >/home/vagrant/api.log 2>&1 &
    SHELL
  end
end
