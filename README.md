# Shooting Ranking Administration

## Description

Cette application aura pour but de gérer les résultats obtenus par des **tireurs.euses** *(via armes à feu, pratiquants le tir sportif)* lors de diverses **compétitions** de tir. Ces résultats seront enregistrés sous forme de 6 **séries** de points, dépendants des impacts sur la cible. Un.e tireur.euse ne pourra tirer qu'une fois par compétition.

## Classes de données de base

Classe **Shooter**: instancie un.e tireur.euse
```cs
public class Shooter : INotFixedInTime, IXMLSavable
{

    private string _id = "";
    private string _firstname = "";
    private string _lastname = "";
    private DateTime _birthday;
    private Country _nationality;
    private DateTime _createdAt;
    private DateTime _updatedAt;

}
```

Classe **Serie**: instancie une série
```cs
public class Serie : INotFixedInTime, IXMLSavable
{

    private int _id;
    private float _points;
    private Shooter _shootedBy;
    private Competition _shootedAt;
    private DateTime _createdAt;
    private DateTime _updatedAt;

}
```

Classe **Competition**: instancie une compétition
```cs
public class Competition : INotFixedInTime, IXMLSavable
{

    private int _id;
    private string _name;
    private DateTime _startDate;
    private DateTime _endDate;
    private DateTime _createdAt;
    private DateTime _updatedAt;

}
```


## Classes de données connexes

Classe **User**: instancie un utilisateur (pour se connecter à l'administration)
```cs
public class User : INotFixedInTime, IXMLSavable
{

    private int _id = -1;
    private string _firstname = "";
    private string _lastname = "";
    private string _email = "";
    private string _password = "";
    private DateTime _createdAt;
    private DateTime _updatedAt;

}
```

Classe **Country**: instancie un pays
```cs
public class Country : IXMLSavable // Codification officielle des nations unies
{

    private int _id;
    private string _name;
    private string _iso2;
    private string _iso3;

}
```