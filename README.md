# :books: :books: :books: API d�di�e � rechercher des livres depuis plusieurs sources :books: :books: :books:

## 1 / Sources choisies 

**Ce projet exploite actuellement deux sources** :
- l'API de la biblioth�que nationale de France (BnF) : http://catalogue.bnf.fr/api
- une base MySQL personnelle

Il a �t� construit de sorte � pouvoir ajouter d'autres sources de donn�es, par example une autre API ou une autre base de donn�es (SQL Server par exemple).

Le choix a �t� fait de ne renvoyer que les entr�es rattach�es � un num�ro ISBN.

**A noter que l'API de la BnF n'est actuellement exploit�e que pour la recherche de type "simple"** (cf section 2.3).

## 2 / Aspects fonctionnels du projet

### 2.1 / D�finitions

Pr�cisons quels sont les termes employ�s pour d�crire les entit�s dans le cadre de ce projet�:

>  **� Edition �** : entit� bibliographique d�sign�e par un ISBN unique. Une �dition est caract�ris�e par une date de publication, un �diteur et s�inscrit dans une s�rie. Il s�agit d�une � manifestation � au sens donn� par l�IFLA dans son formalisme de description des entit�s li�es � la bibliographie. 

> **� Editeur �** : personne morale qui fait publier une �dition. D�sign�e uniquement par son nom dans le cadre du projet.

> **� Livre �** : la d�finition choisie dans le cadre du projet se rapproche de la notion � d�expression � d�crite par l�IFLA. Ici, le livre sera d�crit par son titre, son auteur et la s�rie dans laquelle il s�ins�re. 

> **� Auteur �** : personne physique qui a imagin� � l�oeuvre � (au sens de l�IFLA) et l�a r�alis�e sous forme � d�expression �. Est d�sign�e par son nom. 

> **� S�rie �** : ensemble de plusieurs � �ditions � d�un m�me � �diteur � reconstituant une � expression �, un � livre �.

### 2.2 / Donn�es r�cup�r�es pour chaque livre

Plus concr�tement, l�API renvoie pour chaque ��livre�� un objet contenant�:
- l�**ID du livre**
- un **objet ��Book��** contenant le titre et le nom de l�auteur du livre
- un **tableau ��Editions��** contenant toutes les �ditions de ce livre. Les �ditions sont regroup�es selon le nom de leur s�rie. Si aucun nom de s�rie n�est trouv�, les �ditions sont regroup�es dans un tableau nomm� ��0��. Les �ditions apportent diverses informations, dont l�ISBN, l�ark ID et le nom de l��diteur.

### 2.3 / Deux types de recherche

- La recherche **"simple"** : signifie que l�utilisateur n�a qu�un seul crit�re � saisir lors de sa recherche. Ce crit�re est alors recherch� au sein de plusieurs caract�ristiques des livres�: **le titre, le nom de l�auteur, l�ISBN et le nom de la s�rie**. Un seul crit�re de type `string` est demand�.

- La recherche **"avanc�e"** : l'internaute a la possibilit� de s�lectionner plusieurs crit�res de recherche, qui sont dans ce projet : 

```
- le titre
- l'ISBN
- l'auteur
- le nom de la s�rie
- le genre
- l'�diteur
- le format
- la date de publication (�gale, inf�rieure ou sup�rieure � une date donn�e)
```

### 2.4 / Donn�es de la BnF

La **Biblioth�que Nationale de France** (nomm�e dans la suite du document ��BnF��) met � disposition des internautes des notices bibliographiques et d�autorit�s d�crivant tous les ouvrages ayant �t� d�pos�s dans ses locaux dans le cadre du d�p�t l�gal. Ces notices concernent les livres mais aussi d�autres types d�oeuvres telles que les musicales, les cin�matographiques, les iconographiques, etc.

Les donn�es pr�sentes dans ces notices sont exploitables via une API librement utilisable, sans besoin de s�authentifier.

## 3 / Aspects techniques du projet

### 3.1 / Param�tres n�cessaires � toute recherche

Format des donn�es attendues et renvoy�es�:
`"Content-type": "application/json"`

URL de base�: `back_end_domain/api`

La recherche via la BnF exige de fournir un nombre de "notices" renvoy�es parmi les suivants : 20, 100, 200, 500, 1000. Il peut �tre laiss� � 0 si une autre source est utilis�e.

```ts
{
  "apiBnf": true,
  "apiBnfNoticesQty": 0,
  "criterion": string ( ou "criteria": object )
}
```

### 3.2 / Deux types de recherche

- La recherche **"simple"** : Un seul crit�re de type `string` est demand�.

- La recherche **"avanc�e"** : l'internaute a la possibilit� de s�lectionner plusieurs crit�res de recherche, qui sont dans ce projet : 

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

### 3.3 / Requ�tes API disponibles :

- **GET** `/api/load` : chargement des donn�es de formulaire (genres, �diteurs, formats)
- **POST** `/api/search/simple` : recherche "simple" des livres
- **POST** `/api/search/advanced` : recherche "avanc�e" des livres
- **GET** `api/entity_name` : lister toutes les entit�s de la base de donn�es MySQL d'un type donn� (par exemple `/api/genres`), cr��es � des fins de test