# travel_and_drinks
Drinks & Travels – Backend System

Drinks & Travels är ett backend-system byggt i C# .NET (Minimal API) med MySQL.
Applikationen fungerar som en rese- och upplevelseplattform där användare kan söka resmål, boka hotell och event samt lämna betyg (ratings). Administratörer kan hantera innehåll i systemet.

Tekniker

C# .NET Minimal API

MySQL

Thunder Client (testning)
Session-baserad autentisering
Funktionalitet
CRUD för Countries, Cities, Hotels och Events
Sökning med inkluderande filter (LIKE)
Bookings för hotell och event
Ratings på hotell och event
Admin-behörighet via users.admin
Relationer mellan tabeller med foreign keys
Databas
Databasen innehåller bland annat:
users (med admin-flagga)
countries
cities
hotels
events
bookings
hotel_ratings
event_ratings
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
