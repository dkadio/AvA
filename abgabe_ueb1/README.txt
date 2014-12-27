Architektur verteilter Anwendungen Übung 1 

###Erläuterung der Idee
Die Klasse Knoten.cs in übernimmt die Aufgabe eines Knoten innerhalb eines Graphens. Dieser besitzt alle Methoden die dafür erforlderlich sind.
Jeder Knoten liest beim starten alle Informationen aus der Datei "text.txt" und speichert diese in einer Liste aus Knoten ab.
Aus dieser Liste werden die Nachbarn gewählt. Welche Nachbarn das sind wird über die graphiz-Datei angegeben. Um Kontrollnachrichten zu verteilen
wird die erste Liste verwendet damit jeder Knoten erreicht wird. Eine Normale Nachricht wird über die Nachbarknotenliste verteilt. 

###Nachrichtenformat
Das Nachrichtenformat wird durch die Klasse Message.cs repräsentiert. Diese Klasse ist serialisierbar und wird zwischen den Knoten verendet. 
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

###Erläuterung der Softwarestruktur
Die Gesamte Übung umfasst 3 Programme.
	-Knoten		
		Das Knoten Programm umfasst dabei die Klassen Knoten.cs, Programm.cs und Message.cs
		Die Knotenklasse stellt verschiedene Funktionen zum einlesen von Informationen bereit, 
		wie das einlesen einer Graphviz-Datei oder einfach eine Liste von Informationen nach dem Schema "id Hostname:Port"
		Um die KnotenKlasse zu starten und die args über die Kommandozeile einzulesen existeirt die Klasse Programm.cs die
		über die main-Methode des Programms verfügt. Die Klasse Message.cs wird dafür verwendet um Informationen zwischen Klassen
		zu transportieren. 	

	-Steuerung
		Das Programm Steuerung der zur manuellen Steruerung der Knoten. Das Programm verfügt über eine main-Methode die die args der Kommandozeile einliest
		und eine Verbindung zum gewünschten Knoten herstellt und diesem eine Nachricht sendet.
		
	-Graphgen
		Das Programm Graphgen erstellt einen graphizdatei. Diese erwaretet als Kommandozeilenparameter die Anzahl der Knoten und die Anzahl der Kanten. 


###Hinweise auf Implementierungsbesonderheiten
	-Allgemein
		Die Kommunikation zwischen den Knoten findet über TCP statt, wobei die Verbindung direkt nach übertragen der Nachricht beendet wird. 
		Auf diese Weise kann darauf reagiert werden, wenn ein Knoten nicht erreichbar sein sollte. 

	-Grapghen
		Das Graphgen-Programm erstellt als erstes einen Graphen mit "Knotenanzahl-1 Kanten", damit der Graph auf jedenfall zusammenhägend ist. 
		Damit die Kantenanzahl größer als die Knoteanzahl ist wird sich die Differnz der von den zu erstellenden Kanten zu den erstellten Kanten gemerkt.
		Das heisst die "zu erstellende Knoten" - "Knotenanzahl-1". Im anschluss wird eine Zweite Schleife über diese Differenz iterieren und erstellt beliebige Kanten.
		Am Ende werden die Doppelten Kanten entfernt. Daher kann es sein das weniger Kanten erstellt werden als über die args angegeben wird.

###Typische Abläufe
Um mehrere Knoten zu Starten kann das Skript start.cmd ausgeführt werden mit den Parametern wie viele Knoten erstellt werden sollen
	- start.cmd "Knotenanzahl" "Belivecounter für Knoten"

Damit die Knoten wissen über welchen port, ip und id sie verfügen kann das Skript gentest.cmd ausgeführt werden. Es schreibt die Informationen "id localhost:port" in die test.txt
Dabei muss darauf geachtet werden das es nicht mehr als 99 Knoten sind da sonst der Port nicht mehr mit dem Port der test.txt übereinstimmt.
	- gentest.cmd 25  --> erstellt 25 einträge in der test.txt

Nach Ausführen der beiden Skripte (zuerst gentest.cmd) kann über das Programm Steuerung.exe eine Nachricht an einen beliebigen Knoten gesendet werden. 
Um einen Knoten als Initiator festzulegen, kann das Programm wie folgt benutzt werden:
	- Steuerung.exe "ctrl oder msg" init "port des Knotens" 



###Fazit
Ursprünglich war geplant das Programme über das mono-project auf einem Mac laufen zu lassen. Bis zu einer gewissen Knotenanzahl war dies auch Möglich. Allerdings treten bei
großen Graphen übertragungsprobleme auf. Daher empfinde ich die Entwicklung von C# Projekten unter Windows sehr viel angenehmer da dort die IDE Visual Studio verfügbar ist. 
Ausserdem habe denke ich, das das serialiseren mit XML sich durchaus einfacher realisieren lässt als mit JSON. Diese Folge ziehe ich daraus das ich bereits in der Bachelorthesis damit zu tun hatte. 
Zu dem mono-Project kann allerdings gesgat werden das es eine tolle idee ist und da .Net seit kurzem Open-Source ist wird sich in diese Richtung noch viel tun. 
Da ich bisher nur einige Datenbankabfragen mit C# realisiert habe und dies das erste Programm ist das ich bisher mit C# geschrieben habe muss ich sagen das die Entwicklung schnell geht und sehr einfach ist. Ausserdem gibt es eine sehr große und ausführliche Dokumentation zu der Sprache. 

	- Aufgabe 4
		Bei der Verteilung der Nachrichten im Graph sollte man davon ausgehen können, dass ein Knoten jedes Gerücht gleich oft hören sollte da sich die
		Verteilungsreihenfolge nicht ändert. Allerdings fällt direkt bei dem Initator Knoten auf das die Erste Nachricht die gesendet wird nicht sehr oft nocheinmal
		gehört wird. Diese Tatsache lässt sich dadurch erklären das jeder Knoten beim erhalten der ersten Nachricht erst seine Id an alle nachbar-Knoten sendet bevor er die 
		echte Nachricht weiter verteilt. Das bedeutet das Wenn der erste Knoten anfängt die echte nachricht zu senden fangen die Empäfnger dieser Nachricht erst an die Id zu		     senden. So hört jeder Nachbarknoten die Nachricht zuerst von dem Initator. Bei einer Weiteren Nachricht senden die Nachbarknoten direkt die Nachricht weiter und es
		ist wahrscheinlicher das ein Nachbarknoten des Initaors die Nachricht von einem anderen Knoten als dem Initaor zuerst hört. 
		Dies wird noch wahrscheinlicher wenn man die Nachbaranzahl verringert. 
		Bei allen Versuchen haben alle Knoten die Nachricht erhalten. Allerdings nicht alle haben jede Nachricht geglaubt je nachdem wie hoch der Counter dafür gesetzt war.
		In jedem Fall kann über diesen Versuch gesagt werden das es nicht sicher ist welcher Knoten von welchem Koten die Nachricht erhalten wird. Je nachdem welche Nachbarn		     zuerst angesprochen werden und welche bereits ihre id gesendet haben. So ist die vorhersage sehr schwer zu treffen. 
