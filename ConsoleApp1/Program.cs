using System.IO;
using System.Linq;
using System.Collections.Generic;

// konyvek.csv          id(string);cim(string);szerzo(string);mufaj(string);korhatar(bool);statusz(bool)\n
// tagok.csv            id(string);nev(string);kor(int);tagsag_kezdete(string);tagsag_lejarata(string);kikolcsonzott_konyvek(list)
// kolcsonzes.csv       konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok

if (!File.Exists("konyvek.csv")) File.WriteAllText("konyvek.csv", "id;cim;szerzo;mufaj;korhatar;statusz\n");
if (!File.Exists("tagok.csv")) File.WriteAllText("tagok.csv", "id;nev;kor;tagsag_kezdete;tagsag_lejarata;kikolcsonzott_konyvek\n");
if (!File.Exists("kolcsonzes.csv")) File.WriteAllText("kolcsonzes.csv", "konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok\n");
if (!File.Exists("login.csv")) File.WriteAllText("login.csv","felhasznalonev;jelszo\n");

//--------------------------------------------------
// Belépés

string belepes()
{
    bool jo = false;

    Console.Write("Bejelentkezés\n\nFelhasználónév: ");
    string username = Console.ReadLine();

    Console.Write("Jelszó: ");
    string password = Console.ReadLine();

    foreach (string sor in File.ReadLines("login.csv").Skip(1))
    {
        string[] oszlopok = sor.Split(';');
        //if (oszlopok.Length != 3 || !bool.TryParse(oszlopok[2], out bool igaz))
        //{
        //    Console.WriteLine($"Hibás sor: {sor}");
        //    continue;
        //}
        if (oszlopok[1]==username && oszlopok[2]==password)
        {
            jo = true;
            //menu inditása
            if (bool.Parse(oszlopok[3]))
            {
                return "admin";
            }
            else if (!bool.Parse(oszlopok[3]))
            {
                return "user";
            }
        }

    }

    if (!jo)
    {
        Console.WriteLine("HIBÁS FELHASZNÁLÓNÉV VAGY JELSZÓ!");
    }

    return "sikertelen";
}

//--------------------------------------------------
// Helperek

string normalizal(string bemenet)
{
    return bemenet.ToUpper().Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Replace("?", "").Replace("!", "");
}

bool szamE(string bemenet)
{
    return int.TryParse(bemenet, out int szam) && szam > 0;
}

bool besorolasCheck(string bemenet)
{
    return normalizal(bemenet) == "0" || normalizal(bemenet) == "1" || normalizal(bemenet) == "F" || normalizal(bemenet) == "T" || normalizal(bemenet) == "FALSE" || normalizal(bemenet) == "TRUE";
}

string adatCheck(string eredeti, string regi, string uj)
{
    return normalizal(eredeti) == normalizal(regi) ? uj : eredeti;
}

//--------------------------------------------------
// Admin

//-------------------------
// Menü

bool inditasAdmin()
{
    while (true)
    {
        Console.Write("\n1) Tag\n2) Könyv\n3) Kölcsönzés\n4) Keresés\n0) Kilépés\n\nA választott funkció betűjele: ");
        string valasztas = Console.ReadLine();

        switch (valasztas)
        {
            case "1": tag(); break;
            case "2": konyv(); break;
            case "3": kolcsonzes(); break;
            case "4": kereses(); break;
            case "0": Console.WriteLine("Kilépés..."); return true;

            default: Console.WriteLine("Sikertelen indítás! Adjon meg egy létező opciót!"); break;
        }
    }
}

bool tag()
{
    while (true)
    {
        Console.Write("\n--------------------\nTag\n\n1) Tag felvétele\n2) Tag törlése\n3) Tagok megtekintése\n4) Tag adatmódosítása\n0) Vissza\n\nVálasztás: ");
        string valasztas = Console.ReadLine();

        switch (valasztas)
        {
            case "1": if (felvetelTag()) return true; break;
            case "2": if (torlesTag()) return true; break;
            case "3": listazasTag(); break;
            case "4": if (modositasTag()) return true; break;
            case "0": return false;
            default: Console.WriteLine("Sikertelen végrehajtás: Adjon meg egy létező opciót!"); break;
        }
    }
}

bool konyv()
{
    while (true)
    {
        Console.Write("\n--------------------\nKönyv\n\n1) Könyv felvétele\n2) Könyv törlése\n3) Könyvek megtekintése\n4) Könyv adatmódosítása\n0) Vissza\n\nVálasztás: ");
        string valasztas = Console.ReadLine();

        switch (valasztas)
        {
            case "1": if (felvetelKonyv()) return true; break;
            case "2": if (torlesKonyv()) return true; break;
            case "3": listazasKonyv(); break;
            case "4": if (modositasKonyv()) return true; break;
            case "0": return false;
            default: Console.WriteLine("Sikertelen végrehajtás: Adjon meg egy létező opciót!"); break;
        }
    }
}

bool kolcsonzes()
{
    while (true)
    {
        Console.Write("\n--------------------\nKölcsönzés\n\n1) Kölcsönzés rögzítése\n2) Kölcsönzés törlése\n3) Kölcsönzés hosszabbítása\n0) Vissza\n\nVálasztás: ");
        string valasztas = Console.ReadLine();

        switch (valasztas)
        {
            case "1": if (felvetelKolcsonzes()) return true; break;
            case "2": if (torlesKolcsonzes()) return true; break;
            case "3": if (hosszabitasKolcsonzes()) return true; break;
            case "0": return false;
            default: Console.WriteLine("Sikertelen végrehajtás: Adjon meg egy létező opciót!"); break;
        }
    }
}

bool kereses()
{
    while (true)
    {
        Console.Write("\n--------------------\nKeresés\n\n1) Tagok keresése\n2) Könyvek keresése\n3) Kölcsönzések listája\n0) Vissza\n\nVálasztás: ");
        string valasztas = Console.ReadLine();

        switch (valasztas)
        {
            case "1": if (tagKereses()) return true; break;
            case "2": if (konyvKereses()) return true; break;
            case "3": if (kolcsonzesKereses()) return true; break;
            case "0": return false;
            default: Console.WriteLine("Sikertelen végrehajtás: Adjon meg egy létező opciót!"); break;
        }
    }
}

//-------------------------
// Tag

bool felvetelTag()                                                          // Ide tudod írni a logint is, felh: név; jelsz: id HASHELVE!!!
{
    Console.Write("--------------------\nTag hozzáadása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("\nNév: ");
    string nev = Console.ReadLine();

    Console.Write("\nKor: ");
    string korBemenet = Console.ReadLine();
    if (szamE(korBemenet))
    {
        int kor = int.Parse(korBemenet);
        DateOnly felvetel = DateOnly.FromDateTime(DateTime.Now);
        DateOnly lejarat = felvetel.AddMonths(3);

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nev))
        {
            Console.WriteLine("\nSikertelen hozzáadás: Adjon meg minden adatot!");
            return false;
        }

        string[] tagok = File.ReadAllLines("tagok.csv");

        foreach (string tag in tagok.Skip(1))
        {
            string[] adatok = tag.Split(";");
            if (normalizal(adatok[0]) == normalizal(id))
            {
                Console.WriteLine("\nSikertelen hozzáadás: Ilyen azonosítójú tag már létezik!");
                return false;
            }
        }

        using StreamWriter sw = File.AppendText("tagok.csv");
        sw.WriteLine($"{id};{nev};{kor};{felvetel};{lejarat};nincs");

        return true;
    }
    else
    {
        Console.WriteLine("\nSikertelen hozzáadás: Érvénytelen életkor!");
        return false;
    }
}

bool torlesTag()                                                            // Töröld ki a usert a login.csv-ből is!!!
{
    bool benne = false;
    Console.Write("--------------------\nTag törlése\n\nAzonosító: ");
    string id = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("\nSikertelen törlés: Adjon meg egy azonosítót!");
        return false;
    }

    string[] tagok = File.ReadAllLines("tagok.csv");
    List<string> ujTagok = [];

    foreach (string tag in tagok.Skip(1))
    {
        string[] adatok = tag.Split(";");
        if (normalizal(adatok[0]) == normalizal(id))
        {
            benne = true;

            if (adatok[0] != "nincs")
            {
                Console.WriteLine("\nSikertelen törlés: A tagnak vannak kikölcsönzött könyvei!");
                return false;
            }
        }
        else
        {
            ujTagok.Add(tag);
        }
    }
    if (!benne)
    {
        Console.WriteLine("\nSikertelen törlés: A tag nem szerepel a nyilvántartásban!");
        return false;
    }
    else
    {
        File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));
    }

    return true;
}

void listazasTag()
{
    string[] sorok = File.ReadAllLines("tagok.csv");

    if (sorok.Length == 1)
    {
        Console.WriteLine("\nJelenleg nincsenek regisztrált tagok.");
    }
    else
    {
        Console.WriteLine($"\nJelenlegi tagok száma: {sorok.Length - 1}");
        foreach (string sor in sorok.Skip(1))
        {
            string[] adatok = sor.Split(";");
            Console.WriteLine($"--------------------\nAzonosító: {adatok[0]}\nNév: {adatok[1]}\nKor: {adatok[2]}\nTagság kezdete: {adatok[3]}\nTagság lejárata: {adatok[4]}\nJelenleg kikölcsönzött könyvek: {(adatok[5] == "nincs" ? "Jelenleg nincs kikölcsönzött könyve." : adatok[5])}\n");
        }
    }
}

bool modositasTag()                                                         // Ha nevet módosít akkor a login.csv-ben is updatelni kell!!!
{
    bool benne = false;
    Console.Write("--------------------\nTag adatmódosítása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("\nRégi adat (tagság HOSSZABBÍTÁSA esetén írja be az 'F' betűt!): ");
    string regiAdat = Console.ReadLine();

    Console.Write("\nÚj adat (tagság HOSSZABBÍTÁSA esetén írja be újra az 'F' betűt!): ");
    string ujAdat = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(regiAdat) || string.IsNullOrWhiteSpace(ujAdat))
    {
        Console.WriteLine("\nSikertelen módosítás: Adjon meg minden adatot!");
        return false;
    }

    bool hosszabbitas = normalizal(regiAdat) == "F" && normalizal(ujAdat) == "F";

    string[] tagok = File.ReadAllLines("tagok.csv");
    List<string> ujTagok = [];

    foreach (string tag in tagok.Skip(1))
    {
        string[] adatok = tag.Split(";");

        if (normalizal(adatok[0]) == normalizal(id))
        {
            bool nevModositas = normalizal(adatok[1]) == normalizal(regiAdat);
            bool korModositas = normalizal(adatok[2]) == normalizal(regiAdat);

            if (!hosszabbitas && !nevModositas && !korModositas)
            {
                ujTagok.Add(tag);
                continue;
            }

            if (korModositas && !szamE(ujAdat))
            {
                Console.WriteLine("\nSikertelen módosítás: Érvénytelen életkor!");
                return false;
            }

            if (nevModositas && string.IsNullOrWhiteSpace(ujAdat))
            {
                Console.WriteLine("\nSikertelen módosítás: A név nem lehet üres!");
                return false;
            }

            benne = true;

            string ujNev = nevModositas ? ujAdat : adatok[1];
            string ujKor = korModositas ? ujAdat : adatok[2];
            string ujLejarat = adatok[4];

            if (hosszabbitas)
            {
                if (!DateOnly.TryParse(adatok[4], out DateOnly regiLejarat))
                {
                    Console.WriteLine("\nSikertelen módosítás: Hibás tagsági lejárati dátum!");
                    return false;
                }

                DateOnly maiNap = DateOnly.FromDateTime(DateTime.Now);

                ujLejarat = (regiLejarat > maiNap ? regiLejarat : maiNap).AddMonths(3).ToString("yyyy-MM-dd");
            }

            ujTagok.Add($"{adatok[0]};{ujNev};{ujKor};{adatok[3]};{ujLejarat};{adatok[5]}");
        }
        else
        {
            ujTagok.Add(tag);
        }
    }

    if (!benne)
    {
        Console.WriteLine("\nSikertelen módosítás: Ilyen azonosítóval/adattal rendelkező tag nem szerepel a nyilvántartásban!");
        return false;
    }
    else
    {
        File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));
    }

    return true;
}

//-------------------------
// Könyv

bool felvetelKonyv()
{
    Console.Write("--------------------\nKönyv hozzáadása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("\nCím: ");
    string cim = Console.ReadLine();

    Console.Write("\nSzerző: ");
    string szerzo = Console.ReadLine();

    Console.Write("\nMűfaj: ");
    string mufaj = Console.ReadLine();

    Console.Write("\nBesorolás (0, 'F' vagy 'FALSE' -- nem korhatáros; 1, 'T' vagy 'TRUE' -- korhatáros): ");
    string besorolas = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(cim) || string.IsNullOrWhiteSpace(szerzo) || string.IsNullOrWhiteSpace(mufaj) || !besorolasCheck(normalizal(besorolas)))
    {
        Console.WriteLine("\nSikertelen hozzáadás: Kérem, adjon meg minden adatot!");
        return false;
    }

    string[] konyvek = File.ReadAllLines("konyvek.csv");

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == normalizal(id))
        {
            Console.WriteLine("\nSikertelen hozzáadás: Ilyen azonosítójú könyv már létezik!");
            return false;
        }
    }

    using StreamWriter sw = File.AppendText("konyvek.csv");
    sw.WriteLine($"{id};{cim};{szerzo};{mufaj};{normalizal(besorolas) is "1" or "TRUE" or "T"};true");

    return true;
}

bool torlesKonyv()
{
    bool benne = false;
    Console.Write("--------------------\nKönyv törlése\n\nAzonosító: ");
    string id = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("\nSikertelen törlés: Adjon meg egy azonosítót!");
        return false;
    }

    string[] konyvek = File.ReadAllLines("konyvek.csv");
    List<string> ujKonyvek = [];

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == normalizal(id))
        {
            benne = true;
            if (!bool.Parse(adatok[5]))
            {
                Console.WriteLine("Sikertelen törlés: A könyv jelenleg kölcsönzés alatt áll!");
                ujKonyvek.Add(konyv);
                return false;
            }
        }
        else
        {
            ujKonyvek.Add(konyv);
        }
    }
    if (!benne)
    {
        Console.WriteLine("\nSikertelen törlés: A könyv nem szerepel a nyilvántartásban!");
        return false;
    }
    else
    {
        File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
    }

    return true;
}

void listazasKonyv()
{
    string[] sorok = File.ReadAllLines("konyvek.csv");

    if (sorok.Length == 1)
    {
        Console.WriteLine("\nJelenleg nincsenek regisztrált könyvek.");
    }
    else
    {
        Console.WriteLine($"\nJelenlegi könyvek száma: {sorok.Length - 1}");
        foreach (string sor in sorok.Skip(1))
        {
            string[] adatok = sor.Split(";");
            Console.WriteLine($"--------------------\nAzonosító: {adatok[0]}\nCím: {adatok[1]}\nSzerző: {adatok[2]}\nMűfaj: {adatok[3]}\nBesorolás: {(bool.Parse(adatok[4]) ? "Korhatáros" : "Nem korhatáros")}\nStátusz: {(bool.Parse(adatok[5]) ? "Bent" : "Kint")}\n");
        }
    }
}

bool modositasKonyv()
{
    bool benne = false;
    Console.Write("--------------------\nKönyv adatmódosítása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("\nRégi adat (Korhatár alapján történő besorolás esetén '1' vagy 'TRUE' ha korhatáros, '0' vagy 'FALSE' ha nem): ");
    string regiAdat = Console.ReadLine();

    Console.Write("\nÚj adat (Korhatár alapján történő besorolás esetén '1' vagy 'TRUE' ha korhatáros, '0' vagy 'FALSE' ha nem, az előző ELLENTÉTJE. Ha ez és az előző MEGEGYEZIK, a program akkor is MEGFORDÍTJA a besorolást!):");
    string ujAdat = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id)|| string.IsNullOrWhiteSpace(regiAdat) || string.IsNullOrWhiteSpace(ujAdat))
    {
        Console.WriteLine("\nSikertelen módosítás: Adjon meg minden adatot!");
        return false;
    }

    string[] konyvek = File.ReadAllLines("konyvek.csv");
    List<string> ujKonyvek = [];

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == normalizal(id) && (normalizal(adatok[1]) == normalizal(regiAdat) || normalizal(adatok[2]) == normalizal(regiAdat) || normalizal(adatok[3]) == normalizal(regiAdat) || (besorolasCheck(regiAdat) && besorolasCheck(ujAdat))))
        {
            benne = true;
            ujKonyvek.Add($"{adatok[0]};{adatCheck(adatok[1], regiAdat, ujAdat)};{adatCheck(adatok[2], regiAdat, ujAdat)};{adatCheck(adatok[3], regiAdat, ujAdat)};{(besorolasCheck(regiAdat) && besorolasCheck(ujAdat) ? !bool.Parse(adatok[4]) : adatok[4])};{adatok[5]}");
        }
        else
        {
            ujKonyvek.Add(konyv);
        }
    }

    if (!benne)
    {
        Console.WriteLine("\nSikertelen módosítás: Ilyen azonosítóval/adattal rendelkező könyv nem szerepel a nyilvántartásban!");
        return false;
    }
    else
    {
        File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
    }

    return true;
}

//-------------------------
// Kölcsönzés

bool felvetelKolcsonzes()
{
    bool benneKonyv = false;
    bool benneTag = false;
    bool korhataros = false;
    Console.WriteLine("--------------------\nKölcsönzés rögzítése\n\nKönyv azonosítója: ");
    string konyvId = Console.ReadLine();

    Console.WriteLine("\nTag azonosítója: ");
    string tagId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(konyvId) || string.IsNullOrWhiteSpace(tagId))
    {
        Console.WriteLine("\nSikertelen rögzítés: Adjon meg minden adatot!");
        return false;
    }

    string[] konyvek = File.ReadAllLines("konyvek.csv");
    List<string> ujKonyvek = [];

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == konyvId)
        {
            if (normalizal(adatok[4]) is "1" or "T" or "TRUE")
            {
                korhataros = true;
            }

            benneKonyv = true;
            if (!bool.Parse(adatok[5]))
            {
                Console.WriteLine("Sikertelen rögzítés: A könyv már ki van kölcsönözve!");
                return false;
            }
            else
            {
                ujKonyvek.Add($"{adatok[0]};{adatok[1]};{adatok[2]};{adatok[3]};{adatok[4]};false");
            }
        }
        else
        {
            ujKonyvek.Add(konyv);
        }
    }

    string[] tagok = File.ReadAllLines("tagok.csv");
    List<string> ujTagok = [];

    foreach (string tag in tagok.Skip(1))
    {
        string[] adatokT = tag.Split(";");
        if (normalizal(adatokT[0]) == tagId)
        {
            if (int.Parse(adatokT[2]) < 18 && korhataros)
            {
                Console.WriteLine("Sikertelen rögzítés: A könyv korhatáros, a tag pedig nem felnőtt (18 év vagy az feletti)!");
                return false;
            }

            if (DateOnly.Parse(adatokT[4]) < DateOnly.FromDateTime(DateTime.Now))
            {
               Console.WriteLine("Sikertelen rögzítés: A tag tagsága lejárt!");
                return false;
            }

            benneTag = true;
            ujTagok.Add($"{adatokT[0]};{adatokT[1]};{adatokT[2]};{adatokT[3]};{adatokT[4]};{(adatokT[5] == "nincs" ? konyvId : adatokT[5] + ", " + konyvId)}");
        }
        else
        {
            ujTagok.Add(tag);
        }
    }

    if (benneKonyv && benneTag)
    {
        File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
        File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));
    }
    else if (!benneKonyv)
    {
        Console.WriteLine("\nSikertelen rögzítés: Ilyen azonosítóval rendelkező könyv nem szerepel a nyilvántartásban!");
        return false;
    }
    else if (!benneTag)
    {
        Console.WriteLine("\nSikertelen rögzítés: Ilyen azonosítóval rendelkező tag nem szerepel a nyilvántartásban!");
        return false;
    }

    DateOnly kezdet = DateOnly.FromDateTime(DateTime.Now);
    DateOnly lejarat = kezdet.AddMonths(1);

    using StreamWriter sw = File.AppendText("kolcsonzes.csv");
    sw.WriteLine($"{konyvId};{tagId};{kezdet};{lejarat};0");

    return true;
}

bool torlesKolcsonzes()
{
    return false;
}

bool hosszabitasKolcsonzes()
{
    return false;
}

//-------------------------
// Keresés

bool tagKereses()
{
    return false;
}

bool konyvKereses()
{
    return false;
}

bool kolcsonzesKereses()
{
    return false;
}

//--------------------------------------------------
// User

bool inditasUser()
{
    while (true)
    {
        Console.Write("1) Könyvek keresése\n2) Profil megtekintése\n3) Profil törlése\n4) Kivett könyvek listája\n0) Kilépés\n\nA választott funkció betűjele: ");
        string valasztas = Console.ReadLine();

        switch(valasztas) {
        case "1": if (konyvKereses()) return true; break;
        case "2": if (profil()) return true; break;
        case "3": if (torles()) return true; break;
        case "4": if (kivettKonyvek()) return true; break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: Console.WriteLine("Sikertelen indítás! Adjon meg egy létező opciót!"); break;
        }
    }
}

bool profil()
{
    return false;
}

bool torles()
{
    return false;
}

bool kivettKonyvek()
{
    return false;
}

//--------------------------------------------------
// Indító

Console.WriteLine("=========================\nALEXANDRIA KÖNYVTÁR\n=========================\n\n");

string felhasznalo = belepes();

while (felhasznalo == "sikertelen")
{
    felhasznalo = belepes();
}

if (felhasznalo == "admin")
{
    inditasAdmin();
}
else if (felhasznalo == "user")
{
    inditasUser();
}
