# Alexandria könyvtár

A programot készítette *Nagy Izabella* és *Tóth Álmos*

Ez egy könyvtáros program ahol lehet rögzíteni könyvkölcsönzést, számontartani egy teljes könyvtár tartalmát, könyvek hollétét és a tagságokat kezelni.

## Bejelentkezés

A program indítása után be kell jelentkezni. Különféle fiók van a könyvtárosoknak és a tagoknak. A könyvtáros fiókjának `admin` jogosultsága van, az olvasóknak `user` jogosultsága van. 

## Admin főmenü

Az `admin`, azaz könyvtáros, minden funkcióját használhatja a programnak. Ő tudja a tagokat, a könyveket kezelni és kölcsönzéseket intézni. Valamint az összes mentett adathoz van keresési jogosultsága.

### 1. Tag

Itt lehet kezelni az olvasó tagokat és adatait.

1) **Tag felvétele**
   - Új tagok felvétele a nyilvántartásba
   - Új tag bekért adatai (kötelező kitölteni):
     - név
     - jelszó
     - kor(egész szám)
2) **Tag törlése**
   - Egy tag tagságának végleges megszüntetése
3) **Tagok megtekintése**
   - Az összes tag és adataik kiíratása
4) **Tagok adatmódosítás**
   - Egy tag adatainak módosítása

### 2. Könyv

Itt lehet kezelni a könyvtár tulajdonában lévő könyveket és adataikat.

1) **Könyv felvétele**
   - Új könyv felvétele a nyilvántartásba
   - Új könyv bekért adatai (kötelező kitölteni):
     - azonosító
     - cím
     - szerző
     - műfaj
     - korhatárosigen/nem)
2) **Könyv törlése**
   - Könyvek végleges törlése az könyvtárból
3) **Könyvek megtekintése**
   - Az összes könyv és adataik kiíratása
4) **Könyvek adatmódosítás**
   - Egy könyv adatainak módosítása

### 3. Kölcsönzés

Itt lehet kezelni a kikölcsönzött könyveket és adataikat.

1) **Kölcsönzés hozzáadása**
   - Új kölcsönzés rögzítése
2) **Kölcsönzés törlése**
   - Mekölcsönzés törlése
3) **Kölcsönzés hosszabítása**
   - Meg tud hosszabbítani egy könyv kölcsönzésének idejét

### 4. Keresés

1) **Tagok keresése**
   - Az olvasótagokból lehet keresni, megmutatja a keresett tag összes adatait
2) **Könyvek keresése**
   - A könyvtárban lávő összes könyvől lehet keresni, megmutatja a keresett könyv(ek) összes adatait
3) **Kikölcsönzött könyvek listája**
   - Megmutatja a összes kikölcsönzött könyvet és adataikat

### 0. Kilépés

Ezzel lehet kilépni a programból, vagy kidob a főmenübe, ha már elkezdtél valamit csinálni. Ez a menüpont minden menübe meg fog jelenni 0. elemként.

## User főmenü

A `user`, azaz olvasó tag, kevesebb opciója és jogosultsága van mint a könyvtárosnak. A user saját fiókja törlésén kívül semmilyen adatot nem tudm módosítani.

- 1) **Keresés**

  - A könyvtár összes könyvéből tud keresni
- 2) **Profil megtekintése**

  - Megtekintheti a saját fiókját és az összes adatait a user.
- 3) **Tagság törlése**

  - Törölheti a tagságát
- 4) **Kivett könyvek listája**

  - Megtekintheti az összes kivett könyveit

## Szerkezet

### Adatok felírása

Az  adatokat 4 külön fájlba menti a program.

| Név               | Tárolt adat                         |
| :----------------- | :----------------------------------- |
| `konyvek.csv`    | Teljes könyvtár, az összes könyv |
| `tagok.csv`      | Tagok adatai                         |
| `kolcsonzes.csv` | Kikölcsönzött könyvek            |
| `login.csv`      | Tagok/admin bejelentkezési adatai   |

A fájlok helye:
*\konyvkolcsonzes\ConsoleApp1\bin\Debug\net10.0*

## Használati példa

#### Új felhasználó felvétele
