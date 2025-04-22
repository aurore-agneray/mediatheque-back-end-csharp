# :books: :books: :books: API dédiée à rechercher des livres depuis plusieurs sources :books: :books: :books:

## 1 / Sources choisies 

**Ce projet exploite actuellement deux sources** :
- l'API de la bibliothèque nationale de France (BnF) : http://catalogue.bnf.fr/api
- une base MySQL personnelle

Il a été construit de sorte à pouvoir ajouter d'autres sources de données, par example une autre API ou une autre base de données (SQL Server par exemple).

Le choix a été fait de ne renvoyer que les entrées rattachées à un numéro ISBN.

**A noter que l'API de la BnF n'est actuellement exploitée que pour la recherche de type "simple"** (cf section 2.3).

## 2 / Aspects fonctionnels du projet

### 2.1 / Définitions

Précisons quels sont les termes employés pour décrire les entités dans le cadre de ce projet :

>  **« Edition »** : entité bibliographique désignée par un ISBN unique. Une édition est caractérisée par une date de publication, un éditeur et s’inscrit dans une série. Il s’agit d’une « manifestation » au sens donné par l’IFLA dans son formalisme de description des entités liées à la bibliographie. 

> **« Editeur »** : personne morale qui fait publier une édition. Désignée uniquement par son nom dans le cadre du projet.

> **« Livre »** : la définition choisie dans le cadre du projet se rapproche de la notion « d’expression » décrite par l’IFLA. Ici, le livre sera décrit par son titre, son auteur et la série dans laquelle il s’insère. 

> **« Auteur »** : personne physique qui a imaginé « l’oeuvre » (au sens de l’IFLA) et l’a réalisée sous forme « d’expression ». Est désignée par son nom. 

> **« Série »** : ensemble de plusieurs « éditions » d’un même « éditeur » reconstituant une « expression », un « livre ».

### 2.2 / Données récupérées pour chaque livre

Plus concrètement, l’API renvoie pour chaque « livre » un objet contenant :
- l’**ID du livre**
- un **objet « Book »** contenant le titre et le nom de l’auteur du livre
- un **tableau « Editions »** contenant toutes les éditions de ce livre. Les éditions sont regroupées selon le nom de leur série. Si aucun nom de série n’est trouvé, les éditions sont regroupées dans un tableau nommé « 0 ». Les éditions apportent diverses informations, dont l’ISBN, l’ark ID et le nom de l’éditeur.

### 2.3 / Deux types de recherche

- La recherche **"simple"** : signifie que l’utilisateur n’a qu’un seul critère à saisir lors de sa recherche. Ce critère est alors recherché au sein de plusieurs caractéristiques des livres : **le titre, le nom de l’auteur, l’ISBN et le nom de la série**. Un seul critère de type `string` est demandé.

- La recherche **"avancée"** : l'internaute a la possibilité de sélectionner plusieurs critères de recherche, qui sont dans ce projet : 

```
- le titre
- l'ISBN
- l'auteur
- le nom de la série
- le genre
- l'éditeur
- le format
- la date de publication (égale, inférieure ou supérieure à une date donnée)
```

### 2.4 / Données de la BnF

La **Bibliothèque Nationale de France** (nommée dans la suite du document « BnF ») met à disposition des internautes des notices bibliographiques et d’autorités décrivant tous les ouvrages ayant été déposés dans ses locaux dans le cadre du dépôt légal. Ces notices concernent les livres mais aussi d’autres types d’oeuvres telles que les musicales, les cinématographiques, les iconographiques, etc.

Les données présentes dans ces notices sont exploitables via une API librement utilisable, sans besoin de s’authentifier.

## 3 / Aspects techniques du projet

### 3.1 / Paramètres nécessaires à toute recherche

Format des données attendues et renvoyées :
`"Content-type": "application/json"`

URL de base : `back_end_domain/api`

La recherche via la BnF exige de fournir un nombre de "notices" renvoyées parmi les suivants : 20, 100, 200, 500, 1000. Il peut être laissé à 0 si une autre source est utilisée.

```ts
{
  "apiBnf": true,
  "apiBnfNoticesQty": 0,
  "criterion": string ( ou "criteria": object )
}
```

### 3.2 / Deux types de recherche

- La recherche **"simple"** : Un seul critère de type `string` est demandé.

- La recherche **"avancée"** : l'internaute a la possibilité de sélectionner plusieurs critères de recherche, qui sont dans ce projet : 

```ts
"criteria": {
    "title": string,
    "isbn": string,
    "author": string,
    "series": string,
    "genre": {
        "id": number,
        "name": string
    },
    "publisher": {
        "id": number,
        "name": string
    },
    "format": {
        "id": number,
        "name": string
    },
    "pubDate": {
        "operator": string,
        "criterion": string
    }
}
```

### 3.3 / Requêtes API disponibles :

- **GET** `/api/load` : chargement des données de formulaire (genres, éditeurs, formats)
- **POST** `/api/search/simple` : recherche "simple" des livres
- **POST** `/api/search/advanced` : recherche "avancée" des livres
- **GET** `api/entity_name` : lister toutes les entités de la base de données MySQL d'un type donné (par exemple `/api/genres`), créées à des fins de test

### 3.4 / Contenu attendu du fichier appsettings.json (branche master) :

```
{
  "FrontEndDomains": "DOMAINS_SEPARATED_BY_SEMICOLONS",
  "MySQLConnectionString": "Server=HOST_NAME; User ID=USER_ID; Password=PASSWORD; Database=DATABASE_NAME"
}
```
