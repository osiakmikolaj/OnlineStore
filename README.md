# Dokumentacja projektu OnlineStore
Wymagania
• Visual Studio 2022
• .NET 8 SDK
• SQL Server
Kod do pobrania - github.com/osiakmikolaj/OnlineStore
Instalacja Pakietów NuGet
Aby zainstalować wymagane pakiety NuGet, otwórz Menedżera Pakietów NuGet w Visual
Studio i zainstaluj następujące pakiety:
1. Microsoft.AspNetCore.Identity.EntityFrameworkCore
2. Microsoft.EntityFrameworkCore.SqlServer
3. Microsoft.EntityFrameworkCore.Tools
4. Newtonsoft.Json
Możesz również zainstalować te pakiety za pomocą konsoli Menedżera Pakietów NuGet:
▪ Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
▪ Install-Package Microsoft.EntityFrameworkCore.SqlServer
▪ Install-Package Microsoft.EntityFrameworkCore.Tools
▪ Install-Package Newtonsoft.Json
Konfiguracja
1. Konfiguracja Bazy Danych
W pliku appsettings.json skonfiguruj połączenie z bazą danych.
2. Migracje Bazy Danych
Wykonaj migracje bazy danych w konsoli menadżera pakietów, aby utworzyć
odpowiednie tabele:
▪ Add-Migration Initial
▪ Update-Database
2025 Mikołaj Osiak - 15253
Opis Działania Aplikacji
Aplikacja OnlineStore to sklep internetowy, który umożliwia użytkownikom przeglądanie
produktów, dodawanie ich do zamówienia oraz składanie zamówień. Aplikacja posiada
również panel administracyjny, który umożliwia zarządzanie produktami i użytkownikami.
Funkcje Użytkownika
Przeglądanie Produktów
Użytkownicy mogą przeglądać listę dostępnych produktów na stronie głównej sklepu.
Każdy produkt zawiera nazwę, opis oraz cenę.
Dodawanie Produktów do Zamówienia
Użytkownicy mogą dodawać produkty do zamówienia, wybierając ilość i klikając
przycisk "Add to Order". Produkty dodane do zamówienia są wyświetlane w sekcji "Your
Order".
Składanie Zamówienia
Po dodaniu produktów do zamówienia, użytkownicy mogą złożyć zamówienie, klikając
przycisk "Create Order". Zamówienie zostanie zapisane w bazie danych, a użytkownik
zostanie przekierowany do strony szczegółów zamówienia.
Funkcje Administratora
Zarządzanie Produktami
Administratorzy mogą dodawać, edytować i usuwać produkty. Formularze do
zarządzania produktami są dostępne na stronie sklepu.
Zarządzanie Użytkownikami
Administratorzy mogą nadawać rolę "Admin" innym użytkownikom za pomocą
kontrolera UserController.
