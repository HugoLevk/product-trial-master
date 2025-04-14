# Créer le répertoire https s'il n'existe pas
New-Item -ItemType Directory -Force -Path "https"

# Générer le certificat de développement
dotnet dev-certs https -ep https/aspnetapp.pfx -p YourSecurePassword123!
dotnet dev-certs https --trust

Write-Host "Certificat généré avec succès dans le dossier https/" 