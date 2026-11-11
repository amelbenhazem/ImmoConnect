# ImmoConnect

Plateforme web de mise en relation immobilière : publication d’annonces, catégories, comptes (client / admin) et demandes de visite.

Application **ASP.NET Core 8 MVC** (Razor + Bootstrap) avec **MySQL**.


## Configuration

La chaîne de connexion se trouve dans `ImmoConnect_BE/appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=gestion_immo;user=root;password=;"
}
```

Adaptez `user` et `password` à votre installation MySQL. La base `gestion_immo` est créée automatiquement au premier lancement (migrations / seed).

## Lancer le projet

```bash
cd ImmoConnect_BE
dotnet restore
dotnet run
```

Les migrations et les données de démo sont appliquées au démarrage.

Ou ouvrir `ImmoConnect_BE.sln` dans Visual Studio et lancer le profil **https**.

Au premier démarrage, des données de démo sont insérées.

## Comptes de démonstration

| Rôle   | Email                     | Mot de passe |
|--------|---------------------------|--------------|
| Admin  | `admin@immoconnect.com`   | `Admin123!`  |
| Client | `agent@immoconnect.com`   | `Agent123!`  |

## Fonctionnalités

- Inscription / connexion (cookies, rôles Admin et Client)
- Catalogue de biens avec recherche et filtres (catégorie, prix, statut)
- CRUD des annonces pour le propriétaire
- Demandes de visite
- Tableau de bord administrateur (utilisateurs, catégories, biens, visites)

## Structure

```
ImmoConnect_BE/
├── Controllers/    MVC + API REST
├── Models/         Entités EF Core
├── Views/          Pages Razor
├── Data/           DbContext et seed
├── Services/       Authentification et comptes
└── Repositories/   Accès utilisateurs
```
