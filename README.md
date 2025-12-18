Drinks & Travels – Backend System

Drinks & Travels är ett backend-system byggt i C# .NET med Minimal API och MySQL som databas. Applikationen är tänkt att fungera som en rese- och upplevelseplattform där användare kan utforska olika destinationer, boka hotell och event samt lämna betyg på sina upplevelser. Systemet är uppbyggt som ett REST-API och testas helt via externa verktyg utan något grafiskt gränssnitt.

Projektet är utvecklat med fokus på tydlig struktur, relationsdatabas, rollhantering och sökfunktionalitet. API:t är uppdelat i flera resurser som tillsammans bygger upp hela bokningsflödet.

Applikationen är byggd med C# .NET Minimal API och använder MySQL som databas. All kommunikation sker via HTTP-endpoints och testas med Thunder Client. Autentisering hanteras med sessions, där inloggade användare identifieras via ett user_id och administratörer via en admin-flagga i users-tabellen.

Databasen är relationsbaserad och består av flera sammankopplade tabeller. Centrala tabeller är users, countries, cities, hotels, events och bookings. För betygssystemet används separata tabeller för hotel_ratings och event_ratings. Relationer mellan tabellerna säkras med foreign keys för att säkerställa dataintegritet, till exempel mellan countries och cities samt mellan bookings och hotels/events.

Systemet stödjer full CRUD-funktionalitet för de flesta resurser. Det går att skapa, läsa, uppdatera och ta bort länder, städer, hotell och event. Sökningar kan göras med inkluderande filter genom LIKE-sökningar, vilket gör det möjligt att hitta exempelvis länder, städer eller event baserat på delar av namn. Användare kan skapa bokningar som kopplas till både hotell och event, och dessa bokningar kan sedan hämtas med sammanslagen information via JOIN-frågor.

Ett betygssystem har implementerats där användare kan lämna ratings på både hotell och event. Ratings kan hämtas per hotell eller event, och nya betyg kan skapas via separata POST-endpoints. Detta gör det möjligt att visa tidigare användares omdömen och poäng.

Autentisering sker via sessions. När en användare loggar in lagras user_id i sessionen. Administratörsbehörighet hanteras via en admin-kolumn i users-tabellen. Endpoints som förändrar systemets innehåll, till exempel skapande eller borttagning av länder och städer, är skyddade så att endast användare med admin-behörighet kan utföra dessa operationer.

För att köra applikationen lokalt behöver projektet klonas från GitHub och en MySQL-databas skapas. Ett SQL-script används för att skapa tabeller och lägga in testdata. Connection string i Program.cs måste uppdateras så att den pekar på rätt databas och användare. När detta är gjort kan applikationen startas med kommandot dotnet run, vilket startar API:t på http://localhost:5000
.

All funktionalitet testas via Thunder Client. Där kan man stegvis demonstrera hur GET, POST, PUT och DELETE fungerar för olika resurser i systemet. Admin- och användarroller kan också demonstreras genom att anropa endpoints med olika user_id.

Projektet har utvecklats agilt i team. Arbetet har delats upp i issues och feature-branches, där olika delar av systemet har utvecklats parallellt. Funktionalitet har testats löpande och justerats efter behov, vilket har gjort att systemet kunnat byggas upp steg för steg på ett kontrollerat sätt.

Sammanfattningsvis är Drinks & Travels ett komplett backend-projekt som visar hur man bygger ett REST-API med C# .NET, MySQL, rollhantering, sökfunktioner, bokningslogik och ratings, samtidigt som ett agilt arbetssätt har använts genom hela utvecklingsprocessen.
