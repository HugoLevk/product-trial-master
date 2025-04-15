# Alten Test Technique - Back-end & Front-end

Ce projet est une application web e-commerce complète, développée avec .NET 9.0 pour le back-end (API REST) et Angular pour le front-end. Il implémente les fonctionnalités demandées dans le test technique d'Alten.

## Architecture

### Back-end
Le projet suit une architecture en couches :
- **AltenTest.API** : Couche API, points d'entrée et contrôleurs
- **AltenTest.Application** : Couche application, services et DTOs
- **AltenTest.Domain** : Couche domaine, entités et interfaces
- **AltenTest.Infrastructure** : Couche infrastructure, persistance et implémentations

### Front-end
Le projet front-end est une application Angular avec PrimeNG pour les composants UI.

## Prérequis

- Docker et Docker Compose
- .NET 9.0 SDK  
- Make (optionnel, pour utiliser les commandes du Makefile)

## Installation et démarrage

1. Cloner le dépôt
2. Se placer dans le dossier du projet
3. Exécuter les commandes suivantes :

```bash
# Utiliser le Makefile (recommandé)
make fresh

# Ou sans Makefile
docker-compose build
docker-compose up -d
``` 

## Accès aux services

- **Application front-end** : http://localhost:4200
- **Swagger UI** : http://localhost:5000/index.html
- **Base de données SQL Server** : localhost:1433
  - Utilisateur : sa
  - Mot de passe : YourStrong!Password
  - Base de données : AltenTestDb

## Identifiants administrateur

- **Email** : admin@admin.com
- **Mot de passe** : Admin123!

## Fonctionnalités implémentées

### Back-end

#### Partie 1 : Gestion des produits
- API REST complète pour les produits (CRUD)
- Base de données SQL Server pour la persistance

#### Partie 2 : Authentification et autorisations
- Authentification JWT
- Création de compte utilisateur
- Connexion utilisateur
- Restriction des opérations de produits à l'administrateur
- Gestion de panier d'achat
- Gestion de liste d'envie

### Front-end

#### Partie 1 : Shop
- Affichage des détails des produits dans la liste
- Ajout de produits au panier depuis la liste
- Suppression de produits du panier
- Badge indiquant la quantité de produits dans le panier
- Visualisation du contenu du panier avec les détails des produits

#### Partie 2 : Contact
- Nouveau point de menu "Contact" dans la barre latérale
- Page formulaire de contact avec champs email et message
- Validation des champs (email obligatoire, message obligatoire et < 300 caractères)
- Message de confirmation après envoi du formulaire

#### Bonus
- Possibilité d'ajuster la quantité des produits depuis le panier

### Fonctionnalités supplémentaires
- Middleware de gestion d'erreurs
- Configuration Docker pour le développement

## Compte rendu du travail réalisé

- **Jeudi** : 3h de travail
  - Mise en place du projet back-end
  - Gestion des produits
  - Authentification

- **Vendredi** : 1h de travail
  - Autorisations back-end

- **Dimanche** : 1h30 de travail
  - Mise en place Docker

- **Lundi** : 2h de travail
  - Cart & Wishlist back-end

- **Mardi** : 7h de travail
  - Création de l'application front-end Angular
  - Implémentation de la liste des produits
  - Implémentation du panier
  - Création du formulaire de contact
  - Corrections diverses sur le back-end
  - Configuration CORS et intégration back/front

## Commandes utiles (Makefile)

```bash
make up          # Démarre les conteneurs
make down        # Arrête les conteneurs
make restart     # Redémarre les conteneurs
make logs        # Affiche les logs de tous les conteneurs
make logs-api    # Affiche les logs de l'API
make logs-db     # Affiche les logs de la base de données
make build       # Build la solution .NET et les images Docker
make rebuild     # Clean, rebuild la solution et les images sans cache
make clean       # Nettoie tout (solution, conteneurs, volumes)
make dev         # Démarre en mode développement
make fresh       # Clean, rebuild et redémarre tout
``` 