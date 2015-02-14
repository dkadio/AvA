Architektur verteilter Anwendungen Übung 2

###Erläuterung der Idee
Die Klasse Knoten.cs übernimmt die Aufgabe eines Knoten innerhalb eines Graphens. Dieser besitzt alle Methoden die dafür erforlderlich sind.
Jeder Knoten liest beim starten alle Informationen aus der Datei "text.txt" und speichert diese in einer Liste aus Knoten ab.
Aus dieser Liste werden die Nachbarn gewählt. Welche Nachbarn das sind wird über die graphiz-Datei angegeben. Um Kontrollnachrichten zu verteilen
wird die erste Liste verwendet damit jeder Knoten erreicht wird. Eine Normale Nachricht wird über die Nachbarknotenliste verteilt. 

Für Uebung 2 kommen die zwei weitere Knoten Klassen hinzu (CusomerNode.cs und Busiunessnode.cs). Beide Erben von der Klasse aus der ersten Übung.
Damit der Firmenknoten seine Produkte vertreiben kann sendet dieser den Namen + seine Id als Nachrichteninhalt an seine Customer-Nachbarknoten.
Diese extrahieren aus dieser Nachricht von wem das Produkt stammt und handeln entsprechend. 

Eine Kampagne wird immer dann gestartet wenn sobald ein Busiunessnode initialisiert wird. Bevor jedoch die Kampagne gestartet wird, wird der echo-Algorithmus ausgeführt.
Wenn dieser erfolgreich abgschlossen werden konnte, wird eine Kampange gestartet. 

Eine Kampagne verbraucht 1 Einheit des Etats eines Busiunessnode. Diese wird am Anfang zufaegllig ausgeaehlt.

Die Informationen ob es sich um einen Busiuness- oder Customernode handelt wird ebennfalls aus der "text.txt" gelesen. Hier wird die Datei einfach 
durch das Kürzel BID oder CID erweitert. Dadurch wird der Knoten beim starten entsprechend erstellt. 

Um die Terminierung sicher zu stellen existiert das Programm Observer. Dieses implementiert das Doppelzaehlverfahren und man kann sehen wie viele nachrichten 
gesendet und empfangen wurden. 

###Nachrichtenformat
Das Nachrichtenformat wird durch die Klasse Message.cs repräsentiert. Diese Klasse ist serialisierbar und wird zwischen den Knoten verwendet. 
Als Beispiel wird hier eine Kontrollnachricht in Serialisierter Form gezeigt.

<?xml version="1.0" encoding="utf-8"?>
<Message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <senderId>-1</senderId>
  <nachricht>Nachricht</nachricht>
  <typ>ctrl</typ>
</Message>

Eine Nachricht besteht aus den Attributen:
	-senderId	Die Id des Knoten der diese Nachricht gesendet hat/ Wenn die Id von dem Benutzer gesendet wird ist diese -1
	-nachricht	Der Inhalt der Nachricht. Bei einer KontrollNachricht sind drei Befehle zulässig: endall, end, init
	-typ		Der Typ der Nachricht, Aktuell unterscheidet ein Knoten zwischen Kontrollnachrichten (ctrl) und normalen Nachrichten (msg)

Die klasse Message defniniert verschiedene Konstanten fuer das Attribut typ um zu differenzieren um was es sich fuer eine Nachricht handelt. 

###Erläuterung der Softwarestruktur
Die Gesamte Übung umfasst 5 Programme.
	-Knoten		
		Das Knoten Programm umfasst dabei die Klassen Knoten.cs, Programm.cs, Message.cs, BusinessNode.cs, CustomerNode.cs und Produkt.cs
		Die Knotenklasse stellt verschiedene Funktionen zum einlesen von Informationen bereit, 
		wie das einlesen einer Graphviz-Datei oder einfach eine Liste von Informationen nach dem Schema "id Hostname:Port Knotentyp"
		Um die KnotenKlasse zu starten und die args über die Kommandozeile einzulesen existeirt die Klasse Programm.cs die
		über die main-Methode des Programms verfügt. Die Klasse Message.cs wird dafür verwendet um Informationen zwischen Klassen
		zu transportieren. 
		Die Prodkt-Klasse dient als Informationsquelle wie oft ein Produkt gekauft, wie oft werbung darüber gehört oder wie oft es bereits von 
		anderen knoten gekauft wurde.
		Die Erweiterungsklassen fuer Knoten.cs dienen um die dazu die speziellen Funktionalitäten zu implementieren die fuer die beiden Knotentypen
		benötigt werden.

	-Steuerung
		Das Programm Steuerung der zur manuellen Steruerung der Knoten. Das Programm verfügt über eine main-Methode die die args der Kommandozeile einliest
		und eine Verbindung zum gewünschten Knoten herstellt und diesem eine Nachricht sendet.
		
	-Graphgen
		Das Programm Graphgen erstellt einen graphizdatei. Diese erwaretet als Kommandozeilenparameter die Anzahl der Knoten und die Anzahl der Kanten. 
	
	-GenHelper
		Der GenHelper erstellt eine passende Informationsdatei damit die Datei nicht von Hand selbst geschrieben werden muss. 
	
	-Observer
		Das Observer Programm besteht aus der klasse Observer.cs und aus Programm.cs. Wobei Programm cs die Prüfende Einheit darstellt die die Informationen
		von der als Threat gestarteten Observer-Klasse abruft.

###Hinweise auf Implementierungsbesonderheiten
	-Allgemein
		Die Kommunikation zwischen den Knoten findet über TCP statt, wobei die Verbindung direkt nach übertragen der Nachricht beendet wird. 
		Auf diese Weise kann darauf reagiert werden, wenn ein Knoten nicht erreichbar sein sollte. 

	-Grapghen
		Das Graphgen-Programm erstellt als erstes einen Graphen mit "Knotenanzahl-1 Kanten", damit der Graph auf jedenfall zusammenhägend ist. 
		Damit die Kantenanzahl größer als die Knoteanzahl ist wird sich die Differnz der von den zu erstellenden Kanten zu den erstellten Kanten gemerkt.
		Das heisst die "zu erstellende Knoten" - "Knotenanzahl-1". Im anschluss wird eine Zweite Schleife über diese Differenz iterieren und erstellt beliebige Kanten.
		Am Ende werden die Doppelten Kanten entfernt. Daher kann es sein das weniger Kanten erstellt werden als über die args angegeben wird.

	-EchoAlgorithmus
		Der EchoAlgorithmus wurde so implementiert das er die lediglich die CustomerKnoten mit einbezieht. Was in ungünstigen fällen dazu führen kann das einige
		Knoten als nicht "Teil des Graphes" behandelt werden. Dabei ist immer zu beachten das beim initialisieren des Algorithmus, die CustomerKnoten die von einem Busin	    	 ess Knoten ein Explorer erhalten, diesen in ihre EchoNachbarliste mit aufnehmen da sonst die Anzahl der zu erhalteneden echos verfälscht wird. 

###Typische Abläufe
Das Programm GenHelper kann ausgefuerht werden um eine Informationsdatei zu erstellen. wobei die Anzhal der gewuenschten Knoten, die Anzahl der BusinessNodes und CustomerNodes mitgegeben werden muss. 
	- "GenHelper.exe 25 3 22"

Dannach kann entsprechend das Gengraph Programm gestartet werden das die graphizdatei generiert. 
 	- "Gengraph.exe Knotenanzahl kantenanzahl"

Das Start.cmd ist ein Batch-Skript das alle Knoten startet entsprechend der Menge die in der Informationsdatei stehen. 
	- "Start.cmd"

Nach Ausführen der Skripte kann über das Programm Steuerung.exe eine Nachricht an einen beliebigen Knoten gesendet werden. 
Um einen Knoten als Initiator festzulegen, kann das Programm wie folgt benutzt werden:
	- Steuerung.exe "ctrl oder msg" init "port des Knotens" 

Das Observer Programm kann einfach gestaret werden. Dieses faengt dann an alle Knoten nacheinander zu Fragen wie hoch ihre zaehlerstaende sind.
	- "Observer.exe"

###Fazit
Ursprünglich war geplant das Programme über das mono-project auf einem Mac laufen zu lassen. Bis zu einer gewissen Knotenanzahl war dies auch Möglich. Allerdings treten bei
großen Graphen übertragungsprobleme auf. Daher empfinde ich die Entwicklung von C# Projekten unter Windows sehr viel angenehmer da dort die IDE Visual Studio verfügbar ist. 
Ausserdem denke ich, dass das serialiseren mit XML sich durchaus einfacher realisieren lässt als mit JSON. Diese Folge ziehe ich daraus das ich bereits in der Bachelorthesis damit zu tun hatte. 
Zu dem mono-Project kann allerdings gesgat werden das es eine tolle idee ist und da .Net seit kurzem Open-Source ist wird sich in diese Richtung noch viel tun. 
Da ich bisher nur einige Datenbankabfragen mit C# realisiert habe und dies das erste Programm ist das ich bisher mit C# geschrieben habe muss ich sagen das die Entwicklung schnell geht und sehr einfach ist. Ausserdem gibt es eine sehr große und ausführliche Dokumentation zu der Sprache. 

###Besonderheiten beim Testen
Beim Testen und Ausprobieren des Programms ist mir aufgefallen, dass jenachdem wie die Kaufschwellen der CustomerKnoten gewaehlt werden(random(1,10)), kann es bei einem niedrigen Wert fuer die Etats zu einem direkten Stillstand des Programms kommen da die CustomerKnoten keine der Produkte kauft, somit sich das Etat auch nicht erhoeht und die BusinessKnoten zum stillstand kommen. Das selbige gilt fuer niedrige Werte dafuer wie oft ein Produkt gekauft werden darf. Wenn fuer ein Produkt diese Schwelle erreicht wurde faellt die Möglichkeit raus, dass andere Knoten die Produkte wegen der t2 schwelle kaufen.
