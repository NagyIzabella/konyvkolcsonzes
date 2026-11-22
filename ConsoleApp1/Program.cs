﻿using System.IO;

// konyvek.csv          id(string);cim(string);szerzo(string);mufaj(string);felnott(bool);statusz(bool)\n
// tagok.csv            id(string);nev(string);kor(int);tagsag_kezdete(string);tagsag_lejarata(string);kikolcsonzott_konyvek(list)
// kolcsonzes.csv       konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok

if (!File.Exists("konyvek.csv")) File.WriteAllText("konyvek.csv", "id;cim;szerzo;mufaj;felnott;statusz\n");
if (!File.Exists("tagok.csv")) File.WriteAllText("tagok.csv", "id;nev;kor;tagsag_kezdete;tagsag_lejarata;kikolcsonzott_konyvek\n");
if (!File.Exists("kolcsonzes.csv")) File.WriteAllText("kolcsonzes.csv", "konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok\n");
if (!File.Exists("login.csv")) File.WriteAllText("login.csv","felhasznalonev;jelszo\n");

//--------------------------------------------------
// Belépés

void belepes() {
    bool jo=false;
    Console.Write("Bejelentkezés\n\nFelhasználónév: ");
    string username = Console.ReadLine();

    Console.Write("Jelszó: ");
    string password = Console.ReadLine();

    if (username == "admin" && password == "admin")
    {
        inditasAdmin();
    }
    else
    {
        foreach (string sor in File.ReadLines("tagok.csv"))
        {
            if (sor.Contains(username) && (sor.Contains(password)))
            {
                jo = true;
                inditasUser();
            }
        }

        if (!jo)
        {
            Console.WriteLine("HIBÁS FELHASZNÁLÓNÉV VAGY JELSZÓ!");
        }
    }
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
    return bemenet == "0" || bemenet == "1";
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
        default: sikeres = false; break;
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
        default: sikeres = false; break;
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
        default: sikeres = false; break;
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
        default: sikeres = false; break;
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
        default: sikeres = false; break;
    }

    return sikeres;
}

//-------------------------
// Tagműveletek

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
// Könyvműveletek

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

    Console.Write("\nHa a könyv SZERZŐJÉT módosítaná, a szerző neve elé rakjon egy 'S' betűt!\nHa a könyv MŰFAJÁT módosítaná, a műfaj neve elé rakjon egy 'M' betűt!\nHa a könyv BESOROLÁSÁT módosítaná, írjon egy 'B' betűt, a program automatikusan átírja a másik kategóriára!\nMinden más esetben a könyv CÍME fog frissülni!\nAz azonosító módosítására NINCS opció!\nFONTOS! Ha az új cím kezdőbetűje megegyezik az említett betűk egyikével ('S', 'M', 'B'), akkor rakjon elé egy felkiáltójelet! (!) \n\nÚj adat: ");
    string ujAdat = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(ujAdat))
    {
        Console.WriteLine("Sikertelen módosítás: Adjon meg minden adatot!");
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
            ujKonyvek.Add($"{adatok[0]};{(normalizalKarakter(ujAdat[0]) == "!" ? ujAdat[1..] : normalizalKarakter(ujAdat[0]) is "S" or "M" or "B" ? adatok[1] : ujAdat)};{(normalizalKarakter(ujAdat[0]) == "S" ? ujAdat[1..] : adatok[2])};{(normalizalKarakter(ujAdat[0]) == "M" ? ujAdat[1..] : adatok[3])};{(normalizalKarakter(ujAdat[0]) == "B" ? !bool.Parse(adatok[4]) : adatok[4])};{adatok[5]}");
        }
        else
        {
            ujKonyvek.Add(konyv);
        }
    }

    if (!benne)
    {
        Console.WriteLine("Sikertelen módosítás: A könyv nem szerepel a nyilvántartásban!");
        sikeres = false;
    }
    else
    {
        File.WriteAllLines("konyvek.csv", new string[] { konyvek[0] }.Concat(ujKonyvek));
    }

    return sikeres;
}

//--------------------------------------------------
// User

void inditasUser()
{
    Console.WriteLine("tbt");
}

//--------------------------------------------------
// Indító

// Console.WriteLine("=========================\nALEXANDRIA KÖNYVTÁR\n=========================\n\n")
