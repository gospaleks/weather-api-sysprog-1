# Sistemsko programiranje - Projekat 1 - Weather API

Realizacija pomoću ThreadPool-a.

U AppSettings.json se nalaze podešavanja servera. Veličina keša je stavljena na 2 radi testiranja.

Primer poziva serveru: <br/>
http://localhost:8080/q=Niš&days=5&aqi=yes&alerts=yes

## Zadatak 15:
Kreirati Web server koji klijentu omogućava prikaz vremenske prognoze uz pomoć WeatherAPI-
a. Pretraga se može vršiti pomoću filtera koji se definišu u okviru query-a. Vremenska prognoza,
kao i podaci o kvalitetu vazduha se vraćaju kao odgovor klijentu (podaci o kvalitetu vazduha su
dostupni korišćenjem aqi parametra). Svi zahtevi serveru se šalju preko browser-a korišćenjem
GET metode. Ukoliko navedena prognoza ne postoji, prikazati grešku klijentu.

Način funkcionisanja WeatherAPI-a je moguće proučiti na sledećem linku: <br/> https://www.weatherapi.com/docs/

Primer poziva serveru: <br/> http://api.weatherapi.com/v1/forecast.json?key=&q=London&days=5&aqi=yes&alerts=no
