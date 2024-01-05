<!-- README.et.md -->
See on README fail Eesti keeles.

[English](README.md)

# Hotellide registreerimise ja tühistamise süsteem.
## Üldine.

Süsteem koosneb kahest osast, mis töötavad ühise andmebaasiga.

## Rakendus administreerimiseks(HotelAdminApplication).

Juurdepääsu taseme "**Manager**" kasutajad saavad:

-   Lisada ruum
    
-   Muuta ruumide andmeid
    
-   Eemalda ruum
    
-   Vaadata broneeringuid
    
-   Vaadata klientide andmeid

Süsteem võimaldab kasutajatel, kellel on juurdepääsutase "**Administraator**", teha kõiki "**Manager**" taseme toiminguid ning lisada **uusi süsteemi kasutajaid** mistahes juurdepääsu tasemetele.

## Rakendus klientidele(HotelApplication).

Süsteemi kasutamiseks peab klient registreeruma ja süsteemi sisse logima.

Süsteem võimaldab kasutajatel otsida vabu ruume, filtreerides ajavahemikku, magamiskohtade arvu ja hinda.

Valides ruumi, saab kasutaja seda üksikasjalikumalt vaadata ja broneerida vajalikuks ajavahemikuks.

Samuti saab igaüks **oma broneeringuid** kontrollida.
 **Broneeringut ei saa tühistada hiljem kui 3 päeva enne majutuse algust**.

## Paigaldus.

Fail: appsettings.json

-   "**DBConnection**" - Andmebaasiühenduse rida
    
-   "**ApiConnection**" - Ühendamine API-ga

Normaalse töö jaoks on vaja iga paari jaoks säilitada "ApiKey".

-   AdminSide\HotelAdminApi
    
-   AdminSide\HotelAdminApplication  
ja
-   CustomerSide\HotelApplication
    
-   CustomerSide\HotelCustomerApi

### Näiteks:
    cd AdminSide\HotelAdminApi
    dotnet user-secrets init
    dotnet user-secrets set "ApiKey" "6CBxzdYcEgNDrRhMbDpkBF7e4d4Kib46dwL9ZE5egiL0iL5Y3dzREUBSUYVUwUkN"
    
    cd AdminSide\HotelAdminApi
    dotnet user-secrets init
    dotnet user-secrets set "ApiKey" "6CBxzdYcEgNDrRhMbDpkBF7e4d4Kib46dwL9ZE5egiL0iL5Y3dzREUBSUYVUwUkN"
    
    cd CustomerSide\HotelApplication
    dotnet user-secrets init
    dotnet user-secrets set "ApiKey" "YourSuperPassword"
      
    cd CustomerSide\HotelCustomerApi
    dotnet user-secrets init
    dotnet user-secrets set "ApiKey" "YourSuperPassword"
Süsteem esimesel käivitamisel loob kaks süsteemi kasutajat **ühise parooliga**.
admin@hotels.com,
manager@hotels.com
Nende jaoks on vaja säilitada ka **salasõna**.
*Salasõna peab olema vähemalt 6 tähemärki pikk.
Salasõna peab olema vähemalt üks mittetähtnumbriline märk.
Salasõna peab olema vähemalt üks väiketäht ('a'-'z').
Salasõna peab olema vähemalt üks suurtäht (A-Z).*
### Näiteks:

    cd AdminSide\HotelAdminApplication
    dotnet user-secrets set SeedUserPW "#6CBxzdYcEgNDr"

  Kõik on valmis, käivitage administraatori osa(**HotelAdminApplication** ja **HotelAdminApi**).

( admin@hotels.com - "#6CBxzdYcEgNDr")
Minge portaali ja looge mõned "Room".

Nüüd "**HotelApplication**" portaali sisenedes saate need toad valida ja broneeringuid teha.
