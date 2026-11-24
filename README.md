# Alexandria könyvtár

A programot készítette *Nagy Izabella* és *Tóth Álmos*

Ez egy könyvtáros program ahol lehet rögzíteni kölcsönzést, számontartani a teljes könyvtár tartalmát, könyvek hollétét és a tagságokat kezelni.

## Bejelentkezés

A program indítása után be kell jelentkezni. Különféle fiók van a könyvtárosoknak és a tagoknak. A könyvtáros fiókjának `admin` jogosultsága van, az olvasóknak `user` jogosultsága van. A bejelentkezéshez a jelszavakat hash-eljük.

## Admin főmenü
Az `admin`, azaz könyvtáros, minden funkcióját használhatja a programnak. 

### 1. Tag
Itt lehet kezelni az olvasó tagokat és adatait.

1) **Tag felvétele**
    - Új tagok felvétele a nyilvántartásba
    - Új tag bekért adatai (kötelező kitölteni): azonosító(jelszó), név, kor(egész szám)
2) **Tag törlése**
    - Egy tag tagságának végleges megszüntetése
3) **Tagok megtekintése**
    - Az összes tag és adataik kiíratása
4) **Tagok adatmódosítás**
    - Egy tag adatainak módosítása

### 2. Könyv
Itt lehet kezelni a könyvtár birtokában álló könyveket és adataikat.

1) **Könyv felvétele**
    - Új könyv felvétele a nyilvántartásba
    - új könyv bekért adatai (kötelező kitölteni): azonosító, cím, szerző, műfaj, korhatár(igen/nem)
2) **Könyv törlése**
    - Könyvek végleges törlése az könyvtárból
3) **Könyvek megtekintése**
    - Az összes könyv és adataik kiíratása
4) **Könyvek adatmódosítás**
    - Egy könyv adatainak módosítása

### 3. Kölcsönzés
Itt lehet kezelni a kikölcsönzött könyveket és adataikat.

1) **Kölcsönzés hozzáadása**
    - új könyv felvétele a könyvtárba
2) **Kölcsönzés törlése**
    - könyvek végleges törlése az könyvtárból
3) **Kölcsönzés hosszabítása**
    - meg tud hosszabbítani egy könyv kölcsönzésének idejét

### 4. Keresés

1) **Tagok keresése**
    - az olvasótagokból lehet keresni, megmutatja a keresett tag összes adatait
2) **Könyvek keresése**
    - a könyvtárban lávő összes könyvől lehet keresni, megmutatja a keresett könyv(ek) összes adatait
3) **Kikölcsönzött könyvek listája**
    - megmutatja a összes kikölcsönzött könyvet és adataikat

### 0. Kilépés
Ezzel lehet kilépni a programból, vagy kidob a főmenübe, ha már elkezdtél valamit csinálni. Ez a menüpont minden menübe meg fog jelenni 0. elemként.

## User főmenü
A `user`, azaz olvasó tag, kevesebb jogosultsága van mint a könyvtárosnak. 

- 1) **Keresés**
    - új tagok felvétele
- 2) **Profil megtekintése**
    - megtekintheti a saját fiókját és az összes adatait a user.
- 3) **Tagság törlése**
    - törölheti a tagságát
- 4) **Kivett könyvek listája**
    - megtekintheti az összes kivett könyveit

## Szerkezet

### Adatok felírása
Az  adatokat 4 külön fájlba menti a program.

| Név              | Tárolt adat           | 
| :--------------  | :-------------------- | 
| `konyvek.csv`    | Teljes könyvtár       |
| `tagok.csv`      | Tagok adatai          |
| `kolcsonzes.csv` | Kikölcsönzött könyvek |
| `login.csv`      | Felhasználók          |

A fájlok helye:
*\konyvkolcsonzes\ConsoleApp1\bin\Debug\net10.0*