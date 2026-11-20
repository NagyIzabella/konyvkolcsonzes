﻿using System.IO;

// konyvek.csv          id(int);nev(string);szerzo(string);felnott(bool);statusz(bool)\n
// tagok.csv            id(int);nev(string);kor(int);tagsag_kezdete(string);tagsag_lejarata(string);kikolcsonzott_konyvek(list)
// kolcsonzes.csv       konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok

if (!File.Exists("konyvek.csv")) File.WriteAllText("konyvek.csv", "id;nev;szerzo;felnott;statusz\n");
if (!File.Exists("tagok.csv")) File.WriteAllText("tagok.csv", "id;nev;kor;tagsag_kezdete;tagsag_lejarata;kikolcsonzott_konyvek\n");
if (!File.Exists("kolcsonzes.csv")) File.WriteAllText("kolcsonzes.csv", "konyv_id;tag_id_;datum;visszaviteli_ido;meghosszabbitasok\n");

//--------------------------------------------------
// Belépés

string belepes() {
    Console.Write("Bejelentkezés\n\nFelhasználónév: ");
    string username = Console.ReadLine();

    Console.Write("Jelszó: ");
    string password = Console.ReadLine();

    // Ide kell a check hogy ki a felhasználó, vagy nem, rád bízom
}

//--------------------------------------------------
// Admin

string normalizal(string bemenet)
{
    return bemenet.ToUpper().Replace(" ", "").Replace("-", "").Replace("_", "");
}

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
        case "2": torlesKonyv(); break;
        case "3": hosszabitasKolcsonzes(); break;
        case "0": Console.WriteLine("Kilépés. . ."); break;
        default: sikeres = false; break;
    }

    return sikeres;
}

bool felvetelTag() {
    bool sikeres = true;
    Console.Write("Azonosító: ");
    string id = Console.ReadLine();

    Console.Write("Név: ");
    string nev = Console.ReadLine();

    Console.Write("Kor: ");
    int kor = int.Parse(Console.ReadLine());

    DateOnly felvetel = new();

    DateOnly lejarat = felvetel.AddMonths(3);

    string[] tagok = File.ReadAllLines("tagok.csv");

    foreach (string tag in tagok.Skip(1))
    {
        string[] adatok = tag.Split(";");
        if (normalizal(adatok[0]) == normalizal(id))
        {
            Console.WriteLine("Sikertelen hozzáadás: Ilyen azonosítójú tag már létezik!");
            sikeres = false;
            return sikeres;
        }
    }

    using StreamWriter sw = File.AppendText("tagok.csv");
    sw.WriteLine($"{id};{nev};{kor};{felvetel};{lejarat};nincs");

    return sikeres;
}

//--------------------------------------------------
// User

//--------------------------------------------------
// Indító

// Console.WriteLine("=========================\nALEXANDRIA KÖNYVTÁR\n=========================\n\n")











