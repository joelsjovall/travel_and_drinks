Drinks & Travels – Backend System

Drinks & Travels är ett backend-system byggt i C# .NET (Minimal API) med MySQL.
Applikationen fungerar som en rese- och upplevelseplattform där användare kan söka resmål, boka hotell och event samt lämna betyg (ratings). Administratörer kan hantera innehåll i systemet.

Tekniker

C# .NET Minimal API

MySQL

Thunder Client för testning

Session-baserad autentisering

Funktionalitet:

CRUD för Countries, Cities, Hotels och Events

Sökning med inkluderande filter (LIKE)

Bookings för hotell och event

Ratings på hotell och event

Admin-behörighet via users.admin

Relationer mellan tabeller med foreign keys

Databas

Databasen innehåller bland annat:

-users (med admin-flagga)

-countries

-cities

-hotels

-events

-bookings

-hotel_ratings

-event_ratings

Databasen är relationsbaserad och använder foreign keys för dataintegritet.

Autentisering & Roller

Inloggning sker via sessioner (user_id)

Admin styrs via kolumnen admin i users

Users kan boka och ratea

Admins kan skapa, uppdatera och ta bort innehåll

API – Exempel

GET /countries?search=uni

GET /cities?search=lon

GET /events?search=beer

POST /bookings

GET /hotels/{id}/ratings

POST /hotels/{id}/ratings

Kör applikationen genom att klona projektet:
-git clone <repo-url>
-cd travel_and_drinks

Skapa databasen och kör sql scriptet som finns i projektet för att skapa alla tabeller och datan
-server=127.0.0.1;
-uid=root;
-pwd=PASSWORD;
-database=drinks_and_travels;

Starta API:t med: 
-dotnet run

Testning

Projektet testas via Thunder Client

Alla endpoints kan testas manuellt (GET, POST, PUT, DELETE)

Admin-funktioner kräver att användaren har admin = true/admin = 1 




