# RaupjcProjectJukebox

Projekt, Jukebox, je osmišljen za kreiranje Playlista (lista pjesama, tj. Songov-a) po Mood-ovima (osjećajima, ugođajima, žanrovima glazbe,
itd.) Svaka Playlista se sastoji od nekoliko Song-ova koje je moguće dodati s lokalnog računala (nužno je da file bude tipa .mp3). Za 
prikaz funkcionalnosti web-stranice u bazu je dodatno već nekoliko pjesama (izrezanih zbog ograničenosti veličine baze). Osim što se 
pjesma u bazu može dodati, može se koristiti i već ona koja se u njoj nalazi. Naime, svaki korisnik ima pristup, tj. mogućnost dodavanja 
svih pjesama iz baze u svoju Playlistu. Pretraživanje pjesama po bazi se filtrira po imenu autora pjesme (Artist) te po imenu pjesme 
(Title). Dovoljno je upisati u tražilicu manji niz znakova i rezultat će biti sve pjesme koje sadrže taj niz. Osim navedenih atributa 
(Artist i Title), pjesma može sadržavati i ime albuma (Album) te godinu izdanja (Year). 
Jednom dodana pjesma u playlist-u se ne može izbrisati iz nje. Kao ni Mood, koji, za razliku od Song-ova, ima isključivo jedan atribut i 
to je njegova vrijednost (tipa string koji daje opis svakoj playlisti) po kojoj se i razlikuje. Mood, kao što je već navedeno, može biti
bilo što (isključivo jedna riječ) - osjećaj (sreća/happy, tuga/sad), ugođaj (opuštenost/chill, zaljubljenost/love), itd. Također, svaki
Mood je dopustan svim korisnicima, ali isključivo ih admin može dodavati. Prilikom dodavanja Mood-a u playlistu, prikazano je prvih 
20 najkorištenijih, ali se tu nalazi i posebna tražilica kojom se pretražuje cijela baza i ispisuju svi Mood-ovi s traženom vrijednošću.
Nadalje, playlista, osim Mood-ova i Song-ova, se sastoji od još jednog obaveznog atributa i jednog neobaveznog. Ime playliste je obavezno
te isti korisnik ne smije imati dvije playliste jednakog imena, dok dva korisnika mogu imati dvije playliste jednakog imena. Potom, 
playlist-a također se sastoji i od slike (isključivo .jpg format), koja nije obavezna, ali služi kao simbol prepoznavanja.
Svaki korisnik, nakon ulogiravanja može otići na svoj profil (gornji desni kut, klikom na svoj Nickname) te tamo ima pregled svog
e-maila te nickname-a. Nadalje, ima mogućnost izmjene svoje lozinke te pregled svih svojih playlista, kao i njihovo stvaranje. Korisniku je
vidljiva još jedna mogućnost (Moods), ali ona je omogućena isključivo za admina koji u toj opciji može dodavati nove Mood-ove te ima prikaz
svih dosadašnjih (prikazani su u tablici zajedno s brojem frekvencije - broj pojavljivanja u playlistama). U opciji My Playlists, admin
ima pristup svim playlistama u bazi te ih može sve obrisati, dok korisnik ima pristup samo svojima te iste može obrisati.
Konačno, na naslovnoj stranici (klikom na Jukebox u gornjem lijevom kutu) korisnik je preusmjeren na naslovnicu - glavnu tražilicu. Od tamo
može upisati nekoliko Mood-ova (razmaknutih razmakom) te će mu se izlistati sve playliste koje sadrže barem jedan od upisanih Mood-ova. 
Svaka playlista je prikazana s imenom (Name), svojim odgovarajućim Mood-ovima (Moods), brojem Song-ova u sebi (Tracks), prikazom slike 
(ukoliko postoji) te opcijom Listen koja pokreće odabranu playlistu u njenom vlastitom player-u. U player-u su prikazani isti atributi, 
ali je uz njih još dodan popis svih pjesama redom ispisanih te numeriranih. Pjesme se pokreću automatski (na autoplay) te ih je moguće
zaustaviti, ali njihov odabir nije moguć. Naime, kada završi prva pjesma s popisa, pokreće se druga, potom treća, itd. Nakon završetka svih
pjesama player staje te se potrebno preusmjeriti nazad u Jukebox (ukoliko korisnik želi slušati playliste) ili u svoj profil ukoliko želi
dodati nove.

link: https://raupjcprojectjukebox.azurewebsites.net

admin login
- nickname: admin123
- password: Admin123$%&
