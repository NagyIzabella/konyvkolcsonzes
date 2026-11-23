using System.IO;

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
                inditasAdmin();
                return "admin";
            }
            else if (!bool.Parse(oszlopok[3]))
            {
                inditasUser();
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
    return bemenet.ToUpper().Replace(" ", "").Replace("-", "").Replace("_", "");
}

string normalizalKarakter(char bemenet)
{
    return normalizal(bemenet.ToString());
}

bool szamE(string bemenet)
{
    return int.TryParse(bemenet, out int szam) && szam > 0;
}

bool besorolasCheck(string bemenet)
{
    return bemenet == "0" || bemenet == "1" || normalizal(bemenet) == "F" || normalizal(bemenet) == "T" || normalizal(bemenet) == "FALSE" || normalizal(bemenet) == "TRUE";
}

string adatCheck(string eredeti, string regi, string uj)
{
    return normalizal(eredeti) == normalizal(regi) ? uj : eredeti;
}

//--------------------------------------------------
// Admin

//-------------------------
// Menü

bool inditasAdmin() {
    Console.Write("1) Tag\n2) Könyv\n3) Kölcsönzés\n4) Keresés\n0) Kilépés\n\nA választott funkció betűjele: ");
    string valasztas = Console.ReadLine();
    bool sikeres = true;

    switch(valasztas) {
        case "1": tag(); break;
        case "2": konyv(); break;
        case "3": kolcsonzes(); break;
        case "4": kereses(); break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: Console.WriteLine("Sikertelen indítás! Adjon meg egy létező opciót!"); sikeres = false; break;
    }

    return sikeres;
}

bool tag() {
    Console.Write("--------------------\nTag\n\n1) Tag felvétele\n2) Tag törlése\n3) Tagok megtekintése\n4) Tagok adatmódosítása\n0) Kilépés\n\nA választott funkció betűjele: ");
    string valasztas = Console.ReadLine();
    bool sikeres = true;

    switch(valasztas) {
        case "1": felvetelTag(); break;
        case "2": torlesTag(); break;
        case "3": listazasTag(); break;
        case "4": modositasTag(); break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: Console.WriteLine("Sikertelen végrehajtás! Adjon meg egy létező opciót!"); sikeres = false; break;
    }

    return sikeres;
}

bool konyv() {
    Console.Write("--------------------\nKönyv\n\n1) Könyv felvétele\n2) Könyv törlése\n3) Könyvek megtekintése\n4) Könyvek adatmódosítása\n0) Kilépés\n\nA választott funkció betűjele: ");
    string valasztas = Console.ReadLine();
    bool sikeres = true;

    switch(valasztas) {
        case "1": felvetelKonyv(); break;
        case "2": torlesKonyv(); break;
        case "3": listazasKonyv(); break;
        case "4": modositasKonyv(); break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: Console.WriteLine("Sikertelen végrehajtás! Adjon meg egy létező opciót!"); sikeres = false; break;
    }

    return sikeres;
}

bool kolcsonzes() {
    Console.Write("--------------------\nKölcsönzés\n\n1) Kölcsönzés hozzáadása\n2) Kölcsönzés törlése\n3) Kölcsönzés hosszabítása\n0) Kilépés\n\nA választott funkció betűjele: ");
    string valasztas = Console.ReadLine();
    bool sikeres = true;

    switch(valasztas) {
        case "1": felvetelKolcsonzes(); break;
        case "2": torlesKolcsonzes(); break;
        case "3": hosszabitasKolcsonzes(); break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: Console.WriteLine("Sikertelen végrehajtás! Adjon meg egy létező opciót!"); sikeres = false; break;
    }

    return sikeres;
}

bool kereses() {
    Console.Write("--------------------\nKeresés\n\n1) Tagok keresése\n2) Könyvek keresése\n3) Kölcsönzések listája\n0) Kilépés\n\nA választott funkció betűjele: ");
    string valasztas = Console.ReadLine();
    bool sikeres = true;

    switch(valasztas) {
        case "1": tagKereses(); break;
        case "2": konyvKereses(); break;
        case "3": kolcsonzesKereses(); break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: Console.WriteLine("Sikertelen végrehajtás! Adjon meg egy létező opciót!"); sikeres = false; break;
    }

    return sikeres;
}

//-------------------------
// Tag

bool felvetelTag()
{
    Console.Write("--------------------\nTag hozzáadása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("Név: ");
    string nev = Console.ReadLine();

    Console.Write("Kor: ");
    string korBemenet = Console.ReadLine();
    if (szamE(korBemenet))
    {
        int kor = int.Parse(korBemenet);
        DateOnly felvetel = DateOnly.FromDateTime(DateTime.Now);
        DateOnly lejarat = felvetel.AddMonths(3);

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nev))
        {
            Console.WriteLine("Sikertelen hozzáadás: Adjon meg minden adatot!");
            return false;
        }

        string[] tagok = File.ReadAllLines("tagok.csv");

        foreach (string tag in tagok.Skip(1))
        {
            string[] adatok = tag.Split(";");
            if (normalizal(adatok[0]) == normalizal(id))
            {
                Console.WriteLine("Sikertelen hozzáadás: Ilyen azonosítójú tag már létezik!");
                return false;
            }
        }

        using StreamWriter sw = File.AppendText("tagok.csv");
        sw.WriteLine($"{id};{nev};{kor};{felvetel};{lejarat};nincs");

        return true;
    }
    else
    {
        Console.WriteLine("Sikertelen hozzáadás: Érvénytelen életkor!");
        return false;
    }
}

bool torlesTag()
{
    bool sikeres = true;
    bool benne = false;

    Console.Write("--------------------\nTag törlése\n\nAzonosító: ");
    string id = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("Sikertelen törlés: Adjon meg egy azonosítót!");
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
        }
        else
        {
            ujTagok.Add(tag);
        }
    }
    if (!benne)
    {
        Console.WriteLine("Sikertelen törlés: A tag nem szerepel a nyilvántartásban!");
        sikeres = false;
    }
    else
    {
        File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));
    }

    return sikeres;
}

void listazasTag()
{
    string[] sorok = File.ReadAllLines("tagok.csv");

    if (sorok.Length == 1)
    {
        Console.WriteLine("Jelenleg nincsenek regisztrált tagok.");
    }
    else
    {
        Console.WriteLine($"Jelenlegi tagok száma: {sorok.Length - 1}");
        foreach (string sor in sorok.Skip(1))
        {
            string[] adatok = sor.Split(";");
            Console.WriteLine($"--------------------\nAzonosító: {adatok[0]}\nNév: {adatok[1]}\nKor: {adatok[2]}\nTagság kezdete: {adatok[3]}\nTagság lejárata: {adatok[4]}\nJelenleg kikölcsönzött könyvek: {(adatok[5] == "nincs" ? "Jelenleg nincs kikölcsönzött könyve." : adatok[5])}\n");
        }
    }
}

bool modositasTag()
{
    bool sikeres = true;
    bool benne = false;

    DateOnly maiDatum = DateOnly.FromDateTime(DateTime.Now);
    DateOnly ujLejarat = maiDatum.AddMonths(3);

    Console.Write("--------------------\nTag adatmódosítása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("\nHa a tag KORÁT módosítaná, akkor csak EGY SZÁMOT adjon meg!\nHa a TAGSÁG FRISSÍTÉSÉT szeretné rögzíteni, írjon be egy 'F' betűt!\nMinden más esetben a tag NEVE fog frissülni!\nAz azonosító módosítására NINCS opció!\n\nÚj adat: ");
    string ujAdat = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(ujAdat))
    {
        Console.WriteLine("Sikertelen módosítás: Adjon meg minden adatot!");
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
            ujTagok.Add($"{adatok[0]};{(normalizal(ujAdat) != "F" && !szamE(ujAdat) ? ujAdat : adatok[1])};{(szamE(ujAdat) ? ujAdat : adatok[2])};{(normalizal(ujAdat) == "F" ? maiDatum : adatok[3])};{(normalizal(ujAdat) == "F" ? ujLejarat : adatok[4])};{adatok[5]}");
        }
        else
        {
            ujTagok.Add(tag);
        }
    }

    if (!benne)
    {
        Console.WriteLine("Sikertelen módosítás: A tag nem szerepel a nyilvántartásban!");
        sikeres = false;
    }
    else
    {
        File.WriteAllLines("tagok.csv", new string[] { tagok[0] }.Concat(ujTagok));
    }

    return sikeres;
}

//-------------------------
// Könyv

bool felvetelKonyv()
{
    Console.Write("--------------------\nKönyv hozzáadása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("Cím: ");
    string cim = Console.ReadLine();

    Console.Write("Szerző: ");
    string szerzo = Console.ReadLine();

    Console.Write("Műfaj: ");
    string mufaj = Console.ReadLine();

    Console.Write("Besorolás (0 -- nem korhatáros; 1 -- korhatáros): ");
    string besorolas = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(cim) || string.IsNullOrWhiteSpace(szerzo) || string.IsNullOrWhiteSpace(mufaj) || !besorolasCheck(besorolas))
    {
        Console.WriteLine("Sikertelen hozzáadás: Kérem, adjon meg minden adatot!");
        return false;
    }

    string[] konyvek = File.ReadAllLines("konyvek.csv");

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == normalizal(id))
        {
            Console.WriteLine("Sikertelen hozzáadás: Ilyen azonosítójú könyv már létezik!");
            return false;
        }
    }

    using StreamWriter sw = File.AppendText("konyvek.csv");
    sw.WriteLine($"{id};{cim};{szerzo};{mufaj};{besorolas == "1"};{true}");

    return true;
}

bool torlesKonyv()
{
    bool sikeres = true;
    bool benne = false;

    Console.Write("--------------------\nKönyv törlése\n\nAzonosító: ");
    string id = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id))
    {
        Console.WriteLine("Sikertelen törlés: Adjon meg egy azonosítót!");
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
        }
        else
        {
            ujKonyvek.Add(konyv);
        }
    }
    if (!benne)
    {
        Console.WriteLine("Sikertelen törlés: A könyv nem szerepel a nyilvántartásban!");
        sikeres = false;
    }
    else
    {
        File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
    }

    return sikeres;
}

void listazasKonyv()
{
    string[] sorok = File.ReadAllLines("konyvek.csv");

    if (sorok.Length == 1)
    {
        Console.WriteLine("Jelenleg nincsenek regisztrált könyvek.");
    }
    else
    {
        Console.WriteLine($"Jelenlegi könyvek száma: {sorok.Length - 1}");
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
    bool sikeres = true;
    Console.Write("--------------------\nKönyv adatmódosítása\n\nAzonosító: ");
    string id = Console.ReadLine();

    Console.Write("Régi adat (Korhatár alapján történő besorolás esetén '1' vagy 'TRUE' ha korhatáros, '0' vagy 'FALSE' ha nem): ");
    string regiAdat = Console.ReadLine();

    Console.Write("Új adat (Korhatár alapján történő besorolás esetén '1' vagy 'TRUE' ha korhatáros, '0' vagy 'FALSE' ha nem):");
    string ujAdat = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id)|| string.IsNullOrWhiteSpace(regiAdat) || string.IsNullOrWhiteSpace(ujAdat))
    {
        Console.WriteLine("Sikertelen módosítás: Adjon meg minden adatot!");
        return false;
    }

    string[] konyvek = File.ReadAllLines("konyvek.csv");
    List<string> ujKonyvek = [];

    foreach (string konyv in konyvek.Skip(1))
    {
        string[] adatok = konyv.Split(";");
        if (normalizal(adatok[0]) == normalizal(id) && normalizal(konyv).Contains(normalizal(regiAdat)))
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
        Console.WriteLine("Sikertelen módosítás: Ilyen azonosítóval/adattal rendelkező könyv nem szerepel a nyilvántartásban!");
        sikeres = false;
    }
    else
    {
        File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
    }

    return sikeres;
}

//-------------------------
// Kölcsönzés

bool felvetelKolcsonzes()
{
    return false;
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
    Console.WriteLine("tbt");
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
    bool sikeres = false;
    while (!sikeres)
    {
        sikeres = inditasAdmin();
    }
}
else
{
    bool sikeres = false;
    while (!sikeres)
    {
        sikeres = inditasUser();
    }
}