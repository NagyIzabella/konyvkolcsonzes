if (!File.Exists("konyvek.csv")) File.WriteAllText("konyvek.csv", "id;nev;szerzo;korhatar;statusz");
if (!File.Exists("tagok.csv")) File.WriteAllText("tagok.csv", "id;nev;kor;utolso_tagsagujitas;tagsag_lejarata;kikolcsonzott_konyvek");
if (!File.Exists("kolcsonzes.csv")) File.WriteAllText("kolcsonzes.csv", "konyvid;tagid;datum;hatarido;meghosszabbitasok");