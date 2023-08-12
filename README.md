# DungeonHeroes

Wykorzystane assety: https://www.kenney.nl/assets/tiny-dungeon

#### Założenia gry
Prototyp gry polegający na jak najdłuższym przetrwaniu. Gracz zdobywa doświadczenie poprzez zabijanie pojawiających się przeciwników. Po osiągnięciu kolejno: 5, 10 i 15 poziomu odblokowywane są nowe bronie o różnych statystykach.

#### Aspekt techniczny
* Projekt został zrealizowany na silniku Unity przy podejściu zorientowanym na danych - ECS, w wersji 1.0.11 oraz wykorzystując technologie i paczki wchodzące w skład DOTS. Dodatkowo do zarządzania interfejsem użytkownika wykorzystano podejście hybrydowe.
* W projekcie zastosowano następujące wzorce projektowe: singleton (jako klasa abstrakcyjna, do komunikacji encji z obiektami odpowiadającymi za sterowanie interfejsem użytkownika, ale też ECS'owy singleton - np. jako gracz), obserwator (w postaci ECS'owych komponentów, używanych np. do informowania o śmierci gracza).
* Z zastosowanych elementów silnika użyto: elementy wchodzące w skład DOTS - ECS (Entites, Components, Systems, EntityCommandBuffer, Aspects, DynamicBuffers, BlobAssets), Burst, Jobs, Unity Physics for ECS, ale też elementy takie jak ObjectPooling (do gromadzenia informacji o zadanych obrażeniach przeciwnikom), new input system, 2D Tilemaps.
* Na ostatnim screenshot'cie przedstawiono uzyskaną wydajność na laptopie z procesorem Intel Core i5 8300H oraz kartą graficzna GTX 1050.

### Windows build
https://github.com/MaciejRokicki/EndlessRunner/releases/download/v1.0.0/EndlessRunner.zip

### Screenshots
![](/../master/Media/1.png)
![](/../master/Media/2.png)
![](/../master/Media/3.png)
![](/../master/Media/4.png)
![](/../master/Media/5.png)
