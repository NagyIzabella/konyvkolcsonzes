using System.IO;
using System.Linq;
using System.Collections.Generic;

// konyvek.csv          id(string);cim(string);szerzo(string);mufaj(string);korhatar(bool);statusz(bool)\n
// tagok.csv            id(string);nev(string);kor(int);tagsag_kezdete(string);tagsag_lejarata(string);kikolcsonzott_konyvek(list)
// kolcsonzes.csv       konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok
// login.csv            id(string);felhasznalonev(string);jelszo(string);admin(bool)

if (!File.Exists("konyvek.csv")) File.WriteAllText("konyvek.csv", "id;cim;szerzo;mufaj;korhatar;statusz\n");
if (!File.Exists("tagok.csv")) File.WriteAllText("tagok.csv", "id;nev;kor;tagsag_kezdete;tagsag_lejarata;kikolcsonzott_konyvek\n");
if (!File.Exists("kolcsonzes.csv")) File.WriteAllText("kolcsonzes.csv", "konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok\n");
if (!File.Exists("login.csv")) File.WriteAllText("login.csv", "id;felhasznalonev;jelszo;admin\n");

//--------------------------------------------------
// Belépés

string bejelentkezettId = "";
string belepes()
{
    Console.Write("Bejelentkezés\n\nFelhasználónév: ");
    string username = Console.ReadLine();

    Console.Write("Jelszó: ");
    string password = Console.ReadLine();

    foreach (string sor in File.ReadLines("login.csv").Skip(1))
    {
        if (string.IsNullOrWhiteSpace(sor)) continue;

        string[] oszlopok = sor.Split(';');

        if (oszlopok.Length != 4 || !bool.TryParse(oszlopok[3].Trim(), out bool admin))
        {
            Console.WriteLine($"Hibás sor a login.csv-ben: {sor}");
            continue;
        }

        if (oszlopok[1] == username && oszlopok[2] == password)
        {
            bejelentkezettId = oszlopok[0];
            return admin ? "admin" : "user";
        }
    }

    Console.WriteLine("HIBÁS FELHASZNÁLÓNÉV VAGY JELSZÓ!\n");
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

void loginTorles(string id)
{
    string[] login = File.ReadAllLines("login.csv");
    File.WriteAllLines("login.csv", login.Where((sor, i) => i == 0 || normalizal(sor.Split(';')[0]) != normalizal(id)));
}

void loginNevFrissit(string id, string ujNev)
{
    string[] login = File.ReadAllLines("login.csv");

    for (int i = 1; i < login.Length; i++)
    {
        string[] oszlopok = login[i].Split(';');
        if (oszlopok.Length == 4 && normalizal(oszlopok[0]) == normalizal(id))
        {
            login[i] = $"{oszlopok[0]};{ujNev};{oszlopok[2]};{oszlopok[3]}";
        }
    }

    File.WriteAllLines("login.csv", login);
}
//--------------------------------------------------
// Admin

//-------------------------
// Menü

bool inditasAdmin()
{
    while (true)
    {
        Console.Write("\n1) Tag\n2) Könyv\n3) Kölcsönzés\n4) Keresés\n0) Kilépés\n\nA választott menüpont száma: ");
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
        
        foreach (string sor in File.ReadLines("login.csv").Skip(1))
        {
            string[] oszlopok = sor.Split(";");
            if (oszlopok.Length < 3) continue;

            if (normalizal(oszlopok[0]) == normalizal(id))
            {
                Console.WriteLine("\nSikertelen hozzáadás: Ez az azonosító már foglalt a bejelentkezési adatok között!");
                return false;
            }

            if (oszlopok[1].Trim() == nev.Trim())
            {
                Console.WriteLine("\nSikertelen hozzáadás: Ilyen nevű felhasználó már létezik a bejelentkezési adatok között!");
                return false;
            }
        }

        File.AppendAllText("tagok.csv", $"{id};{nev};{kor};{felvetel:yyyy-MM-dd};{lejarat:yyyy-MM-dd};nincs\n");
        File.AppendAllText("login.csv", $"{id};{nev};{id};false\n");
        // using StreamWriter sw = File.AppendText("tagok.csv");
        // sw.WriteLine($"{id};{nev};{kor};{felvetel};{lejarat};nincs");

        return true;
    }
    else
    {
        Console.WriteLine("\nSikertelen hozzáadás: Érvénytelen életkor!");
        return false;
    }
}

bool torlesTag()
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

            if (adatok[5] != "nincs")
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
        loginTorles(id);
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
    string valtozottNev = null;
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
            if (nevModositas) valtozottNev = ujNev;
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
        if (valtozottNev != null) loginNevFrissit(id, valtozottNev);
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

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(regiAdat) || string.IsNullOrWhiteSpace(ujAdat))
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
    Console.Write("--------------------\nKölcsönzés törlése (visszavétel)\n\nKikölcsönzött könyv azonosítója: ");
    string konyvId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(konyvId))
    {
        Console.WriteLine("\nSikertelen törlés: Adjon meg egy azonosítót!");
        return false;
    }

    // 1) A kölcsönzés megkeresése
    string[] kolcsonzesek = File.ReadAllLines("kolcsonzes.csv");
    List<string> ujKolcsonzesek = [];
    string tagId = null;

    foreach (string sor in kolcsonzesek.Skip(1))
    {
        string[] adatok = sor.Split(";");
        if (normalizal(adatok[0]) == normalizal(konyvId))
        {
            tagId = adatok[1];
        }
        else
        {
            ujKolcsonzesek.Add(sor);
        }
    }

    if (tagId == null)
    {
        Console.WriteLine("\nSikertelen törlés: Ez a könyv nincs kikölcsönözve!");
        return false;
    }

    // 2) Megerősítés
    Console.Write("\nBiztosan törlöd a kölcsönzést?\n(0) Nem\n(1) Igen\nVálasztás: ");
    string valasztas = Console.ReadLine();

    if (valasztas != "1")
    {
        if (valasztas != "0") Console.WriteLine("\nHibás bemenet!");
        return false;
    }

    // 3) A könyv újra "bent" van
    string[] konyvek = File.ReadAllLines("konyvek.csv");
    List<string> ujKonyvek = [];

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == normalizal(konyvId))
        {
            ujKonyvek.Add($"{adatok[0]};{adatok[1]};{adatok[2]};{adatok[3]};{adatok[4]};true");
        }
        else
        {
            ujKonyvek.Add(konyv);
        }
    }

    // 4) A könyv kikerül a tag listájából
    string[] tagok = File.ReadAllLines("tagok.csv");
    List<string> ujTagok = [];

    foreach (string tag in tagok.Skip(1))
    {
        string[] adatok = tag.Split(";");
        if (normalizal(adatok[0]) == normalizal(tagId))
        {
            List<string> maradek = adatok[5].Split(", ").Where(k => normalizal(k) != normalizal(konyvId)).ToList();
            string ujLista = maradek.Count == 0 ? "nincs" : string.Join(", ", maradek);
            ujTagok.Add($"{adatok[0]};{adatok[1]};{adatok[2]};{adatok[3]};{adatok[4]};{ujLista}");
        }
        else
        {
            ujTagok.Add(tag);
        }
    }

    File.WriteAllLines("kolcsonzes.csv", new string[] { kolcsonzesek[0] }.Concat(ujKolcsonzesek));
    File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
    File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));

    Console.WriteLine("\nA kölcsönzés sikeresen törölve, a könyv visszakerült a könyvtárba!");
    return true;
}

bool hosszabitasKolcsonzes()
{
    const int MAX_HOSSZABBITAS = 2;   // ennyiszer lehet egy kölcsönzést meghosszabbítani
    const int HOSSZABBITAS_HONAP = 1; // egyszerre ennyi hónappal tolódik a visszaviteli határidő

    Console.Write("--------------------\nKölcsönzés hosszabbítása\n\nKönyv azonosítója: ");
    string konyvId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(konyvId))
    {
        Console.WriteLine("\nSikertelen hosszabbítás: Adjon meg egy azonosítót!");
        return false;
    }

    string[] kolcsonzesek = File.ReadAllLines("kolcsonzes.csv");
    List<string> ujKolcsonzesek = [];
    bool benne = false;

    foreach (string sor in kolcsonzesek.Skip(1))
    {
        string[] adatok = sor.Split(";");

        if (normalizal(adatok[0]) != normalizal(konyvId))
        {
            ujKolcsonzesek.Add(sor);
            continue;
        }

        benne = true;

        if (!DateOnly.TryParse(adatok[3], out DateOnly visszaviteli) || !int.TryParse(adatok[4], out int hosszabbitasok))
        {
            Console.WriteLine("\nSikertelen hosszabbítás: Hibás adat a kölcsönzés nyilvántartásában!");
            return false;
        }

        if (hosszabbitasok >= MAX_HOSSZABBITAS)
        {
            Console.WriteLine($"\nSikertelen hosszabbítás: A kölcsönzés már {MAX_HOSSZABBITAS}x meg lett hosszabbítva!");
            return false;
        }

        if (visszaviteli < DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("\nSikertelen hosszabbítás: A visszaviteli határidő már lejárt!");
            return false;
        }

        // a tag tagsága is érvényes legyen
        foreach (string tag in File.ReadLines("tagok.csv").Skip(1))
        {
            string[] tagAdatok = tag.Split(";");
            if (normalizal(tagAdatok[0]) == normalizal(adatok[1]) && DateOnly.Parse(tagAdatok[4]) < DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("\nSikertelen hosszabbítás: A tag tagsága lejárt!");
                return false;
            }
        }

        DateOnly ujVisszaviteli = visszaviteli.AddMonths(HOSSZABBITAS_HONAP);
        ujKolcsonzesek.Add($"{adatok[0]};{adatok[1]};{adatok[2]};{ujVisszaviteli};{hosszabbitasok + 1}");

        Console.WriteLine($"\nSikeres hosszabbítás! Új visszaviteli dátum: {ujVisszaviteli} ({hosszabbitasok + 1}. hosszabbítás)");
    }

    if (!benne)
    {
        Console.WriteLine("\nSikertelen hosszabbítás: Ilyen könyvhöz nem tartozik kölcsönzés!");
        return false;
    }

    File.WriteAllLines("kolcsonzes.csv", new string[] { kolcsonzesek[0] }.Concat(ujKolcsonzesek));
    return true;

return false;
}

//-------------------------
// Keresés

// Sanyi-féle keresés

//Console.Write("Mit keress: ");
//string keress = Console.ReadLine();
//Console.WriteLine();

//bool benne = false;
//List<string> josorok = new();

//foreach (string sor in File.ReadLines("adatbazis.txt"))
//{
//    if (sor.Contains(keress))
//    {
//        benne = true;
//        josorok.Add(sor);
//    }
//}
//if (benne == true)
//{
//    Console.WriteLine($" {josorok.Count()} találat:");
//    foreach (string josor in josorok)
//    {
//        Console.WriteLine(josor);
//        Console.WriteLine();
//    }
//}
//else
//{
//    Console.WriteLine("Nem található adat!");
//    Console.WriteLine();
//}

bool tagKereses()
{
    Console.Write("--------------------\nTagok keresése\n\nKerés: ");
    string keres = Console.ReadLine();
    Console.WriteLine();

    if (keres.Contains(';'))
    {
        Console.WriteLine("A keresés nem tartalmazhat ';' karaktert");
        return false;
    }

    bool benne = false;
    List<string> josorok = new();

    foreach (string sor in File.ReadLines("tagok.csv"))
    {
        if (normalizal(sor).Contains(normalizal(keres)))
        {
            benne = true;
            josorok.Add(sor);
        }
    }
    if (benne == true)
    {
        Console.WriteLine($" {josorok.Count()} találat:");
        foreach (string josor in josorok)
        {
            string[] kimenet = josor.Split(";");
            Console.WriteLine($"--------------------\nAzonosító: {kimenet[0]}\nNév: {kimenet[1]}\nKor: {kimenet[2]}\nTagság kezdete: {kimenet[3]}\nTagság lejárata: {kimenet[4]}\nKikölcsönzött könyvek: {kimenet[5]}\n--------------------\n");
        }
    }
    else
    {
        Console.WriteLine("Nem található adat!\n ");
    }

    return true;
}

bool konyvKereses()
{
    Console.Write("--------------------\nKönyek keresése\n\nKerés: ");
    string keres = Console.ReadLine();
    Console.WriteLine();

    if (keres.Contains(';'))
    {
        Console.WriteLine("A keresés nem tartalmazhat ';' karaktert");
        return false;
    }

    bool benne = false;
    List<string> josorok = new();

    foreach (string sor in File.ReadLines("konyvek.csv").Skip(1))
    {
        if (normalizal(sor).Contains(normalizal(keres)))
        {
            benne = true;
            josorok.Add(sor);
        }
    }
    if (benne == true)
    {
        Console.WriteLine($" {josorok.Count()} találat:");
        foreach (string josor in josorok)
        {
            string[] kimenet = josor.Split(";");
            Console.WriteLine($"--------------------\nAzonosító: {kimenet[0]}\nCím: {kimenet[1]}\nSzerző: {kimenet[2]}\nMűfaj: {kimenet[3]}\nKorhatáros: {(bool.Parse(kimenet[4]) ? "Igen" : "Nem")}\nKi van kölcsönözve: {(bool.Parse(kimenet[5]) ? "Igen" : "Nem")}\n--------------------\n");
        }
    }
    else
    {
        Console.WriteLine("Nem található adat!\n ");
    }

    return true;
}

bool kolcsonzesKereses()
{
    Console.Write("--------------------\nKölcsönzések keresése\n\nKerés: ");
    string keres = Console.ReadLine();
    Console.WriteLine();

    if (keres.Contains(';'))
    {
        Console.WriteLine("A keresés nem tartalmazhat ';' karaktert");
        return false;
    }

    bool benne = false;
    List<string> josorok = new();

    foreach (string sor in File.ReadLines("kolcsonzes.csv"))
    {
        if (normalizal(sor).Contains(normalizal(keres)))
        {
            benne = true;
            josorok.Add(sor);
        }
    }
    if (benne == true)
    {
        Console.WriteLine($" {josorok.Count()} találat:");
        foreach (string josor in josorok)
        {
            string[] kimenet = josor.Split(";");
            Console.WriteLine($"--------------------\nA könyv azonosítója: {kimenet[0]}\nTag azonosítója: {kimenet[1]}\nDátum: {kimenet[2]}\nVisszaviteli dátum: {kimenet[3]}\nMeghosszabbítások száma:{kimenet[4]}\n--------------------\n");
        }
    }
    else
    {
        Console.WriteLine("Nem található adat!\n ");
    }

    return true;
}

//--------------------------------------------------
// User

bool inditasUser()
{
    while (true)
    {
        Console.Write("\n1) Könyvek keresése\n2) Profil megtekintése\n3) Profil törlése\n4) Kivett könyvek listája\n0) Kilépés\n\nA választott menüpont száma: ");
        string valasztas = Console.ReadLine();

        switch (valasztas)
        {
            case "1": konyvKereses(); break;
            case "2": profil(); break;
            case "3": if (torles()) return true; break;   // sikeres törlés után a program kilép
            case "4": kivettKonyvek(); break;
            case "0": Console.WriteLine("Kilépés..."); return true;
            default: Console.WriteLine("\nSikertelen indítás! Adjon meg egy létező opciót!"); break;
        }
    }
}

bool profil()
{
    foreach (string sor in File.ReadLines("tagok.csv").Skip(1))
    {
        string[] adatok = sor.Split(';');

        if (normalizal(adatok[0]) == normalizal(bejelentkezettId))
        {
            Console.WriteLine($"\n--------------------\nAzonosító: {adatok[0]}\nNév: {adatok[1]}\nKor: {adatok[2]}\nTagság kezdete: {adatok[3]}\nTagság lejárata: {adatok[4]}\nJelenleg kikölcsönzött könyvek: {(adatok[5] == "nincs" ? "Jelenleg nincs kikölcsönzött könyve." : adatok[5])}\n--------------------");
            return true;
        }
    }

    Console.WriteLine("\nA profil nem található!");
    return false;
}

bool torles()
{
    bool benne = false;
    string[] tagok = File.ReadAllLines("tagok.csv");
    List<string> ujTagok = [];

    foreach (string tag in tagok.Skip(1))
    {
        string[] adatok = tag.Split(';');

        if (normalizal(adatok[0]) == normalizal(bejelentkezettId))
        {
            benne = true;

            if (adatok[5] != "nincs")
            {
                Console.WriteLine("\nSikertelen törlés: Önnél még van kikölcsönzött könyv, előbb vigye vissza!");
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
        Console.WriteLine("\nSikertelen törlés: A profil nem található!");
        return false;
    }

    Console.Write("\nBiztosan szeretnéd törölni a tagságodat?\n(0) Nem\n(1) Igen\nVálasztás: ");
    string valasztas = Console.ReadLine();

    if (valasztas == "1")
    {
        File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));
        loginTorles(bejelentkezettId);

        Console.WriteLine("\nA tagságod sikeresen törölted!");
        return true;
    }

    if (valasztas != "0") Console.WriteLine("\nHibás bemenet!");
    return false;
}

bool kivettKonyvek()
{
    string[] konyvek = File.ReadAllLines("konyvek.csv");
    bool volt = false;

    foreach (string kolcsonSor in File.ReadLines("kolcsonzes.csv").Skip(1))
    {
        string[] kolcsonAdatok = kolcsonSor.Split(';');

        if (normalizal(kolcsonAdatok[1]) != normalizal(bejelentkezettId))
        {
            continue;
        }

        foreach (string konyvSor in konyvek.Skip(1))
        {
            string[] konyvAdatok = konyvSor.Split(';');

            if (normalizal(konyvAdatok[0]) == normalizal(kolcsonAdatok[0]))
            {
                volt = true;
                Console.WriteLine($"--------------------\nAzonosító: {konyvAdatok[0]}\nCím: {konyvAdatok[1]}\nSzerző: {konyvAdatok[2]}\nKikölcsönzés dátuma: {kolcsonAdatok[2]}\nVisszaviteli dátum: {kolcsonAdatok[3]}\nMeghosszabbítások száma: {kolcsonAdatok[4]}\n--------------------");
                break;
            }
        }
    }

    if (!volt)
    {
        Console.WriteLine("\nJelenleg nincs kikölcsönzött könyve.");
    }

    return true;
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